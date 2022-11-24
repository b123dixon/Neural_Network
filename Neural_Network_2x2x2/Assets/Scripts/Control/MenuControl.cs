using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{
    //simply for changing scenes and shutting down application
    public void ChangeScene(string newScene)
    {
        SceneManager.LoadScene(newScene);
    }
    public void CloseDown()
    {
        Application.Quit();
    }
    public void GoToLink()
    {
        Application.OpenURL("https://docs.google.com/document/d/1IxZJHJHN0iulzqdrRlhCHvhhn7ctN2i_qlNzbNL_lQw/edit?usp=sharing");
    }
}