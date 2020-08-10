using UnityEngine;
using System.Linq;

// Not to be confused with PauseMenuController.
public class PauseController : MonoBehaviour
{
    public bool gameIsPaused;

    public void PauseGame(bool pauseGame){
        if (pauseGame == gameIsPaused) return;
        gameIsPaused = pauseGame;
        FindObjectsOfType<Pausable>().ToList().ForEach(p => p.Pause(pauseGame));
    }
}
