using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAreaScript : MonoBehaviour{
  private GameObject parent;
  private CharacterConditionScript ccs;
  private CharacterConditionScript colccs;
  private string myTag;
  private string enemyTag;
  private List<GameObject> attackEnemysList = new List<GameObject>();

  // Start is called before the first frame update
  void Start(){
    parent = transform.root.gameObject;
    ccs = parent.GetComponent<CharacterConditionScript>();
    myTag = ccs.GetTeam();
    enemyTag = (myTag == "Team0") ? "Team1" : "Team0";
  }

  // Update is called once per frame
  void Update(){

  }

  private bool AddAttacked(GameObject input){
    if(attackEnemysList.Contains(input)){
      return false;
    }else{
      attackEnemysList.Add(input);
      return true;
    }
  }

  public void ResetAttacked(){
    attackEnemysList.Clear();
  }

  private void OnTriggerEnter(Collider collision){
    //Debug.Log(collision.transform);
    if(collision.tag == enemyTag){
      Debug.Log(collision.gameObject.name);
      if(AddAttacked(collision.gameObject)){
        colccs = collision.gameObject.GetComponent<CharacterConditionScript>();
        colccs.DecreaseLifePoint();
      }
    }
  }
}
