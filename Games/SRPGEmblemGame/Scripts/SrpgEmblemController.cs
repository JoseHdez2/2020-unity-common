using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    protected override void StartTurnChild(){
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
        string winningTeamId = teamIds.FirstOrDefault(t => IsTeamAlive(t));
        if(winningTeamId == "good guys"){
            teamText.text = $"Victory!";
        } else {
            teamText.text = $"Defeat...";
        }
        ninjaVision.GetReady();
        yield return new WaitForSeconds(0.3f);
        ToggleFieldCursor(false);
        if(winningTeamId == "good guys"){
            audioSource.PlaySound(ESRPGSound.FanfareWin);
        } else {
            audioSource.PlaySound(ESRPGSound.FanfareLose);
        }
        ninjaVision.Activate();
        yield return new WaitForSeconds(1);
        musicSource.Pause();
    }

}
