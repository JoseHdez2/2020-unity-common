using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(VfxNinjaVision))]
public class SrpgEmblemController : SrpgController
{
    private SrpgEmblemMusicSource musicSource;
    private VfxNinjaVision ninjaVision;

    private void Awake() {
        musicSource = FindObjectOfType<SrpgEmblemMusicSource>();
        musicSource.PlaySound(GetTurnMusic());
        ninjaVision = GetComponent<VfxNinjaVision>();
    }

    public override void ChangeTurn(bool firstTurn = false, string forceTeamId = null){
        base.ChangeTurn(firstTurn, forceTeamId);
        ToggleFieldCursor(false);
        StartCoroutine(CrChangeTurn());
    }

    private IEnumerator CrChangeTurn(){
        Debug.Log("Change Turn!");
        ninjaVision.GetReady();
        yield return new WaitForSeconds(0.3f);
        ToggleFieldCursor(false);
        audioSource.PlaySound(ESRPGSound.TurnChange);
        ninjaVision.Activate();
        yield return new WaitForSeconds(1);
        musicSource.Pause();
        // TODO end animation
        yield return new WaitUntil(() => !audioSource.IsPlaying());
        ninjaVision.Deactivate();
        musicSource.PlaySound(GetTurnMusic());
        ToggleFieldCursor(true);
    }

    private ESrpgEmblemMusic GetTurnMusic(){
        if(curTeam == "good guys"){
            return ESrpgEmblemMusic.PlayerTurn;
        } else { // if (curTeam == "bad guys"){
            return ESrpgEmblemMusic.EnemyTurn;
        }
    }

    protected override void EndGame(){
        Debug.Log("Game has ended!");
        StartCoroutine(CrEndGame());
    }

    private IEnumerator CrEndGame(){
        Debug.Log("Ending game...");
        teamText.text = $"Victory!";
        ninjaVision.GetReady();
        yield return new WaitForSeconds(0.3f);
        ToggleFieldCursor(false);
        audioSource.PlaySound(ESRPGSound.FanfareWin);
        ninjaVision.Activate();
        yield return new WaitForSeconds(1);
        musicSource.Pause();
    }

}
