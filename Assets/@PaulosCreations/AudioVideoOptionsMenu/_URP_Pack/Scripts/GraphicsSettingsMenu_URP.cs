using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using System.IO;
using TMPro;
using PaulosVideoMenu_URP;

public class GraphicsSettingsMenu_URP : MonoBehaviour
{
    public enum SaveFormat { playerprefs, iniFile };
    [Space(10)]
    public SaveFormat saveFormat;

    [Header("Set True for IOS or Windows Store Apps.")]
    public bool usePersistentDatapath; //Use Persistent for "IOS" and "Windows Store Apps" or if you prefer to saves the file in a seperate persistent Folder.

    //[SerializeField]
    //private RenderPipelineAsset[] renderPipelines;//fill this with your pre made URP_Assets in the inspector;
    
    [Header("Select wich settings you want to use. Settings set to[UnUsed] can be removed from/disabled in the menu UI")]
    [Space(10)]
    [SerializeField]
    private SettingsUsedState qualityLevelUsed;
    [SerializeField]
    private SettingsUsedState resolutionUsed, windowedModeUsed, antiAliasingUsed, textureQualityUsed;

    [Header("Values to use on Reset or if no values are saved")]
    [SerializeField]
    private MenuVariables_URP DefaultSettings = new MenuVariables_URP();
    private MenuVariablesSimple_URP DefaultSettingsConverted = new MenuVariablesSimple_URP();
    private MenuVariablesSimple_URP CurrentSettings = new MenuVariablesSimple_URP();

    [Header("UI elements references")]
    [Space(10)]
    [SerializeField]
    private TMP_Text qualityLevelText;
    [SerializeField]
    private TMP_Text resolutionText, windowedModeText, textureQualityText;
    [SerializeField]
    private GameObject anisoLevelObj;

    private string saveFileDataPath;
    private List<Resolution> availableResolutions = new List<Resolution>();
    private int currentResolutionIndex;

    private bool initiated, isApplying;

    private void Awake()
    {
#if UNITY_EDITOR
        if (UnityEngine.EventSystems.EventSystem.current == null)
            Debug.LogWarning("There is no Event System in the scene !! UI Elements can not detect input.");
#endif
        //Use Persistent for "IOS" and "Windows Store Apps" or if you prefer to saves the file in a seperate persistent Folder.
        if (!usePersistentDatapath)
            saveFileDataPath = Application.dataPath + "/QualitySettings.ini";//puts the file in the games/applications folder.
        else saveFileDataPath = Application.persistentDataPath + "/QualitySettings.ini";

        //get available resolutions and filter them.
        Resolution[] availableResolutionsAll = Screen.resolutions;//checking the available resolution options.

        //we get every resolution with every available refreshrate, we only need the resolution ones.
        float resX = 0, resY= 0;
        for (int i = 0; i < availableResolutionsAll.Length; i++)
        {
            if (resX != availableResolutionsAll[i].width && resY != availableResolutionsAll[i].height)
            {
                resX = availableResolutionsAll[i].width;
                resY = availableResolutionsAll[i].height;

                availableResolutions.Add(availableResolutionsAll[i]);
            }
        }
        availableResolutionsAll = null;

        ConvertDefaultSettings();
        LoadMenuVariables();
        initiated = true;
    }

    //converting the easier to read settings class to the easyer to use in script settings class
    private void ConvertDefaultSettings()
    {
        DefaultSettingsConverted.Qualitylevel = DefaultSettings.Qualitylevel;

        if (DefaultSettings.Resolution.x == 0 || DefaultSettings.Resolution.y == 0)
        {
            DefaultSettingsConverted.ResolutionX = Screen.width;
            DefaultSettingsConverted.ResolutionY = Screen.height;
        }
        else
        {
            DefaultSettingsConverted.ResolutionX = DefaultSettings.Resolution.x;
            DefaultSettingsConverted.ResolutionY = DefaultSettings.Resolution.y;
        }

        switch (DefaultSettings.WindowedMode)
        {
            case FullScreenMode.ExclusiveFullScreen:
                DefaultSettingsConverted.WindowedMode = 0;
                break;
            case FullScreenMode.FullScreenWindow:
                DefaultSettingsConverted.WindowedMode = 1;
                break;
            case FullScreenMode.MaximizedWindow:
                DefaultSettingsConverted.WindowedMode = 2;
                break;
            case FullScreenMode.Windowed:
                DefaultSettingsConverted.WindowedMode = 3;
                break;
        }

        switch (DefaultSettings.TextureQuality)
        {
            case TextureQualityEnum.FullRes:
                DefaultSettingsConverted.TextureQuality = 0;
                break;
            case TextureQualityEnum.HalfRes:
                DefaultSettingsConverted.TextureQuality = 1;
                break;
            case TextureQualityEnum.QuarterRes:
                DefaultSettingsConverted.TextureQuality = 2;
                break;
            case TextureQualityEnum.EighthRes:
                DefaultSettingsConverted.TextureQuality = 3;
                break;
        }

        DefaultSettingsConverted.Warning = DefaultSettings.WarningMessage;
        CurrentSettings.Warning = DefaultSettings.WarningMessage;
    }

    #region Button functions
    public void UI_SetQualityLevel(int _addSubtract) //changes the general Quality setting without changing the Vsync,Antialias or Anisotropic settings.
    {
        if (qualityLevelUsed == SettingsUsedState.notUsed || isApplying)
            return;

        CurrentSettings.Qualitylevel += _addSubtract;
        CurrentSettings.Qualitylevel = Mathf.Clamp(CurrentSettings.Qualitylevel, 0, QualitySettings.names.Length-1);

        if (CurrentSettings.Qualitylevel != QualitySettings.GetQualityLevel())
        {
            //Changing Quality Levels overrides all changed settings.
            //We have to apply all of them again.
            ApplySettings(CurrentSettings);
        }
    }

    public void UI_SetResolution(int _addSubtract)
    {
        if (resolutionUsed == SettingsUsedState.notUsed || isApplying)
            return;

        currentResolutionIndex += _addSubtract;
    
        //loop around
        if (currentResolutionIndex < 0)
            currentResolutionIndex = availableResolutions.Count - 1;
        else if (currentResolutionIndex >= availableResolutions.Count)
            currentResolutionIndex = 0;

        CurrentSettings.ResolutionX = availableResolutions[currentResolutionIndex].width;
        CurrentSettings.ResolutionY = availableResolutions[currentResolutionIndex].height;

        //can`t change resolution without setting FullScreenMode.
        switch (CurrentSettings.WindowedMode)
        {
            case 0:
                Screen.SetResolution(CurrentSettings.ResolutionX, CurrentSettings.ResolutionY, FullScreenMode.ExclusiveFullScreen, 0);
                break;                                                                 
            case 1:                                                                    
                Screen.SetResolution(CurrentSettings.ResolutionX, CurrentSettings.ResolutionY, FullScreenMode.FullScreenWindow, 0);
                break;
            case 2:
                Screen.SetResolution(CurrentSettings.ResolutionX, CurrentSettings.ResolutionY, FullScreenMode.MaximizedWindow, 0);
                break;
            case 3:
                Screen.SetResolution(CurrentSettings.ResolutionX, CurrentSettings.ResolutionY, FullScreenMode.Windowed, 0);
                break;
        }

        resolutionText.SetText("{0}x{1}", CurrentSettings.ResolutionX, CurrentSettings.ResolutionY);
    }

    public void UI_SetWindowedMode(int _windowedMode)
    {
        if (windowedModeUsed == SettingsUsedState.notUsed || isApplying)
            return;

        CurrentSettings.WindowedMode = _windowedMode;

        switch (CurrentSettings.WindowedMode)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                windowedModeText.text = "FullScreen";
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                windowedModeText.text = "FullScreen Windowed";
                break;
            case 2:
                Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
                windowedModeText.text = "Maximized Windowed";
                break;
            case 3:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                windowedModeText.text = "Windowed";
                break;
        }
    }

    public void UI_SetTextureQuality(int _textureQuality)
    {
        if (textureQualityUsed == SettingsUsedState.notUsed || isApplying)
            return;

        CurrentSettings.TextureQuality = _textureQuality;
        QualitySettings.masterTextureLimit = CurrentSettings.TextureQuality;

        switch (CurrentSettings.TextureQuality)
        {
            case 0:
                textureQualityText.text = "Full";
                break;
            case 1:
                textureQualityText.text = "Half";
                break;
            case 2:
                textureQualityText.text = "Quarte";
                break;
            case 3:
                textureQualityText.text = "Eighth";
                break;
        }
    }

    public void UI_ResetToDefault()
    {
        if (isApplying)
            return;

        ApplySettings(DefaultSettingsConverted);       
    }

    //called when GraphicsMenu UIPanel is disabled or the menu is closed
    public void UI_SaveSettings()
    {
        if (!initiated || isApplying)
            return;

        SaveMenuVariables();
    }
    #endregion

    private void LoadMenuVariables()
    {
        if (saveFormat == SaveFormat.playerprefs)
        {
            if (PlayerPrefs.HasKey("Qualitylevel"))//to check if there are playerprefs saved.
            {
                MenuVariablesSimple_URP newMenuVars = new MenuVariablesSimple_URP();

                newMenuVars.Qualitylevel = PlayerPrefs.GetInt("Qualitylevel");
                newMenuVars.ResolutionX = PlayerPrefs.GetInt("ResolutionX");
                newMenuVars.ResolutionY = PlayerPrefs.GetInt("ResolutionY");
                newMenuVars.WindowedMode = PlayerPrefs.GetInt("WindowedMode");
                newMenuVars.TextureQuality = PlayerPrefs.GetInt("TextureQuality");

                ApplySettings(newMenuVars);

                newMenuVars = null;
            }
            else //no player prefs are saved.
            {
                //use the default values
                ApplySettings(DefaultSettingsConverted);
            }
        }
        else if (saveFormat == SaveFormat.iniFile)
        {
            if (File.Exists(saveFileDataPath))//to check if the file exists.
            {
                MenuVariablesSimple_URP newMenuVars = JsonUtility.FromJson<MenuVariablesSimple_URP>(File.ReadAllText(saveFileDataPath));

                ApplySettings(newMenuVars);

                newMenuVars = null;
            }
            else //no settings were saved.
            {
                //use the default values
                ApplySettings(DefaultSettingsConverted);
            }
        }
    }

    private void ApplySettings(MenuVariablesSimple_URP _varsLoaded)
    {
        isApplying = true;

        if (qualityLevelUsed == SettingsUsedState.used)
        {
            QualitySettings.SetQualityLevel(_varsLoaded.Qualitylevel);
            qualityLevelText.text = QualitySettings.names[_varsLoaded.Qualitylevel];
        }

        if (resolutionUsed == SettingsUsedState.used)
        {
            if (windowedModeUsed == SettingsUsedState.used)
            {
                switch (_varsLoaded.WindowedMode)
                {
                    case 0:
                        Screen.SetResolution(_varsLoaded.ResolutionX, _varsLoaded.ResolutionY, FullScreenMode.ExclusiveFullScreen, 0);
                        windowedModeText.text = "FullScreen";
                        break;
                    case 1:
                        Screen.SetResolution(_varsLoaded.ResolutionX, _varsLoaded.ResolutionY, FullScreenMode.FullScreenWindow, 0);
                        windowedModeText.text = "FullScreen Windowed";
                        break;
                    case 2:
                        Screen.SetResolution(_varsLoaded.ResolutionX, _varsLoaded.ResolutionY, FullScreenMode.MaximizedWindow, 0);
                        windowedModeText.text = "Maximized Windowed";
                        break;
                    case 3:
                        Screen.SetResolution(_varsLoaded.ResolutionX, _varsLoaded.ResolutionY, FullScreenMode.Windowed, 0);
                        windowedModeText.text = "Windowed";
                        break;
                }
            }
            else
            {
                Screen.SetResolution(_varsLoaded.ResolutionX, _varsLoaded.ResolutionY, Screen.fullScreenMode);
            }

            resolutionText.SetText("{0}x{1}", _varsLoaded.ResolutionX, _varsLoaded.ResolutionY);

            //finding the applied resolution index NR
            for (int i = 0; i < availableResolutions.Count; i++)
            {
                if (availableResolutions[i].width == _varsLoaded.ResolutionX && availableResolutions[i].height == _varsLoaded.ResolutionY)
                {
                    currentResolutionIndex = i;
                    break;
                }
            }
        }
        else if (windowedModeUsed == SettingsUsedState.used)
        {
            switch (_varsLoaded.WindowedMode)
            {
                case 0:
                    Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                    windowedModeText.text = "FullScreen";
                    break;
                case 1:
                    Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                    windowedModeText.text = "FullScreen Windowed";
                    break;
                case 2:
                    Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
                    windowedModeText.text = "Maximized Windowed";
                    break;
                case 3:
                    Screen.fullScreenMode = FullScreenMode.Windowed;
                    windowedModeText.text = "Windowed";
                    break;
            }
        }

        if (textureQualityUsed == SettingsUsedState.used)
        {
            QualitySettings.masterTextureLimit = _varsLoaded.TextureQuality;

            switch (_varsLoaded.TextureQuality)
            {
                case 0:
                    textureQualityText.text = "Full";
                    break;
                case 1:
                    textureQualityText.text = "Half";
                    break;
                case 2:
                    textureQualityText.text = "Quarte";
                    break;
                case 3:
                    textureQualityText.text = "Eighth";
                    break;
            }
        }

        CurrentSettings.Qualitylevel = _varsLoaded.Qualitylevel;
        CurrentSettings.ResolutionX = _varsLoaded.ResolutionX;
        CurrentSettings.ResolutionY = _varsLoaded.ResolutionY;
        CurrentSettings.WindowedMode = _varsLoaded.WindowedMode;
        CurrentSettings.TextureQuality = _varsLoaded.TextureQuality;

        isApplying = false;
    }

    private void SaveMenuVariables()
    {
        if (saveFormat == SaveFormat.playerprefs)
        {
            PlayerPrefs.SetInt("Qualitylevel", CurrentSettings.Qualitylevel);
            PlayerPrefs.SetInt("ResolutionX", CurrentSettings.ResolutionX);
            PlayerPrefs.SetInt("ResolutionY", CurrentSettings.ResolutionY);
            PlayerPrefs.SetInt("WindowedMode", CurrentSettings.WindowedMode);
            PlayerPrefs.SetInt("TextureQuality", CurrentSettings.TextureQuality);
        }
        else if (saveFormat == SaveFormat.iniFile)
        {
            #region Setting the correct values for settings the are not used but will show on the ini file .
            MenuVariablesSimple_URP menuVarsToSave = new MenuVariablesSimple_URP();

            if (qualityLevelUsed == SettingsUsedState.used)
                menuVarsToSave.Qualitylevel = CurrentSettings.Qualitylevel;
            else menuVarsToSave.Qualitylevel = QualitySettings.GetQualityLevel();

            if (resolutionUsed == SettingsUsedState.used)
            {
                menuVarsToSave.ResolutionX = CurrentSettings.ResolutionX;
                menuVarsToSave.ResolutionY = CurrentSettings.ResolutionY;
            }
            else
            {
                menuVarsToSave.ResolutionX = Screen.currentResolution.width;
                menuVarsToSave.ResolutionY = Screen.currentResolution.height;
            }

            if (windowedModeUsed == SettingsUsedState.used)
                menuVarsToSave.WindowedMode = CurrentSettings.WindowedMode;
            else
            {
                switch (Screen.fullScreenMode)
                {
                    case FullScreenMode.ExclusiveFullScreen:
                        menuVarsToSave.WindowedMode = 0;
                        break;
                    case FullScreenMode.FullScreenWindow:
                        menuVarsToSave.WindowedMode = 1;
                        break;
                    case FullScreenMode.MaximizedWindow:
                        menuVarsToSave.WindowedMode = 2;
                        break;
                    case FullScreenMode.Windowed:
                        menuVarsToSave.WindowedMode = 3;
                        break;
                }
            }

            if (textureQualityUsed == SettingsUsedState.used)
                menuVarsToSave.TextureQuality = CurrentSettings.TextureQuality;
            else menuVarsToSave.TextureQuality = QualitySettings.masterTextureLimit;

            menuVarsToSave.Warning = DefaultSettingsConverted.Warning;
            #endregion

            File.WriteAllText(saveFileDataPath, JsonUtility.ToJson(menuVarsToSave, true));
        }
    }
}

//custom classes
namespace PaulosVideoMenu_URP
{
    //easier to read and adjust in the inspector
    [System.Serializable]
    public class MenuVariables_URP
    {
        public int Qualitylevel = 1;

        [Header("Setting one or both to Zero, will use the monitors/windows resolution.")]
        public Vector2Int Resolution = new Vector2Int(0, 0);
        [Range(0.1f, 2f)]
        public FullScreenMode WindowedMode = FullScreenMode.MaximizedWindow;
        public AntiAliasLevelEnum AntiAliaslevel = 0;
        public TextureQualityEnum TextureQuality = TextureQualityEnum.FullRes;

        [Header("A Warning for users changing the ini file.")]
        public string WarningMessage = "Edit this file at your own risk!";
    }

    //easier to use in script
    [System.Serializable]
    public class MenuVariablesSimple_URP
    {
        public int Qualitylevel;
        public int ResolutionX, ResolutionY;
        public int WindowedMode;
        public int TextureQuality;

        public string Warning;
    }

    public enum SettingsUsedState { used, notUsed };
    public enum AntiAliasLevelEnum { off, x2, x4, x8 };
    public enum TextureQualityEnum { FullRes, HalfRes, QuarterRes, EighthRes };
    public enum AnisotropicLevelEnum { x2, x4, x8, x16 };
}