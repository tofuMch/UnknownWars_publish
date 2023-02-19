using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScene : MonoBehaviour
{
    [SerializeField] private GameObject[] buttonObjects = new GameObject[4];
    private CustomButton[] buttons = new CustomButton[4];
    
    void Start()
    {
        for (int i = 0; i < 4; i++) {
            buttons[i] = buttonObjects[i].GetComponent<CustomButton>();
        }

        float step = 0f;
        foreach (CustomButton button in buttons) {
            button.SetDefaultButtonSize(1.0f + step);
            
            button.onEnterCallback = () => { };
            button.onExitCallback = () => { };
            button.onDownCallback = () => { };
            button.onUpCallback = () => { };
            
            step += 0.1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
