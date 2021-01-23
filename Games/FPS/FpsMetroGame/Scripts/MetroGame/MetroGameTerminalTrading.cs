using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MetroGameTerminalTrading : MetroGameSubsystem
{
    [Tooltip("Resource for this terminal.")]
    public EResource resource;

    public override void UpdateMarketData()
    {
        // metroGame.GetCurrStation().currPrices[
    }

    private int GetCurrStationQty() { return metroGame.GetCurrStation().currQuantities[resource]; }
    private float GetCurrStationPrice() { return metroGame.GetCurrStation().currPrices[resource]; }

    public override void UpdateText()
    {
        if(!metroGame.unlockedResources.Contains(resource)) { return; }
        string newText = $"{resource}\n---------\n";
        if(metroGame.currStation < 0){
            newText += $"Currently moving.";
        } else {
            int stationQty = metroGame.GetCurrStation().currQuantities[resource];
            float stationPrice = metroGame.GetCurrStation().currPrices[resource];
            int yourQty = metroGame.resources[resource];
            newText += $"Station has {stationQty}\nat ${stationPrice} each.\nYou have {yourQty}.";
        }
        this.text.text = newText;
    }

    public void Buy()
    {
        if (metroGame.currStation < 0) return;
        if(GetCurrStationQty() > 0 && GetCurrStationPrice() < metroGame.money)
        {
            if (resource == EResource.GUNS)
            {
                metroGame.CallPolice();
            }
            metroGame.GetCurrStation().currQuantities[resource] -= 1;
            metroGame.resources[resource] += 1;
            metroGame.money -= GetCurrStationPrice();
            UpdateText();
            metroGame.GetCurrStation().UpdateText();
        }
    }

    public void Sell()
    {
        if (metroGame.currStation < 0) return;
        if (metroGame.resources[resource] > 0)
        {
            metroGame.GetCurrStation().currQuantities[resource] += 1;
            metroGame.resources[resource] -= 1;
            metroGame.money += GetCurrStationPrice();
            UpdateText();
            metroGame.GetCurrStation().UpdateText();
        }
    }
}
