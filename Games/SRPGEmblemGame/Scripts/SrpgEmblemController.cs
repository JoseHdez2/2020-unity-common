using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SrpgEmblemController : SrpgController
{
    private SrpgEmblemMusicSource musicSource;

    private void Start() {
        base.Start();
        musicSource = FindObjectOfType<SrpgEmblemMusicSource>();
        musicSource.PlaySound(GetTurnMusic());
    }

    public override void ChangeTurn(){
        Debug.Log("child ChangeTurn!");
        base.ChangeTurn();
        StartCoroutine(CrChangeTurn());
    }

    private IEnumerator CrChangeTurn(){
        Debug.Log("Change Turn!");
        fieldCursor.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.3f);
        audioSource.PlaySound(ESRPGSound.TurnChange);
        yield return new WaitForSeconds(1);
        musicSource.Pause();
        // TODO end animation
        yield return new WaitForSeconds(1);
        musicSource.PlaySound(GetTurnMusic());
        fieldCursor.gameObject.SetActive(true);
    }

    private ESrpgEmblemMusic GetTurnMusic(){
        if(curTeam == "good guys"){
            return ESrpgEmblemMusic.PlayerTurn;
        } else { // if (curTeam == "bad guys"){
            return ESrpgEmblemMusic.EnemyTurn;
        }
    }

}
