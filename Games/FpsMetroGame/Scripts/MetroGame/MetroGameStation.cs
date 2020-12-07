using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using TMPro;

public class MetroGameStation : MetroGameSubsystem
{
    public string name;
    public int id;

    [SerializeField] private List<ResourceSpecs> initLocalResourceSpecs;
    private Dictionary<EResource, ResourceSpecs> localResourceSpecs = new Dictionary<EResource, ResourceSpecs>();

    public Dictionary<EResource, int> currQuantities = new Dictionary<EResource, int>();
    public Dictionary<EResource, float> currPrices = new Dictionary<EResource, float>();

    public void Start()
    {
        foreach (ResourceSpecs specs in metroGame.initGlobalResourceSpecs)
        {
            localResourceSpecs[specs.resource] = specs;
        }
        foreach (ResourceSpecs specs in initLocalResourceSpecs)
        {
            localResourceSpecs[specs.resource] = specs;
        }
        foreach (EResource res in metroGame.unlockedResources)
        {
            currQuantities[res] = 0;
            currPrices[res] = 0;
        }
        UpdateMarketData();
    }

    private int generatePrices(EResource res)
    {
        int priceMin = localResourceSpecs[res].priceRangeMin;
        int priceMax = localResourceSpecs[res].priceRangeMax;
        return UnityEngine.Random.Range(priceMin, priceMax);
    }

    private int generateQuantities(EResource res)
    {
        int qtyMin = localResourceSpecs[res].quantityMinPerTurn;
        int qtyMax = localResourceSpecs[res].quantityMaxPerTurn;
        return UnityEngine.Random.Range(qtyMin, qtyMax);
    }

    public void UpdateMarket()
    {
        UpdateMarketData();
        UpdateText();
    }

    public override void UpdateMarketData()
    {
        foreach (EResource res in metroGame.unlockedResources)
        {
            try
            {
                currQuantities[res] += generateQuantities(res);
            } catch (Exception e) {
                Debug.Log(res);
            }
            currPrices[res] = generatePrices(res);
            if(currQuantities[res] < 0) { currQuantities[res] = 0; }
        }
    }

    public override void UpdateText()
    {
        string isHere = metroGame.currStation == this.id ? "(<= You)" : "";
        string text = $"{id}: {name} {isHere}\n";
        text += $"===========================\n";
        foreach (EResource res in metroGame.unlockedResources)
        {
            text += $"{res, 9}: {currQuantities[res]} (${currPrices[res]} each)\n";
        }
        this.text.text = text;
    }

    public void Go()
    {
        metroGame.GoToStation(this.id);
    }
}
