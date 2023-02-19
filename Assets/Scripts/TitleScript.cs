using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScript : MonoBehaviour{
    [SerializeField] private GameObject matchingButtonObject;
    [SerializeField] private GameObject settingButtonObject;
    [SerializeField] private GameObject exitButtonObject;
    private CustomButton matchingButton;
    private CustomButton settingButton;
    private CustomButton exitButton;
    [SerializeField] private string matchingSceneName;
    [SerializeField] private string settingSceneName;

    // Start is called before the first frame update
    void Start(){
        matchingButton = matchingButtonObject.GetComponent<CustomButton>();
        settingButton = settingButtonObject.GetComponent<CustomButton>();
        exitButton = exitButtonObject.GetComponent<CustomButton>();
        
        matchingButton.onUpCallback = () => { ChangeScene(matchingSceneName); };
        settingButton.onUpCallback = () => { ChangeScene(settingSceneName); };
        exitButton.onUpCallback = () => { ExitGame(); };
    }

    // Update is called once per frame
    void Update(){

    }

    public void ChangeScene(string sceneName){
      SceneManager.LoadScene(sceneName);
    }
    public void ExitGame(){
    	Application.Quit();
    }
}
