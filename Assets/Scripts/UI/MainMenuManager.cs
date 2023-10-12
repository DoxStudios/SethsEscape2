using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    string sceneText = "";

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void UpdateSceneText()
    {
        sceneText = GetComponentInChildren<TMP_InputField>().text;
    }

    public void LoadSelectedScene()
    {
        if(sceneText != "")
        {
            SceneManager.LoadScene(sceneText);
        }
    }
}
