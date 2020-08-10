using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq;
using System;
using TMPro;
using ExtensionMethods;
using System.Collections.Generic;

public enum EMainMenuOpt
{
    START,
    CREDITS,
    OPTIONS,
    QUIT
}

public class TitleScreenMenuController : MenuController<EMainMenuOpt>
{
    private CreditsController creditsController;
    private GameObject optionsController;
    public TMP_Text versionText;

    public TMP_Text titleText;
    public ECase titleCase;

    private LevelLoader levelLoader;

    private List<RuntimePlatform> RuntimePlatformsQuittable = new RuntimePlatform[]{
        RuntimePlatform.WindowsPlayer,
        RuntimePlatform.OSXPlayer,
        RuntimePlatform.LinuxPlayer
    }.ToList();

    // Use this for initialization
    void Start()
    {
        levelLoader = FindObjectOfType<LevelLoader>();
        Time.timeScale = 1; // TODO for now.
        menuOptions = (EMainMenuOpt[])Enum.GetValues(typeof(EMainMenuOpt));
        if (!RuntimePlatformsQuittable.Contains(Application.platform)) {
            menuOptions = menuOptions.Where(opt => opt != EMainMenuOpt.QUIT).ToArray();
        }
        if (optionsController == null)
        {
            menuOptions = menuOptions.Where(opt => opt != EMainMenuOpt.OPTIONS).ToArray();
        }
        if (titleText != null) { titleText.text = Application.productName.FormatCase(titleCase); }
        if (versionText != null) { versionText.text = $"ver. {Application.version}"; }
    }

    void OnEnable()
    {
        FindObjectOfType<CreditsController>(includeInactive: true).enabled = false;
    }

    protected override void HandleOption(EMainMenuOpt menuOption)
    {
        switch (menuOption)
        {
            case EMainMenuOpt.START: levelLoader.LoadNextLevel(); break;
            case EMainMenuOpt.CREDITS: FindObjectOfType<CreditsController>(includeInactive: true).enabled = true; break;
            case EMainMenuOpt.OPTIONS: break;
            case EMainMenuOpt.QUIT: Application.Quit(); break;
        }
    }
}
