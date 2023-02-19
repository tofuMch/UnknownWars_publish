using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchingScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start(){
      Invoke("ChangeScene", 1.5f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeScene(){
      SceneManager.LoadScene("BattleScene");
    }
}
