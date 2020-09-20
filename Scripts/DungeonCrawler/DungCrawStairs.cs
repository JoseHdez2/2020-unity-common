using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class DungCrawStairs : MonoBehaviour
{
    enum StairsType { LoadNextLevel, LoadPrevLevel }
    private AbsDunCraLevelInterpreter levelInterpreter;
    [SerializeField] private StairsType type = StairsType.LoadNextLevel;

    private void Start(){
        levelInterpreter = FindObjectOfType<AbsDunCraLevelInterpreter>();
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") {
            other.gameObject.SetActive(false);
            LoadLevel();
        }
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) {
            other.gameObject.SetActive(false);
            LoadLevel();
        }
    }

    private void LoadLevel(){
        switch (type) {
            case StairsType.LoadNextLevel: levelInterpreter.LoadNextLevel(); break;
            case StairsType.LoadPrevLevel: levelInterpreter.LoadPrevLevel(); break;
        }
    }
}
