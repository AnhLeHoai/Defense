using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class DataLevel
{
    [SerializeField] private int level = 1;
    [SerializeField] private LevelType levelType = LevelType.Tutorial;
    [SerializeField] private int levelCountToBonus = -1;
    [SerializeField] private int bonusStepInterstitialIndex = -1;
    [SerializeField] private int bonusLevelIndex = 0;
    [SerializeField] private int displayLevel = 1;
    [SerializeField] private bool isKeyCollected = false;

    public int Level { get => level; set => level = value; }
    public int LevelCountToBonus { get => levelCountToBonus; set => levelCountToBonus = value; }
    public int BonusStepInterstitialIndex { get => bonusStepInterstitialIndex; set => bonusStepInterstitialIndex = value; }
    public int BonusLevelIndex { get => bonusLevelIndex; set => bonusLevelIndex = value; }
    public LevelType LevelType { get => levelType; set => levelType = value; }
    public bool IsKeyCollected { get => isKeyCollected; set => isKeyCollected = value; }
    public int DisplayLevel { get => displayLevel; set => displayLevel = value; }

    public override string ToString()
    {
        return $"{nameof(level)}: {level}, " +
               $"{nameof(levelType)}: {levelType}, " +
               $"{nameof(levelCountToBonus)}: {levelCountToBonus}, " +
               $"{nameof(bonusStepInterstitialIndex)}: {bonusStepInterstitialIndex}, " +
               $"{nameof(bonusLevelIndex)}: {bonusLevelIndex}, " +
               $"{nameof(displayLevel)}: {displayLevel}, " +
               $"{nameof(isKeyCollected)}: {isKeyCollected}";
    }
}