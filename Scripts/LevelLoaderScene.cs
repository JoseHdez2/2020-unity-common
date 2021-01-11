using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

/*
 * Loads different scenes, but first disables stuff 
 * and waits for a screen wipe animation to finish.
 */
public class LevelLoaderScene : AbsLevelLoader<SceneReference>
{
    public SceneReference nextLevel, prevLevel;
    // LevelLoaderScene
    protected override int GetSceneBuildIndex(SceneReference sceneRef) => SceneManager.GetSceneByPath(sceneRef.ScenePath).buildIndex; 
    protected override SceneReference NextLevel() => nextLevel;
    protected override SceneReference PrevLevel() => prevLevel;
    protected override SceneReference NextLevelOrTitle() => nextLevel; // TODO
    protected override bool LevelExists(SceneReference buildIndex) => buildIndex != null;
}
