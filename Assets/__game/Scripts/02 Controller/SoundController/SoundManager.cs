using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

[Singleton("SoundManager", true)]
public class SoundManager : Singleton<SoundManager>
{
    public enum GameSound
    {
        BGM,
        BGM_BONUS,
        BGM_BONUS_2,
        Footstep,
        Spin,
        Lobby
    }

    public enum Other
    {
        MineExplode,
        FireExtinguished,
        ObjectExploded
    }

    public enum BreakableObjectSound
    {
        Metal,
        Wood,
        Rock
    }

    [SerializeField] private SoundData soundData;

    public AudioSource bgMusic;
    public AudioSource fxSound;
    public AudioSource fxSoundFootStep;

    private float bgVol;
    private bool isPlayFootStep;

    #region UNITY METHOD
    private void Start()
    {
        SettingFxSound(GameManager.Instance.PlayerDataManager.GetSoundSetting());
        SettingMusic(GameManager.Instance.PlayerDataManager.GetMusicSetting());
        isPlayFootStep = false;
    }
    #endregion
    #region PUBLIC METHOD

    public void PlayFxSound(Enum soundEnum)
    {
        switch (soundEnum)
        {
            case LevelResult levelResult:
                {
                    switch (levelResult)
                    {
                        case LevelResult.Win:
                            PlaySoundWinCrewmate();
                            break;
                        case LevelResult.Lose:
                            PlaySoundWinImposter();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    break;
                }
            case GameSound gameSound:
                {
                    switch (gameSound)
                    {
                        case GameSound.BGM:
                        case GameSound.BGM_BONUS:
                        case GameSound.BGM_BONUS_2:
                            PlayBGM(Random.Range(0, soundData.AudioBgs.Length));
                            break;
                        case GameSound.Footstep:
                            PlayFootStep();
                            break;
                        case GameSound.Spin:
                            PlaySoundSpin();
                            break;
                        case GameSound.Lobby:
                            PlayBGM(soundData.AudioLobby);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                }
            case Other other:
                {
                    switch (other)
                    {
                        case Other.MineExplode:
                            PlayFxSound(soundData.AudioMineExplode);
                            break;
                        case Other.ObjectExploded:
                            PlayFxSound(soundData.ObjectExploded);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                }
            case TypeSoundIngame collectibleSound:
                {
                    PlaySoundCollectible(collectibleSound);
                    break;
                }
            default:
                throw new InvalidEnumArgumentException($"{soundEnum} is not supported");
        }
    }

    public void PlayFxSound(Enum soundEnum, AudioSource audioSource)
    {
        switch (soundEnum)
        {
            case Role role:
                
                if ((int)role < soundData.AudioAttacks.Length)
                    PlayFxSound(soundData.AudioAttacks[(int)role], audioSource);
                return;
            case BreakableObjectSound breakableObjectSound:
                PlayFxSound(soundData.BreakableObjectSounds[(int)breakableObjectSound], audioSource);
                break;
            case Other other:
                {
                    switch (other)
                    {
                        case Other.MineExplode:
                            PlayFxSound(soundData.AudioMineExplode, audioSource);
                            break;
                        case Other.FireExtinguished:
                            PlayFxSound(soundData.AudioFireExtinguished, audioSource);
                            break;
                        case Other.ObjectExploded:
                            PlayFxSound(soundData.ObjectExploded, audioSource);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                }
            default:
                throw new InvalidEnumArgumentException($"{soundEnum} is not supported");
        }
    }

    public void StopSound(Enum soundEnum)
    {
        switch (soundEnum)
        {
            case GameSound gameSound:
                {
                    switch (gameSound)
                    {
                        case GameSound.Lobby:
                        case GameSound.BGM:
                            bgMusic.DOFade(0, 1f).OnComplete(action: () => bgMusic.Stop());
                            break;
                        case GameSound.Footstep:
                            StopFootStep();
                            break;
                        case GameSound.Spin:
                            StopFxSound();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    break;
                }
            case LevelResult levelResult:
            case TypeSoundIngame collectibleSound:
                {
                    StopFxSound();
                    break;
                }
            case Other other:
                {
                    switch (other)
                    {
                        case Other.MineExplode:
                            StopFxSound();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                }
            default:
                throw new InvalidEnumArgumentException($"{soundEnum} is not supported");
        }
    }

    public void SettingFxSound(bool isOn)
    {
        var vol = isOn ? 1 : 0;
        fxSound.volume = vol;
        fxSoundFootStep.volume = vol;
        fxSound.mute = !isOn;
        fxSoundFootStep.mute = !isOn;
    }

    public void SettingMusic(bool isOn)
    {
        bgVol = isOn ? 1 : 0;
        bgMusic.volume = bgVol;
        bgMusic.mute = !isOn;
        //ValueBGMusic = vol;
    }

    #endregion
    #region PRIVATE METHOD
    private void PlayFxSound(AudioClip clip, AudioSource audioSource)
    {
        audioSource.volume = fxSound.volume;
        audioSource.clip = clip;
        audioSource.Play();
    }

    public bool IsOnVibration
    {
        get
        {
            return PlayerPrefs.GetInt("OnVibration", 1) == 1 ? true : false;
        }
    }

    private void PlayBGM(int index)
    {
        var backgroundMusics = soundData.AudioBgs;
        PlayBGM(backgroundMusics[index]);

    } 
    
    private void PlayBGM(AudioClip audioClip)
    {
        bgMusic.loop = true;
        bgMusic.clip = audioClip;
        bgMusic.volume = 0;
        bgMusic.DOKill();
        bgMusic.DOFade(bgVol, 1f);
        bgMusic.Play();
    }

    private void PlayFxSound(AudioClip clip)
    {
        fxSound.PlayOneShot(clip);
    }

    private void StopFxSound()
    {
        fxSound.Stop();
    }

    public void PlaySoundButton()
    {
        PlayFxSound(soundData.AudioClickBtn);
    }

    public void PlaySoundSpin()
    {
        PlayFxSound(soundData.AudioSpinWheel);
    }

    public void PlaySoundRevive()
    {
        PlayFxSound(soundData.AudioRevive);
    }

    public void PlaySoundReward()
    {
        PlayFxSound(soundData.AudioReward);
    }

    public void PlaySoundStartCrewmate()
    {
        PlayFxSound(soundData.AudioStartCrewmate);
    }

    public void PlaySoundStartImpostor()
    {
        PlayFxSound(soundData.AudioStartImpostor);
    }

    public void PlaySoundWinCrewmate()
    {
        PlayBGM(soundData.AudioWin);
    }

    public void PlaySoundWinImposter()
    {
        PlayBGM(soundData.AudioLose);
    }

    public void PlaySoundCollectible(TypeSoundIngame typeSound)
    {
        PlayFxSound(soundData.ListAudioCollects[(int)typeSound - 1]);
    }

    public void PlayFootStep()
    {
        if (isPlayFootStep)
            return;

        isPlayFootStep = true;
        fxSoundFootStep.Play();

        Analytics.LogFirstLogJoystick();
    }

    public void StopFootStep()
    {
        fxSoundFootStep.Stop();
        isPlayFootStep = false;
    }

    public void PlaySoundOverTime()
    {
        fxSound.PlayOneShot(soundData.AudioOverTime);
    }
    #endregion
}
