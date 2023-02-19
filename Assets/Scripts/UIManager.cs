using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour{

	[SerializeField] private GameObject gameEndPanel;
	[SerializeField] private GameObject gameMenuPanel;
	[SerializeField] private GameObject normalPanel;

	[SerializeField] private GameObject menuButtonObject;
	[SerializeField] private GameObject closeMenuButtonObject;
	[SerializeField] private GameObject midTitleButtonObject;
	[SerializeField] private GameObject midSettingButtonObject;
	[SerializeField] private GameObject midMatchingButtonObject;
	[SerializeField] private GameObject endTitleButtonObject;
	[SerializeField] private GameObject endSettingButtonObject;
	[SerializeField] private GameObject endMatchingButtonObject;

	private CustomButton menuButton;
	private CustomButton closeMenuButton;
	private CustomButton midTitleButton;
	private CustomButton midSettingButton;
	private CustomButton midMatchingButton;
	private CustomButton endTitleButton;
	private CustomButton endSettingButton;
	private CustomButton endMatchingButton;

  // Start is called before the first frame update
  void Start()
  {
	  ActivateNormalPanel();

	  menuButton = menuButtonObject.GetComponent<CustomButton>();
	  closeMenuButton = closeMenuButtonObject.GetComponent<CustomButton>();

	  midTitleButton = midTitleButtonObject.GetComponent<CustomButton>(); 
	  midSettingButton = midSettingButtonObject.GetComponent<CustomButton>(); 
	  midMatchingButton = midMatchingButtonObject.GetComponent<CustomButton>();
	  
	  endTitleButton = endTitleButtonObject.GetComponent<CustomButton>(); 
	  endSettingButton = endSettingButtonObject.GetComponent<CustomButton>(); 
	  endMatchingButton = endMatchingButtonObject.GetComponent<CustomButton>();

	  menuButton.onUpCallback = () => { ActivateGameMenuPanel(); };
	  closeMenuButton.onUpCallback = () => { ActivateNormalPanel(); };
	  const string titleSceneName = "TitleSceneDark"; 
	  const string settingSceneName = "SkillSettingScene";
	  const string matchingSceneName = "MatchingScene";
	  midTitleButton.onUpCallback = () => { SceneManager.LoadScene(titleSceneName); }; 
	  midSettingButton.onUpCallback = () => { SceneManager.LoadScene(settingSceneName); }; 
	  midMatchingButton.onUpCallback = () => { SceneManager.LoadScene(matchingSceneName); };
	  endTitleButton.onUpCallback = () => { SceneManager.LoadScene(titleSceneName); }; 
	  endSettingButton.onUpCallback = () => { SceneManager.LoadScene(settingSceneName); }; 
	  endMatchingButton.onUpCallback = () => { SceneManager.LoadScene(matchingSceneName); };
  }

  // Update is called once per frame
  void Update(){

  }

  public void ActivateGameEndPanel(){
	  gameEndPanel.SetActive(true);
    gameMenuPanel.SetActive(false);
    normalPanel.SetActive(false);
  }

  public void ActivateGameMenuPanel(){
    gameEndPanel.SetActive(false);
    gameMenuPanel.SetActive(true);
    normalPanel.SetActive(false);
  }

  public void ActivateNormalPanel(){
    gameEndPanel.SetActive(false);
    gameMenuPanel.SetActive(false);
    normalPanel.SetActive(true);
  }

}
