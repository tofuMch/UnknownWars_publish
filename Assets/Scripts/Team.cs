using UnityEngine;

public class Team{
  private static int INITIAL_NUM_TEAM_CHARACTER;
  private static int INITIAL_CONTROL_ID = 0;
  private int prefabNumber;
  private string team_name;
  
  private GameObject[] characters;
  private CharacterConditionScript[] ccs;
  private bool[] isAliveList;
  private int countCharacter;

  public void SetCharacters(int i, GameObject input) {
    characters[i] = input;
    ccs[i] = characters[i].GetComponent<CharacterConditionScript>();
    ccs[i].SetTeam(team_name);
    ccs[i].SetId(i);
  }
  public GameObject GetCharacters(int i) { return characters[i]; }

  private int controlId;

  public string GetTeamName() { return team_name; }

  public int GetControlId() { return controlId; }
  public void SetPrefabNumber(int num){ prefabNumber = num; }
  public int GetPrefabNumber() { return prefabNumber; }
  public void SetAliveList(int id, bool isAlive){ isAliveList[id] = isAlive; }
  public bool GetAliveList(int id){ return isAliveList[id]; }

  private int GetAliveNum() {
    int tmp = 0;
    for (int i = 0; i < INITIAL_NUM_TEAM_CHARACTER; i++) {
      if (GetAliveList(i)) { tmp++; }
    }

    Debug.Log($"{team_name}:alive {tmp}");
    return tmp;
  }
  public void ChangeControlCharacter(){
    ccs[ controlId ].SetPlayerControl(false);
    for(int i = 0; i < INITIAL_NUM_TEAM_CHARACTER; i++){
      int tmp = ( controlId + i ) % INITIAL_NUM_TEAM_CHARACTER;
      if( GetAliveList( tmp ) ){ controlId = tmp; break; }
    }
    ccs[ controlId ].SetPlayerControl(true);
  }

  public int GetNextId() {
    for(int i = 1; i < INITIAL_NUM_TEAM_CHARACTER; i++){
      int tmp = ( controlId + i ) % INITIAL_NUM_TEAM_CHARACTER;
      if (GetAliveList(tmp)) { return tmp; }
    }
    return -1;
  }
  
  public bool DecreaseTeamCount(){
    countCharacter--;
    if(GetAliveNum() <= 0) return true; // dead
    return false;
  }
  
  public Team( string teamName, int initChrNum, int initCntId, int prfNum) {
    this.team_name = teamName;
    INITIAL_NUM_TEAM_CHARACTER = initChrNum;
    INITIAL_CONTROL_ID = initCntId;
    prefabNumber = prfNum;
    characters = new GameObject[ INITIAL_NUM_TEAM_CHARACTER ];
    ccs = new CharacterConditionScript[INITIAL_NUM_TEAM_CHARACTER];
    isAliveList = new bool[ INITIAL_NUM_TEAM_CHARACTER ];
    countCharacter = INITIAL_NUM_TEAM_CHARACTER;
    controlId = INITIAL_CONTROL_ID;
  }
}
