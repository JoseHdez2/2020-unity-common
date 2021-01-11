using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

/*
 * Loads different scenes, but first disables stuff 
 * and waits for a screen wipe animation to finish.
 */
public abstract class AbsLevelLoader<T> : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;

    private IToggleable screenWipe;
    public List<GameObject> disableDuringLoad;
    InputMaster controls;

    private void Awake(){
        screenWipe = GameObject.Find("ScreenWipe").GetComponent<IToggleable>();
        controls = InputMasterSingleton.Get();
    }

    private void Start(){
        if (screenWipe != null) {
            screenWipe.Toggle(false);
        }
        disableDuringLoad.ForEach(obj => obj.SetActive(true));
        controls.Enable();
    }

    public void StartLevel(){
        PauseMenuController.PauseGameStatic(true);
    }

    public void RestartLevel() => LoadLevel(CurScene().buildIndex);
    public void LoadNextLevel() => LoadLevel(GetSceneBuildIndex(NextLevel()));
    public void LoadPrevLevel() => LoadLevel(GetSceneBuildIndex(PrevLevel()));
    public void LoadNextLevelOrTitle() => LoadLevel(GetSceneBuildIndex(NextLevelOrTitle()));

    public void LoadLevel(int buildIndex){
        StartCoroutine(CrLoadLevel(buildIndex));
    }

    IEnumerator CrLoadLevel(int buildIndex) {
        if(controls != null){
            controls.Disable();
        }
        disableDuringLoad.ForEach(obj => obj.SetActive(false));
        if (transition) {
            transition.SetTrigger("Start");
            yield return new WaitForSeconds(transitionTime);
            // yield return new WaitWhile(() => transition.GetCurrentAnimatorStateInfo(0).IsName("Start"));
        } else if (screenWipe != null) {
            screenWipe.Toggle(true);
            yield return new WaitUntil(() => screenWipe.IsDone());
        }
        SceneManager.LoadScene(buildIndex);
    }
    protected abstract int GetSceneBuildIndex(T index);
    protected Scene CurScene() => SceneManager.GetActiveScene();
    protected abstract T NextLevel();
    protected abstract T PrevLevel();

    protected abstract T NextLevelOrTitle();
    protected abstract bool LevelExists(T buildIndex);
}

public class LevelLoader : AbsLevelLoader<int> {
    protected override int GetSceneBuildIndex(int buildIndex) => buildIndex;

    protected int CurLevelIndex(){
        Debug.Log($"CurLevelIndex: {SceneManager.GetActiveScene().buildIndex}");
        return SceneManager.GetActiveScene().buildIndex;
    }
    protected override int NextLevel() => CurLevelIndex() + 1;
    protected override int PrevLevel() => CurLevelIndex() - 1;
    protected override int NextLevelOrTitle() => LevelExists(NextLevel()) ? NextLevel() : 0;
    protected override bool LevelExists(int buildIndex) => buildIndex < SceneManager.sceneCountInBuildSettings;
}
