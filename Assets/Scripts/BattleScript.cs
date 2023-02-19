using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class BattleScript : MonoBehaviour{

    const float OBJ_INS_HEIGHT = 27.4f;
    const int NUM_FIELD_OBJ = 1000;
    const int NUM_TEAM_CHARACTER = 50;

    // const int NUM_FIELD_OBJ = 0;
    // private const int NUM_TEAM_CHARACTER = 2;

    const float MAX_FIELD_X = 150f;
    const float MIN_FIELD_X =   0f;
    const float MAX_FIELD_Z = 150f;
    const float MIN_FIELD_Z =   0f;

    const float SPOWN_WIDTH_Z = 30f;
    const float MIN_TEAM1_SPOWN_Z = MIN_FIELD_Z;
    const float MAX_TEAM1_SPOWN_Z = MIN_TEAM1_SPOWN_Z + SPOWN_WIDTH_Z;
    const float MAX_TEAM2_SPOWN_Z = MAX_FIELD_Z;
    const float MIN_TEAM2_SPOWN_Z = MAX_TEAM2_SPOWN_Z - SPOWN_WIDTH_Z;

    const int NUM_OBSTACLE_PREFAB = 21;
    const int NUM_CHARACTER_PREFAB = 8;
    
    const int INITIAL_CONTROL_ID = 0;

    const int NUM_TEAM = 2;/* = player count*/

    private int numMyTeam = 0;

    public int GetNumMyTeam() { return numMyTeam; }

    public GameObject uiManager;
    private UIManager um;

    [SerializeField] private GameObject[] obj = new GameObject[NUM_OBSTACLE_PREFAB];
    [SerializeField] private GameObject[] characterPrefab = new GameObject[NUM_CHARACTER_PREFAB];
    
    private bool isWin = false;
    public bool isWinner(){ return isWin; }

    const int NUM_LAYER_CHARACTER = 7;
    const int NUM_LAYER_OBS_NOCOL = 8;
    const int NUM_LAYER_OBS_COL = 9;

    Vector3 OnGround(float x, float z){
      Vector3 rPos = new Vector3(x, OBJ_INS_HEIGHT, z);
      Ray ray = new Ray(rPos, Vector3.down);
      Vector3 pos = rPos;
      RaycastHit hit;
      const int layerMask = ~( 1 << NUM_LAYER_CHARACTER | 1 << NUM_LAYER_OBS_NOCOL | 1 << NUM_LAYER_OBS_COL );
      if( Physics.Raycast(ray, out hit, (float)OBJ_INS_HEIGHT, layerMask) ){ pos = hit.point; }
      return pos;
    }
    
    public Team[] Teams = new Team[NUM_TEAM];

    void Start() {
      for (int i = 0; i < NUM_TEAM; i++) {
        string name = "Team" + i.ToString();
        Teams[i] = new Team(name, NUM_TEAM_CHARACTER, INITIAL_CONTROL_ID, i );
        Debug.Log($"generate {name}");
      }
      Teams[numMyTeam].SetPrefabNumber(CharacterChangeScript.GetMyCharacterNum());

      //instantiate obstacles
      for (int i = 0; i < NUM_FIELD_OBJ; i++) {
        Instantiate(obj[Random.Range(0, NUM_OBSTACLE_PREFAB)],
          OnGround(Random.Range(MIN_FIELD_X, MAX_FIELD_X), Random.Range(MIN_FIELD_Z, MAX_FIELD_Z)),
          Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up));
      }

      //instantiate characters
      for (int i = 0; i < NUM_TEAM_CHARACTER; i++) {
        Teams[0].SetCharacters(i, Instantiate(characterPrefab[Teams[0].GetPrefabNumber()],
                             OnGround(Random.Range(MIN_FIELD_X, MAX_FIELD_X), Random.Range(MIN_TEAM1_SPOWN_Z, MAX_TEAM1_SPOWN_Z)),
                                    Quaternion.identity));
        Teams[1].SetCharacters(i, Instantiate(characterPrefab[Teams[1].GetPrefabNumber()],
                              OnGround(Random.Range(MIN_FIELD_X, MAX_FIELD_X), Random.Range(MIN_TEAM2_SPOWN_Z, MAX_TEAM2_SPOWN_Z)),
                              Quaternion.AngleAxis(180, Vector3.up)));
        Teams[0].SetAliveList(i, true);
        Teams[1].SetAliveList(i, true);
      }

      //set player control
      Teams[numMyTeam].ChangeControlCharacter();
      
      //start coroutine
      for (int i = 0; i < NUM_TEAM_CHARACTER; i++) {
        Teams[0].GetCharacters(i).GetComponent<CharacterControllerScript>().StartCoroutine("EnableAgent");
        Teams[1].GetCharacters(i).GetComponent<CharacterControllerScript>().StartCoroutine("EnableAgent");
      }

      um = uiManager.GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

	 public void EndGame(string winnerTeamName) {
      isWin = winnerTeamName == Teams[numMyTeam].GetTeamName();
      um.ActivateGameEndPanel();
    }

    public void ChangeScene(string sceneName){
      SceneManager.LoadScene(sceneName);
    }
}
