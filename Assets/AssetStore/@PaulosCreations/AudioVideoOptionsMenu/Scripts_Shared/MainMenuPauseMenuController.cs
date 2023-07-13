using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPauseMenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject mainCanvasObj;
    [SerializeField]
    private GameObject mainMenuPanelObj, optionsPanelObj, graphicsPanelObj, audioPanelObj;
    [SerializeField]
    private GameObject closeGameImageObj;

    private float previousTimescale;
    private bool menuOpen;

    private void Start()
    {

        ButtonOpenMenu();
    }

    public void ButtonOpenMenu()
    {
        graphicsPanelObj.SetActive(false);
        audioPanelObj.SetActive(false);
        optionsPanelObj.SetActive(false);
        mainMenuPanelObj.SetActive(true);

        mainCanvasObj.SetActive(true);

        menuOpen = true;
    }

    public void ButtonQuitGame()
    {
        closeGameImageObj.SetActive(true);
        Application.Quit();
    }

    //for testing/Debugging.
    public void DeleteSavedSettings()
    {
        PlayerPrefs.DeleteKey("Qualitylevel");
        PlayerPrefs.DeleteKey("ResolutionX");
        PlayerPrefs.DeleteKey("ResolutionY");
        PlayerPrefs.DeleteKey("antiAliasSlider");
        PlayerPrefs.DeleteKey("RenderScale");
        PlayerPrefs.DeleteKey("WindowedMode");
        PlayerPrefs.DeleteKey("VSync");
        PlayerPrefs.DeleteKey("AntiAliaslevel");
        PlayerPrefs.DeleteKey("TextureQuality");
        PlayerPrefs.DeleteKey("AnisotropicMode");
        PlayerPrefs.DeleteKey("AnisotropicLevel");

        PlayerPrefs.DeleteKey("muted");
        PlayerPrefs.DeleteKey("mainVolume");
        PlayerPrefs.DeleteKey("fxVolume");
        PlayerPrefs.DeleteKey("musicVolume");
        PlayerPrefs.DeleteKey("speakerMode");
    }
}
