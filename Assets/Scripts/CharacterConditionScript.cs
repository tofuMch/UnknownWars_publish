using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterConditionScript : MonoBehaviour{
  // Start is called before the first frame update
  private int id;
  private int lifePoint;
  private float attackSpeed;
  // for test
  public bool isPlayerControl = false;
  private Transform trs;
  private Animator animator;
  private CharacterControllerScript controller;
  private string teamTag;
  private int numMyTeam;
  
  const int INITIAL_LIFE_POINT = 4;
  const int INITIAL_ATTACK_SPEED = 1;

  public void IncreaseAttackSpeed(){ attackSpeed++; }
  public void DecreaseAttackSpeed(){ attackSpeed--; }

  public Vector3 GetTransform(){ return trs.position; }
  public void SetId(int input){ id = input; }
  public void SetPlayerControl(bool input){
    isPlayerControl = input;
    if(controller == null){
      controller = GetComponent<CharacterControllerScript>();
    }
    controller.ChangeControlMode(input);
  }
  public bool GetPlayerControl(){ return isPlayerControl; }
  public void   SetTeam(string input){ teamTag = input; gameObject.tag = input;
    numMyTeam = Int32.Parse(input.Substring(input.Length - 1));
    Debug.Log(numMyTeam);
  }
  public string GetTeam(){ return teamTag; }
  public int GetLifePoint(){ return lifePoint; }

  [SerializeField] private AudioClip hitSound;
  AudioSource audioSource;

  public void DecreaseLifePoint(){
    lifePoint--;
    audioSource.PlayOneShot(hitSound);
    if(lifePoint >= 0){
      hpIcon.DecreaseLifeIcon();
      if(lifePoint == 0){Dead();
      }else{animator.SetTrigger("Gethit");}
    }
  }

  private void Dead(){
    animator.SetTrigger("Dead");
    Invoke("DestroyMe", 3f);
    
    bs.Teams[numMyTeam].SetAliveList(id, false);
    if ( bs.Teams[ numMyTeam ].DecreaseTeamCount() ) {
      bs.EndGame( bs.Teams[ (numMyTeam + 1) % 2 ].GetTeamName() );
    }
    if(isPlayerControl) {
      //Invoke("ChangeCharacter", 1.5f);
      controller.StopCharacterControl();
      StartCoroutine(MoveCameraToNextCharacter(bs.Teams[numMyTeam].GetNextId()));
    }
    //SetTeam("Dead");
  }

  private IEnumerator MoveCameraToNextCharacter(int nextCharacterId) {
    controller.SetCameraOnPlayerEnable(false);
    GameObject nextCharacter = bs.Teams[bs.GetNumMyTeam()].GetCharacters(nextCharacterId);
    Vector3 nextCharacterPosition = nextCharacter.transform.position;
    Vector3 curPos = controller.GetCameraPos();
    Vector3 nxtPos = controller.GetCameraPosFromPlayer(nextCharacterPosition);
    float d = Vector3.Distance(curPos, nxtPos);
    float speed = 4.5f * d * Time.deltaTime;
    while ((curPos != nxtPos) 
           && bs.Teams[bs.GetNumMyTeam()].GetAliveList(nextCharacterId) )
    {
      curPos = controller.GetCameraPos();
      nxtPos = controller.GetCameraPosFromPlayer(nextCharacterPosition);
      controller.SetCameraPos( Vector3.MoveTowards( curPos, 
                                                    nxtPos, 
                                                    speed));
      nextCharacterPosition = nextCharacter.transform.position;
      yield return null;
    }
    ChangeCharacter();
    controller.SetCameraOnPlayerEnable(true);
  }

  private void ChangeCharacter() {
    bs.Teams[bs.GetNumMyTeam()].ChangeControlCharacter();
  }

  private void DestroyMe(){
    Destroy(gameObject);
  }


  private GameObject battleSceneManager;
  private BattleScript bs;

  private GameObject HPManager;
  private HPIconScript hpIcon;

  void Start(){
    lifePoint = INITIAL_LIFE_POINT;
    attackSpeed = INITIAL_ATTACK_SPEED;
    trs = GetComponent<Transform>();
    animator = GetComponent<Animator>();
    controller = GetComponent<CharacterControllerScript>();
    battleSceneManager = GameObject.FindGameObjectWithTag("BattleSceneManager");
    bs = battleSceneManager.GetComponent<BattleScript>();

    HPManager = transform.GetChild(3).gameObject.transform.GetChild(0).gameObject;
    hpIcon = HPManager.GetComponent<HPIconScript>();

    audioSource = GetComponent<AudioSource>();
  }

  // Update is called once per frame
  void Update(){

  }
}
