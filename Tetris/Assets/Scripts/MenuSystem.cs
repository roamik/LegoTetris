using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;


public class MenuSystem : MonoBehaviour
{
    public GameObject mainMenuHolder;
    public GameObject optionsMenuHolder;

    public Slider volumeSlider;

    public float volume;

    //public Toggle[] resolutionToggles;
    //public int[] screenWidths;
    //int activeScreenResIndex;

    public Text levelText;

    void Start()
    {
       levelText.text = "0";
    }

    public void RetryButton()
    {
        SceneManager.LoadScene("LevelMain");
    }

    public void StartButton()
    {
        if(Game.startingLevel == 0)
        {
            Game.startingAtLevelZero = true;
        }
        else
        {
            Game.startingAtLevelZero = false;
        }

        SceneManager.LoadScene("LevelMain");
    }

    public void OptionsMenu()
    {
        mainMenuHolder.SetActive(false);
        optionsMenuHolder.SetActive(true);
    }

    public void MainMenu()
    {
        mainMenuHolder.SetActive(true);
        optionsMenuHolder.SetActive(false);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    //public void SetScreenResolution(int i)
    //{
    //    if (resolutionToggles[i].isOn)
    //    {
    //        activeScreenResIndex = i;
    //        float aspectRatio = 16 / 9f;
    //        Screen.SetResolution(screenWidths[i], (int)(screenWidths[i] / aspectRatio), false);
    //    }
    //}

    //public void SetFullScreen (bool isFullScreen)
    //{
    //    for (int i = 0; i < resolutionToggles.Length; i++)
    //    {
    //        resolutionToggles[i].interactable = !isFullScreen;
    //    }

    //    if(isFullScreen)
    //    {
    //        Resolution[] allResolutions = Screen.resolutions;
    //        Resolution maxResolution = allResolutions[allResolutions.Length - 1];
    //        Screen.SetResolution(maxResolution.width, maxResolution.height, true);
    //    }
    //    else
    //    {
    //        SetScreenResolution(activeScreenResIndex);
    //    }
    //}

    public void ChangedLevelSliderValue(float value)
    {
        Game.startingLevel = (int)value;
        levelText.text = value.ToString();
    }

}
