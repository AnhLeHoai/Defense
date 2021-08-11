using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class SoundData: MonoBehaviour
{
    public AudioClip AudioClickBtn;

    public AudioClip AudioFootStep;
    public AudioClip AudioRevive;
    public AudioClip AudioReward;
    public AudioClip AudioSpinWheel;
    public AudioClip AudioStartCrewmate;
    public AudioClip AudioStartImpostor;
    public AudioClip AudioWin;
    public AudioClip AudioLose;

    public AudioClip AudioLobby;
    public AudioClip[] AudioBgs;
    public AudioClip AudioOverTime;


    [Title("Congratulations")] 
    public AudioClip[] AudioSingleKills;
    public AudioClip[] AudioMultiKills;
    
    [Title("Weapons")]
    [ListDrawerSettings(ShowIndexLabels = true)]
    public AudioClip[] AudioAttacks;

    public AudioClip AudioMineExplode;
    public AudioClip AudioFireExtinguished;
    public AudioClip ObjectExploded;
     
    [Title("Collectibles")]
    public List<AudioClip> ListAudioCollects;

    [Title("Special Objects")] 
    public AudioClip[] BreakableObjectSounds;



}
