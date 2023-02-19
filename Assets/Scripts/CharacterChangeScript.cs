using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterChangeScript : MonoBehaviour{
  private GameObject mainCamera;
  private const int CHARACTER_COUNT = 8;
  private int[] cameraPosX = new int[CHARACTER_COUNT];
  const float CAMERA_POS_Y = 1.3f;
  const int CAMERA_POS_Z = -2;

  const int CHARACTER_DISTANCE = 5;
  const int INITIAL_NUM_SELECT_CHARACTER = 0;

  public static int myCharacterNum = INITIAL_NUM_SELECT_CHARACTER;
  public static int GetMyCharacterNum(){ return myCharacterNum; }

  private void SetCameraPos(int num){mainCamera.transform.position = new Vector3(cameraPosX[num], CAMERA_POS_Y, CAMERA_POS_Z);}

  void Start(){
    mainCamera = Camera.main.gameObject;
    for(int i = 0; i < CHARACTER_COUNT; i++){
      cameraPosX[i] = CHARACTER_DISTANCE * i;
    }
    SetCameraPos(myCharacterNum);
  }

  void Update(){

  }

  public void SelectCharacter(int num){
    SetCameraPos(num);
    myCharacterNum = num;
  }
}
