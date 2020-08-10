using System;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Linq;

public enum EPauseMenuOpt {
    RESUME,
    RESTART,
    OPTIONS,
    QUIT_TO_TITLE
}

class PauseMenuController : MenuController<EPauseMenuOpt>
{
    public AudioSource gameplayMusic;
    public ToggleWithKey pauseToggle;
    
    // Start is called before the first frame update
    void Start() {
        Time.timeScale = 1; // TODO for now.
        menuOptions = (EPauseMenuOpt[])Enum.GetValues(typeof(EPauseMenuOpt));
    }

    private void OnEnable() { PauseGame(true); }

    private void OnDisable() { PauseGame(false); }

    private void PauseGame(bool paused) {
        if (gameplayMusic) {
            if (paused) {
                gameplayMusic.Pause();
            } else {
                gameplayMusic.UnPause();
            }
        }
        PauseGameStatic(paused);
    }

    public static void PauseGameStatic(bool paused) {
        Time.timeScale = paused ? 0 : 1;

        FindObjectsOfType<Pausable>().ToList().ForEach(p => p.Pause(paused));
    }

    protected override void HandleOption(EPauseMenuOpt menuOption) {
        switch (menuOption) {
            case EPauseMenuOpt.RESUME: pauseToggle.ToggleObject(); break;
            case EPauseMenuOpt.RESTART: SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); break;
            case EPauseMenuOpt.OPTIONS: break;
            case EPauseMenuOpt.QUIT_TO_TITLE: SceneManager.LoadScene(0); break;
        }
    }
}
