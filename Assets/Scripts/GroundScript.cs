using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//#define OBJ_INS_HEIGHT 17.4

public class GroundScript : MonoBehaviour{
/*
  private Rigidbody rb;
  private Collider coll;
  private float distance;//ray distance

  public bool hasCollision;
  
  const float DISTANCE_OBSTACLE_GROUND = 0.001f;
*/
  // Start is called before the first frame update
  void Start(){
/*    rb = GetComponent<Rigidbody>();
    coll = GetComponent<Collider>();
    distance = DISTANCE_OBSTACLE_GROUND;
*/  }

/*  bool isGround = false;  */

  // Update is called once per frame
  void Update(){
/*    if( isGround == false ){
      Vector3 rayPosition = transform.position;
      Ray ray = new Ray(rayPosition, Vector3.down);
      isGround = Physics.Raycast(ray, distance);
      if( isGround ){
        rb.isKinematic = true;
        //if(!hasCollision)coll.isTrigger = true;
      }
    }
*/  }
}
