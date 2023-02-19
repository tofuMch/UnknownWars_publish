using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPIconScript : MonoBehaviour{
  [SerializeField] private GameObject lifeObj;
  private GameObject parent;
  private CharacterConditionScript ccs;

  private int lifePoint;

  // Start is called before the first frame update
  void Start(){
    parent = transform.root.gameObject;
    ccs = parent.GetComponent<CharacterConditionScript>();
    lifePoint = ccs.GetLifePoint();
    for(int i = 0; i < lifePoint; i++){
      Instantiate<GameObject>(lifeObj, transform);
    }
  }

	public void SetHPIcon(int num){
		//HP Icon count set here.
	}

  public void DecreaseLifeIcon(){
    lifePoint--;
    if(lifePoint > 0){
      transform.GetChild(lifePoint-1).gameObject.SetActive(false);
    }
  }

  // Update is called once per frame
  void Update(){

  }

}
