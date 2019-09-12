using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StartMenuButtons : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void PressStartButton()
    {
        Debug.Log("pressed the button");
        SceneManager.LoadScene(1);
    }

    public void PressExitButton()
    {
        Application.Quit();
    }
}
