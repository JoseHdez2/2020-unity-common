using ExtensionMethods;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;
using UnityEngine.Audio;

public class FpsProcGameAudioMgr : MonoBehaviour {
    enum AmbianceType { Office, Outdoors }
    [SerializeField] AudioSource ambianceSound, clockSound, gun, notebook, creditsMusic;
    [SerializeField] AudioClip soundCock, soundShot, soundAsk, soundWrite;
    AudioMixer audioMixer;
    [SerializeField] SerializableDictionaryBase<AmbianceType, AudioClip> ambiances;

    public void StartClockSound() => clockSound.Play();
    public void StopClockSound() => clockSound.Stop();
    public void PlayGunCocking() => gun.Play(soundCock);
    public void PlayGunShot() => gun.Play(soundShot);
    public void PlayConvoAsk() => gun.Play(soundAsk);
    public void PlayConvoWrite() => notebook.Play(soundWrite);
    public void PlayCreditsMusic() => creditsMusic.Play();

    public void StopAllSounds() {
        ambianceSound.Stop();
        clockSound.Stop();
    }
    public void SwitchAmbiance(FpsProcBldg bldg){
        ambianceSound.Pause(); // ambianceSound.FadeOut(0.5f);
        // audioMixer.TransitionToSnapshots();
        AudioClip sound = ambiances[AmbianceType.Office];
        // if(bldg is FpsProcBldgOffice){
        //     sound = ambiances[AmbianceType.Office];
        // } else {
        //     sound = ambiances[AmbianceType.Outdoors];
        // }
        ambianceSound.clip = sound;
        ambianceSound.Play();
    }
}