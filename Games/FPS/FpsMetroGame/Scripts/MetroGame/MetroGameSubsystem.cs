using UnityEngine;
using System.Collections;
using TMPro;

public abstract class MetroGameSubsystem : MonoBehaviour
{
    [SerializeField] protected TMP_Text text;
    protected MetroGame metroGame;

    private void Awake()
    {
        text = GetComponentInChildren<TMP_Text>();
        metroGame = FindObjectOfType<MetroGame>();
    }

    public abstract void UpdateMarketData();
    public abstract void UpdateText();
}