using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseBttn : MonoBehaviour {

    public void ReturnMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
