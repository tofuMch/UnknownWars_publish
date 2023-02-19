using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkillSettingScript : MonoBehaviour {
    [SerializeField] private GameObject titleButtonObject;
    [SerializeField] private GameObject matchingButtonObject;
    private CustomButton titleButton;
    private CustomButton matchingButton;
    [SerializeField] private string titleSceneName;
    [SerializeField] private string matchingSceneName;

    private const int kNumCharacterButton = 8;
    [SerializeField] private GameObject[] characterButtonObjects = new GameObject[kNumCharacterButton];
    private CustomButton[] characterButtons = new CustomButton[kNumCharacterButton];
    
    private CharacterChangeScript characterChangeComp;

    // Start is called before the first frame update
    void Start() {
        titleButton = titleButtonObject.GetComponent<CustomButton>();
        matchingButton = matchingButtonObject.GetComponent<CustomButton>();
        
        titleButton.onUpCallback = () => { ChangeScene(titleSceneName); };
        matchingButton.onUpCallback = () => { ChangeScene(matchingSceneName); };

        characterChangeComp = GetComponent<CharacterChangeScript>();
        for (int i = 0; i < kNumCharacterButton; i++)
        {
          characterButtons[i] = characterButtonObjects[i].GetComponent<CustomButton>();
          int character_i = i; // for の外で SelectCharacter(kNumCharacterButton)になるのを防ぐ
          characterButtons[i].onUpCallback = () => { characterChangeComp.SelectCharacter(character_i); };
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeScene(string sceneName){
      SceneManager.LoadScene(sceneName);
    }
}
