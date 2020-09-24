using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

/*
 * Loads different scenes, but first disables stuff 
 * and waits for a screen wipe animation to finish.
 */
public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;

    private ImageWipe screenWipe;
    public List<GameObject> disableDuringLoad;
    InputMaster controls;

    private void Awake()
    {
        screenWipe = GameObject.FindGameObjectWithTag("ScreenWipe").GetComponent<ImageWipe>();
        controls = InputMasterSingleton.Get();
    }

    private void Start()
    {
        if (screenWipe) {
            screenWipe.ToggleWipe(false);
        }
        disableDuringLoad.ForEach(obj => obj.SetActive(true));
        controls.Enable();
    }

    public void StartLevel(){
        PauseMenuController.PauseGameStatic(true);
    }

    public void RestartLevel() => LoadLevel(CurLevel());
    public void LoadNextLevel() => LoadLevel(NextLevel());
    public void LoadPrevLevel() => LoadLevel(PrevLevel());
    public void LoadNextLevelOrTitle() => LoadLevel(NextLevelOrTitle());

    public void LoadLevel(int buildIndex){
        StartCoroutine(LoadLevelInner(buildIndex));
    }

    IEnumerator LoadLevelInner(int levelIndex)
    {
        Debug.Log($"Loading level: {levelIndex}.");
        controls.Disable();
        disableDuringLoad.ForEach(obj => obj.SetActive(false));
        if (transition) {
            transition.SetTrigger("Start");
            yield return new WaitForSeconds(transitionTime);
            // yield return new WaitWhile(() => transition.GetCurrentAnimatorStateInfo(0).IsName("Start"));
        } else if (screenWipe) {
            screenWipe.ToggleWipe(true);
            yield return new WaitUntil(() => screenWipe.isDone());
        }
        SceneManager.LoadScene(levelIndex);
    }

    private int CurLevel() => SceneManager.GetActiveScene().buildIndex;
    private int NextLevel() => CurLevel() + 1;
    private int PrevLevel() => CurLevel() - 1;
    private int NextLevelOrTitle() => LevelExists(NextLevel()) ? NextLevel() : 0;

    private bool LevelExists(int buildIndex) => buildIndex < SceneManager.sceneCountInBuildSettings;

}
