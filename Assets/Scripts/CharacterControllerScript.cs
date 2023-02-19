using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterControllerScript : MonoBehaviour{

  private float x, z;
  private float smooth = 10f;

  public Vector3 targetPos;

  const float SPEED_RUN = 10.0f;
  const float SPEED_WALK = 1.0f;
  public float speed = SPEED_RUN;

  private Rigidbody rb;
  private Animator animator;
  private CharacterConditionScript ccs;
  private CharacterConditionScript[] teamCcss;
  private Coroutine coroutine = null;
  private NavMeshAgent myAgent;

  private GameObject[] teamCharacters;
  private GameObject mainCamera;

  private GameObject attackArea;
  private AttackAreaScript aas;
  private Collider attackAreaCollider;

  private int framesFromPreviousSaveForward = 0;

  private Vector3 savedForwardVector;

  [SerializeField] private AudioClip attackSound;
  AudioSource audioSource;

  const float TIME_ATTACK_CONTINUE = 0.5f;/*attack remain during this time.*/

  // Start is called before the first frame update
  void Start() {
    isCameraOnPlayerEnabled = true;
    animator = GetComponent<Animator>();
    rb = GetComponent<Rigidbody>();
    ccs = GetComponent<CharacterConditionScript>();
    mainCamera = Camera.main.gameObject;
    teamCharacters = GameObject.FindGameObjectsWithTag(ccs.GetTeam());
    teamCcss = System.Array.ConvertAll(teamCharacters, c => c.GetComponent<CharacterConditionScript>());

    attackArea = transform.GetChild(2).gameObject;
    attackAreaCollider = attackArea.GetComponent<BoxCollider>();
    aas = attackArea.GetComponent<AttackAreaScript>();
    audioSource = GetComponent<AudioSource>();
  }

  // Update is called once per frame
  void Update(){


  }

  public IEnumerator EnableAgent(){
    for(int i=0;i<3;i++){
      yield return null;
    }
    myAgent = GetComponent<NavMeshAgent>();
    myAgent.enabled = true;
    myAgent.updatePosition = false;
    yield return new WaitForSeconds(0.1f);
    LaunchCharacterControl(ccs.GetPlayerControl());
  }

  void Move(float x, float z){
    float length_inv = ( x != 0 || z != 0 ) ? 1f/Mathf.Sqrt(x*x + z*z) : 0;
    Vector3 targetDir = new Vector3(x, 0, z);
    rb.velocity       = new Vector3(x*length_inv, 0, z*length_inv) * speed;
    animator.SetFloat("Walk", rb.velocity.magnitude);

    if(targetDir.magnitude > 0.1){
      Quaternion rotation = Quaternion.LookRotation(targetDir);
      transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * smooth);
    }
  }

  private bool isCameraOnPlayerEnabled;

  public void SetCameraOnPlayerEnable(bool input) { isCameraOnPlayerEnabled = input; }
  private IEnumerator PlayerCoroutine(){
    yield return new WaitForSeconds(0.1f);
    while(true){
      Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
      if(isCameraOnPlayerEnabled) MoveCamera();
      Attack(Input.GetMouseButtonDown(0));
      yield return null;
    }
  }

  private IEnumerator AICoroutine(){
    float distanceFOV = 20f;
    float distanceAttackField = 1.5f;
    Vector3 nextPoint;
    Vector3 myPos;
    yield return new WaitForSeconds(0.1f);
    while(true){
      GameObject enemyCharacter = GetEnemy(distanceFOV);
      myPos = transform.position;
      if(enemyCharacter == null){
        if(framesFromPreviousSaveForward++ == 0){
          savedForwardVector = transform.forward;
          targetPos = myPos + savedForwardVector * 10;
          myAgent.SetDestination(targetPos) ;
        }
        framesFromPreviousSaveForward %= 60;

        nextPoint = myAgent.steeringTarget;
        Move(nextPoint.x - myPos.x, nextPoint.z - myPos.z);
        // MoveForward();
        yield return null;
      }else{
        Vector3 enemyPos = enemyCharacter.transform.position;
        myAgent.SetDestination(enemyPos);
        yield return new WaitForSeconds(.1f);
        nextPoint = myAgent.steeringTarget;
        Move(nextPoint.x - myPos.x, nextPoint.z - myPos.z);
        if(GetEnemy(distanceAttackField) != null){
          Attack(true);
          yield return new WaitForSeconds(1f);
        }
      }
    }
  }

  public void LaunchCharacterControl(bool input){
    if(input){
      coroutine = StartCoroutine(nameof(PlayerCoroutine));
    }else{
      coroutine = StartCoroutine(nameof(AICoroutine));
    }
  }

  public void ChangeControlMode(bool input){
    if(coroutine!=null){
      StopCoroutine(coroutine);
    }
    LaunchCharacterControl(input);
  }

  public void StopCharacterControl(){
    if(coroutine!=null){
      StopCoroutine(coroutine);
      coroutine = null;
    }
  }

  GameObject GetEnemy(float distanceFOV){
    // field of vision
    string teamName = ccs.GetTeam();
    string enemyTeam = teamName == "Team0" ? "Team1" : "Team0";
    Vector3 myPos = this.transform.position;
    Vector3 enemyPos;
    GameObject[] enemyCharacters = GameObject.FindGameObjectsWithTag(enemyTeam);
    foreach(GameObject character in enemyCharacters){
      enemyPos = character.transform.position;
      if(Vector3.Distance(myPos, enemyPos) <= distanceFOV){
        return character;
      }
    }
    return null;
  }

  GameObject GetPlayerCharacter(){
    for(int i=0; i<teamCcss.Length; i++){
      if(teamCcss[i].GetPlayerControl()){
        return teamCharacters[i];
      }
    }
    return null;
  }

  void MoveCamera(){
    Vector3 playerPos = ccs.GetTransform();
    Vector3 cameraPos = GetCameraPosFromPlayer(playerPos);
    Vector3 cameraLook = new Vector3(0, 1f, 0);
    SetCameraPos(cameraPos);
    mainCamera.transform.LookAt(playerPos + cameraLook);
  }

  public void SetCameraPos(Vector3 pos) {
    mainCamera.transform.position = pos;
  }

  
  public Vector3 GetCameraPos() {
    return mainCamera.transform.position;
  }

  public Vector3 GetCameraPosFromPlayer(Vector3 player_pos) {
    Vector3 cameraPos = new Vector3(player_pos.x, player_pos.y + 1.5f, player_pos.z - 5);
    return cameraPos;
  }
  
  void Attack(bool isAttack){
    if(isAttack){
      if(!attackAreaCollider.enabled){
        animator.SetTrigger($"Attack{Random.Range(1, 4)}");/*attack motion has 4 type*/
        attackAreaCollider.enabled = true;
        speed = SPEED_WALK;
        Invoke("AttackColliderReset", TIME_ATTACK_CONTINUE);
        audioSource.PlayOneShot(attackSound);
      }
    }
  }

  private void AttackColliderReset(){
    attackAreaCollider.enabled = false;
    speed = SPEED_RUN;
    aas.ResetAttacked();
  }
}
