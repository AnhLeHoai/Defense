using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public Action<TypeItem> actionUITop;
    public DataTexture DataTexture;
    public DataRewardEndGame DataRewardEndGame;
    public DataLuckyWheel DataLuckyWheel;

    private DataLevel dataLevel;

    public void SetDataLevel(DataLevel dataLevel)
    {
        this.dataLevel = dataLevel;
        PlayerPrefs.SetString(Helper.DataLevel, JsonUtility.ToJson(dataLevel));
        SetMaxLevelReached(dataLevel.Level);
    }

    public DataLevel GetDataLevel()
    {
        if (dataLevel != null)
        {
            return dataLevel;
        }

        var dataLevelJson = PlayerPrefs.GetString(Helper.DataLevel, string.Empty);
        if (dataLevelJson == string.Empty)
        {
            dataLevel = new DataLevel();
        }
        else
        {
            dataLevel = JsonUtility.FromJson<DataLevel>(dataLevelJson);
        }

        return dataLevel ?? new DataLevel();
    }

    private void SetMaxLevelReached(int currentLevel)
    {
        PlayerPrefs.SetInt(Helper.DataMaxLevelReached, Math.Max(GetMaxLevelReached(), currentLevel));
    }

    public int GetMaxLevelReached()
    {
        return PlayerPrefs.GetInt(Helper.DataMaxLevelReached, 1);
    }

    public bool GetUnlockSkin(TypeEquipment type, int id)
    {
        return PlayerPrefs.GetInt(Helper.DataTypeSkin + type + id, 0) == 0 ? false : true;
    }

    public void SetUnlockSkin(TypeEquipment type, int id)
    {
        PlayerPrefs.SetInt(Helper.DataTypeSkin + type + id, 1);
        SetIdEquipSkin(type, id);
    }

    public int GetIdEquipSkin(TypeEquipment type)
    {
        return PlayerPrefs.GetInt(Helper.DataEquipSkin + type, -1);
    }

    public void SetIdEquipSkin(TypeEquipment type, int id)
    {
        PlayerPrefs.SetInt(Helper.DataEquipSkin + type, id);
    }

    public int GetNumberWatchVideoSkin(TypeEquipment type, int id)
    {
        return PlayerPrefs.GetInt(Helper.DataNumberWatchVideo + type + id, 0);
    }

    public void SetNumberWatchVideoSkin(TypeEquipment type, int id, int number)
    {
        PlayerPrefs.SetInt(Helper.DataNumberWatchVideo + type + id, number);
    }

    public int GetGold()
    {
        return PlayerPrefs.GetInt(Helper.GOLD, 0);
    }

    public void SetGold(int _count)
    {
        PlayerPrefs.SetInt(Helper.GOLD, _count);
    }

    public int GetKey()
    {
        return PlayerPrefs.GetInt(Helper.KEY, 0);
    }

    public void SetKey(int _count)
    {
        PlayerPrefs.SetInt(Helper.KEY, _count);
    }

    public int GetCurrentIndexRewardEndGame()
    {
        return PlayerPrefs.GetInt(Helper.CurrentRewardEndGame, 0);
    }

    public void SetCurrentIndexRewardEndGame(int index)
    {
        PlayerPrefs.SetInt(Helper.CurrentRewardEndGame, index);
    }

    public int GetProcessReceiveRewardEndGame()
    {
        return PlayerPrefs.GetInt(Helper.ProcessReceiveEndGame, 0);
    }

    public void SetProcessReceiveRewardEndGame(int number)
    {
        PlayerPrefs.SetInt(Helper.ProcessReceiveEndGame, number);
    }


    public int GetNumberWatchDailyVideo()
    {
        return PlayerPrefs.GetInt("NumberWatchDailyVideo", DataLuckyWheel.NumberSpinDaily);
    }

    public void SetNumberWatchDailyVideo(int number)
    {
        PlayerPrefs.SetInt("NumberWatchDailyVideo", number);
    }

    public bool GetFreeSpin()
    {
        return PlayerPrefs.GetInt("FreeSpin", 1) > 0 ? true : false;
    }

    public void SetFreeSpin(bool isFree)
    {
        int free = isFree ? 1 : 0;
        PlayerPrefs.SetInt("FreeSpin", free);
    }

    public int GetNumberWatchVideoSpin()
    {
        return PlayerPrefs.GetInt("NumberWatchVideoSpin", 0);

    }

    public void SetNumberWatchVideoSpin(int count)
    {
        PlayerPrefs.SetInt("NumberWatchVideoSpin", count);
    }

    public string GetTimeLoginSpinFreeWheel()
    {
        return PlayerPrefs.GetString("TimeSpinFreeWheel", "");
    }

    public void SetTimeLoginSpinFreeWheel(string time)
    {
        PlayerPrefs.SetString("TimeSpinFreeWheel", time);
    }

    public string GetTimeLoginSpinVideo()
    {
        return PlayerPrefs.GetString("TimeLoginSpinVideo", "");
    }

    public void SetTimeLoginSpinVideo(string time)
    {
        PlayerPrefs.SetString("TimeLoginSpinVideo", time);
    }

    public void SetSoundSetting(bool isOn)
    {
        PlayerPrefs.SetInt(Helper.SoundSetting, isOn ? 1 : 0);
    }

    public bool GetSoundSetting()
    {
        return PlayerPrefs.GetInt(Helper.SoundSetting, 1) == 1;
    }

    public void SetMusicSetting(bool isOn)
    {
        PlayerPrefs.SetInt(Helper.MusicSetting, isOn ? 1 : 0);
    }

    public bool GetMusicSetting()
    {
        return PlayerPrefs.GetInt(Helper.MusicSetting, 1) == 1;

    }

    public bool IsNoAds()
    {
        return PlayerPrefs.GetInt("NoAds", 0) == 1;
    }

    public void SetNoAds()
    {
        PlayerPrefs.SetInt("NoAds", 1);
    }

    private List<int> listIdSkin = new List<int>();
    public int GetIdSkinOtherPlayer()
    {
        if (listIdSkin.Count == 0)
        {
            for (int i = 1; i < Enum.GetNames(typeof(Skin)).Length; i++)
            {
                listIdSkin.Add(i);
            }
        }

        var index = UnityEngine.Random.Range(0, listIdSkin.Count);
        int id = listIdSkin[index];
        listIdSkin.RemoveAt(index);

        return id;
    }

    public void ClearListIdSkin()
    {
        if (listIdSkin.Count > 0)
            listIdSkin.Clear();
    }

    public void SetNumberPlay(int num)
    {
        PlayerPrefs.SetInt("NumberPlay", num);
    }

    public int GetNumberPlay()
    {
        return PlayerPrefs.GetInt("NumberPlay", 0);
    }

    private List<string> listNamePlayer = new List<string>();
    public string GetName()
    {
        string name = "";

        if (listNamePlayer.Count <= 0)
        {
            for (int i = 0; i < NameGenerator.Names.Length; i++)
            {
                listNamePlayer.Add(NameGenerator.Names[i]);
            }
        }

        int index = UnityEngine.Random.Range(0, listNamePlayer.Count);
        name = listNamePlayer[index];
        listNamePlayer.RemoveAt(index);

        return name;
    }

    public void SetLastLevelRoles(params Role[] roles)
    {
        StringBuilder stringRoles = new StringBuilder();
        for (int i = 0; i < roles.Length; i++)
        {
            stringRoles.Append((int)roles[i]).Append(",");
        }

        stringRoles.Length--;

        PlayerPrefs.SetString("LastLevelRoles", stringRoles.ToString());
    }

    public Role[] GetLastLevelRoles()
    {
        var value = PlayerPrefs.GetString("LastLevelRoles", "");
        if (value == "")
        {
            return null;
        }

        string[] stringRoles = value.Split(',');

        Role[] roles = new Role[stringRoles.Length];
        for (int i = 0; i < roles.Length; i++)
        {
            roles[i] = (Role)int.Parse(stringRoles[i]);
        }

        return roles;
    }

    #region Get Id Role

    private List<int> listIdRoles = new List<int>();
    public Role GetRoleRandom()
    {
        Role role = Role.Assassin;
        if (listIdRoles.Count == 0)
        {
            for (int i = 0; i < Enum.GetValues(typeof(Role)).Length; i++)
            {
                listIdRoles.Add(i);
            }
        }

        var index = UnityEngine.Random.Range(0, listIdRoles.Count);
        role = (Role)listIdRoles[index];
        listIdRoles.RemoveAt(index);

        return role;
    }

    #endregion

    public string GetTimeLoginOpenGift()
    {
        return PlayerPrefs.GetString("TimeLoginOpenGift", "");
    }

    public void SetTimeLoginOpenGift(string time)
    {
        PlayerPrefs.SetString("TimeLoginOpenGift", time);
    }
}
