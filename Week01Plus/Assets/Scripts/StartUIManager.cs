using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUIManager : MonoBehaviour
{
    public void OnClickGameStart()
    {
        SceneManager.LoadSceneAsync("WorldMap");
    }

    public void OnClickExit()
    {
        Application.Quit();
    }
}
