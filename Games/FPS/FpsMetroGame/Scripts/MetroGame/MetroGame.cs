using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable] public class DictionaryOfResourceAndInt : SerializableDictionary<EResource, int> { }

public class MetroGame : MonoBehaviour
{
    // Game configuration
    public List<ResourceSpecs> initGlobalResourceSpecs;

    // Metro resources
    public DictionaryOfResourceAndInt resources = new DictionaryOfResourceAndInt(); // resources in stock.
    public float money = 100; // player money.
    public int currStation;

    public List<EResource> unlockedResources;
    [Tooltip("Money you must have so that the resource is auto-unlocked in the next market change.")]
    public DictionaryOfResourceAndInt unlockResourcesRequirements;

    public TMP_Text textSmartphone;
    public TMP_Text[] textLongScreens;
    private MetroGameStation[] stations;
    private MetroGameTerminalTrading[] tradingTerminals;
    private FakeTunnelParticles fakeTunnelParticles;
    public void Awake()
    {
        foreach (EResource res in unlockedResources)
        {
            resources[res] = 0;
        }

        stations = GetComponentsInChildren<MetroGameStation>();
        tradingTerminals = GetComponentsInChildren<MetroGameTerminalTrading>();
        fakeTunnelParticles = FindObjectOfType<FakeTunnelParticles>();
        stations = stations.Select((station, index) => { station.id = index; return station; }).ToArray();
    }

    void Start() {
        StartCoroutine(CountUp());
    }

    public int marketDurationSecs = 60;
    public int secsToDestination = 0;
    private int elapsedSeconds = 0;

    public IEnumerator CountUp()
    {
        yield return new WaitForSeconds(1);
        if (elapsedSeconds % marketDurationSecs == 0) { UpdateMarket(); }
        ++elapsedSeconds;
        UpdateScreens();
        StartCoroutine(CountUp());
    }


    private string policeText = "Illegal transaction detected.\nPlease stay where you are.";
    internal void CallPolice()
    {
        resources = new DictionaryOfResourceAndInt();
        money = 0;
        StopAllCoroutines();
        StartCoroutine(CallPolice1());
        textLongScreens.ToList().ForEach(t =>
        {
            t.text = policeText;
            t.text = policeText;
        });
    }

    private IEnumerator CallPolice1()
    {
        StartCoroutine(CallPolice2());
        yield return new WaitForSeconds(10);
        SceneManager.LoadScene("TitleScreen");
    }

    public Image fadeToBlack;
    private IEnumerator CallPolice2()
    {
        fadeToBlack.color = fadeToBlack.color + new Color(0, 0, 0, 0.1f);
        yield return new WaitForSeconds(1);
        StartCoroutine(CallPolice2());
    }

    private void UpdateSmartphoneScreen()
    {
        textSmartphone.text = $"Money: ${money}.\n";
        foreach (EResource res in unlockedResources)
        {
            textSmartphone.text += $"{res,9}s: {resources[res]}\n";
        }
    }

    private void UpdateLongScreen(TMP_Text textLongScreen)
    {
        if (secsToDestination > 0)
        {
            textLongScreen.text = $"Arriving at {stations[newStationId].name} in {secsToDestination} seconds.\n";
        }
        else
        {
            if (currStation >= 0)
            {
                textLongScreen.text = $"You are in {GetCurrStation().name}.\n";
            }
        }

        int secs = marketDurationSecs - elapsedSeconds % marketDurationSecs;
        textLongScreen.text += $"Market will change in {secs} seconds.\n";
    }

    private void UpdateMarket()
    {
        stations.ToList().ForEach(s => s.UpdateMarket());

        UpdateScreens();
    }

    private void UpdateScreens()
    {
        stations.ToList().ForEach(s => s.UpdateText());
        tradingTerminals.ToList().ForEach(s => s.UpdateText());

        UpdateSmartphoneScreen();
        textLongScreens.ToList().ForEach(tls => UpdateLongScreen(tls));
    }

    public MetroGameStation GetCurrStation() { return stations[currStation]; }

    private int newStationId;
    public void GoToStation(int stationId)
    {
        if (secsToDestination > 0) return; // ignore impatient button pressers.
        fakeTunnelParticles.Resume();
        newStationId = stationId;
        currStation = -1;
        secsToDestination = 10;
        UpdateScreens();
        StartCoroutine(GoToStation());
    }

    private IEnumerator GoToStation()
    {
        if(secsToDestination <= 0)
        {
            fakeTunnelParticles.Pause();
            currStation = newStationId;
            UpdateScreens();
            yield break;
        }
        yield return new WaitForSeconds(1);
        --secsToDestination;;
        UpdateScreens();
        StartCoroutine(GoToStation());
    }
}
