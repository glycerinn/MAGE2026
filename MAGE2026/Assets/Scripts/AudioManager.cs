using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioSource BGM;
    public AudioSource SFX;

    public AudioClip MainMenuBGM;
    public AudioClip GameBGM;
    public AudioClip GameOverSFX;
    public AudioClip GameWinSFX;
    public AudioClip clicksfx;
    public AudioClip hoversfx;
    public AudioClip grabsfx;
    public AudioClip glitchsfx;
    public AudioClip humfailsfx;
    public AudioClip humsuccesssfx;
    public AudioClip minigamesuccesssfx;
    public AudioClip satellitesfx;
    public AudioClip yessfx;
    public AudioClip fixsfx;

    public static AudioManager instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayBGM(AudioClip clip)
    {
        if (BGM.clip == clip && BGM.isPlaying)
            return;

        BGM.clip = clip;
        BGM.Play();
    }

    public void StopBGM()
    {
        BGM.Stop();
    }

    public void StopSFX()
    {
        SFX.Stop();
    }

    public void playMainMenuBGM()
    {
        BGM.clip = MainMenuBGM;
        BGM.Play();
    }

    public void playGameBGM()
    {
        PlayBGM(GameBGM);
    }

    public void playGameOverSFX()
    {
        SFX.PlayOneShot(GameOverSFX);
    }

    public void playGameWinSFX()
    {
        SFX.PlayOneShot(GameWinSFX);
    }

    public void playClickSFX()
    {
        SFX.PlayOneShot(clicksfx);
    }

    public void playHoverSFX()
    {
        SFX.PlayOneShot(hoversfx);
    }

    public void playGrabSFX()
    {
        SFX.PlayOneShot(grabsfx);
    }

    public void playGlitchSFX()
    {
        SFX.PlayOneShot(glitchsfx);
    }

    public void playHumFailSFX()
    {
        SFX.PlayOneShot(humfailsfx);
    }

    public void playHumSuccessSFX()
    {
        SFX.PlayOneShot(humsuccesssfx);
    }

    public void playFixSFX()
    {
        SFX.PlayOneShot(fixsfx);
    }

    public void playFix2SFX()
    {
        SFX.PlayOneShot(fixsfx);
        StartCoroutine(StopFixSFX());
    }

    private IEnumerator StopFixSFX()
    {
        yield return new WaitForSeconds(2f);
        SFX.Stop();
    }

    public void playMinigamesuccessSFX()
    {
        SFX.PlayOneShot(minigamesuccesssfx);
    }

    public void playsatelliteSFX()
    {
        SFX.PlayOneShot(satellitesfx);
    }

    public void playYesSFX()
    {
        SFX.PlayOneShot(yessfx);
    }
}