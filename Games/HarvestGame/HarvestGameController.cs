using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HarvestGameController : MonoBehaviour
{
    [SerializeField] private TMP_Text textClock;
    [SerializeField] private ImageWipe imageWipe;
    private DateTime date = new DateTime(2000, 8, 1, 6, 00, 0); // 1st October 2000, 6am. I just needed a month starting on Sunday.
    public bool isPaused = false;
    [SerializeField] private RepeatingTimer timer = new RepeatingTimer(1f);

    private void Start() {
        imageWipe.ToggleWipe(fillScreen: false);
        UpdateTextClock(date);
    }

    private void Update(){
        if(isPaused){ return; }
        if(timer.UpdateAndCheck(Time.deltaTime)){
            DateAddMinutes(5);
        }
    }

    private void ChangeDay() {
        StartCoroutine(CrChangeDay());
    }

    private IEnumerator CrChangeDay(){
        imageWipe.ToggleWipe(fillScreen: true);
        yield return new WaitUntil(() => imageWipe.IsDone());
        DateToNextDay();
        imageWipe.ToggleWipe(fillScreen: false);
    }

    private void DateToNextDay() {
        date = new DateTime(date.Year, date.Month, date.Day + 1, 6, 00, 0);
        UpdateTextClock(date);
    }

    private void DateAddMinutes(int minutes) {
        date.AddMinutes(minutes);
        UpdateTextClock(date);
    }

    private void UpdateTextClock(DateTime date) {
        textClock.text = date.ToString("dddd d HH:mm");
    }
}
