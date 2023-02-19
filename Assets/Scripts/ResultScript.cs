using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultScript : MonoBehaviour{

  [SerializeField] GameObject sceneManager;
  BattleScript bs;
  TextMeshProUGUI resultText;

  // Start is called before the first frame update
  void Start(){
    bs = sceneManager.GetComponent<BattleScript>();
    resultText = transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>();
    if( bs.isWinner() ){
      resultText.text = "You win!";
    }else{
      resultText.text = "You lose...";
    }
  }

  // Update is called once per frame
  void Update(){

  }
}
