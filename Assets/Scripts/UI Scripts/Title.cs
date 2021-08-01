using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public string sceneName = "GameStage";

    public void ClickStart()
    {
        Debug.Log("loading");
        SceneManager.LoadScene(sceneName);
    }

    public void ClickLoad()
    {
        Debug.Log("load");
    }

    public void ClickExit()
    {
        Debug.Log("exit");
    }
}