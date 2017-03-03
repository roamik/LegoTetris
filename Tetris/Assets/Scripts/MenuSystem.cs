using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuSystem : MonoBehaviour {

    public void RetryButton()
    {
        SceneManager.LoadScene("LevelMain");
    }

    public void StartButton()
    {
        SceneManager.LoadScene("LevelMain");
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
