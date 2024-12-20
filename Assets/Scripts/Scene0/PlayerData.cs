﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;
public class PlayerData //플레이어 데이터만을 저장하는 데이터 클래스
{
    public readonly string NAME;
    //public readonly string JOB;
    //public readonly Sprite PORTRAIT;
    public float max_Player_Hp;
    public float cur_Player_Hp;
    public float max_Player_Sn;
    public float cur_Player_Sn;
    public float max_Player_Mp;
    public float cur_Player_Mp;

    public int player_Gold;
    public int player_level;
    public float player_max_Exp;
    public float player_cur_Exp;
    
    public float atk_Speed;
    public float atk_Range;
    public float base_atk_Dmg;
    public bool skill_Able;
    public bool isMelee;

    public int defensePoint;
    public float limitAttackSpeed;
    public Ally.Job job;
    public BaseEntity.Attribute playerAttribute;

    public List<Item> listInventory;
    public List<Item> listEquipment;

    public List<PartyData> listPartyDatas;
    public List<PartyData> listPartyDeparture;

    public int playerIndex = 0;

    public int playerQuestID;
    public int playerQuestIndex;

    public PlaceState PlaceState;
    PlayerDifficulty playerDifficulty;
    //public PartyData partySlotData = null;// Hero.cs ... 에서 동일개체인지 확인하려고 추가한 변수..의미가없는거같기도하고
    public PlayerData(string name)
    {
        playerIndex = 0;
        this.NAME = name;
        max_Player_Hp = 40f;
        cur_Player_Hp = max_Player_Hp;
        max_Player_Mp = 5f;
        cur_Player_Mp = 0f;
        max_Player_Sn = 50f;
        cur_Player_Sn = max_Player_Sn;
        player_max_Exp = 10f;
        player_cur_Exp = 0f;
        player_Gold = 1500;
        atk_Speed = 1f;
        atk_Range = 1.2f;
        base_atk_Dmg = 3f;
        player_level = 1;
        defensePoint = 10;

        skill_Able = true;
        isMelee = true;

        playerQuestID = 0;
        playerQuestIndex = 0;

        listInventory = new List<Item>();
        listEquipment = new List<Item>();
        
        listPartyDatas = new List<PartyData>();
        listPartyDeparture = new List<PartyData>();

        playerAttribute = BaseEntity.Attribute.Normal;
        playerDifficulty = PlayerDifficulty.Tutorial_Before;
    }
    public PlayerData(int index, float hp, float mp, float atk_spd, float atk_range, float atkDmg, int lv, string name, bool skil_able, bool melee, int defense, Ally.Job job, BaseEntity.Attribute attribute)
    {
        playerIndex = index;

        max_Player_Hp = hp;
        cur_Player_Hp = hp;
        max_Player_Mp = mp;
        cur_Player_Mp = 0f;

        atk_Speed = atk_spd;
        atk_Range = atk_range;
        base_atk_Dmg = atkDmg;

        player_level = lv;
        this.NAME = name;
        defensePoint = defense;
        skill_Able = skil_able;
        isMelee = melee;
        this.job = job;
        playerAttribute = attribute;
    }

    public void SetPlayerDataDifficulty(int _index)
    {
        playerDifficulty = (PlayerDifficulty)_index;
    }
    public PlayerDifficulty GetPlayerDataDifficulty()
    {
        Debug.Log("Get PlayerData[0].playerDifficulty: "+playerDifficulty.ToString());
        return playerDifficulty;
    }

    public string GetPlayerName()
    {
        return this.NAME;
    }

    public void GetPlayerExp(float _exp)
    {
        Debug.Log("얻은 경험치: " + _exp);
        if ((this.player_max_Exp - this.player_cur_Exp) <= _exp )//내가 레벨업까지 필요로하는 경험치의 양 보다. 지금 집어먹은 경험치의 양이 클때.
        {
            _exp -= (this.player_max_Exp - this.player_cur_Exp); //2

            player_level++;
            this.player_max_Exp *= 2;
            this.player_cur_Exp = 0;
            Debug.Log("계산 후 경험치: "+_exp);

            GetPlayerExp(_exp);
            //레벨업 발생
            AddLvelUpStats();
        }
        else
        {
            this.player_cur_Exp += _exp;
        }
    }

    public void AddLvelUpStats()//현재 레벨에 따라 스탯 새롭게 설정하는 메서드 추가
    {
        //Mathf.Clamp는 주어진 value가 min과 max 사이에 있는지를 확인하고,
        //만약 value가 min보다 작으면 min을 반환하고, max보다 크면 max를 반환합니다. 그렇지 않으면 value를 그대로 반환합니다.

        /*max_Player_Hp = 40f + (_nowLevel * 3f);     // 체력: 플레이어가 전투에서 생존할 수 있도록 기본 체력을 높게 설정하고 레벨에 따라 체력이 증가
        base_atk_Dmg = 3f + (_nowLevel * 0.6f);     // 공격력: 플레이어의 공격력을 중간 수준으로 설정하고 레벨에 따른 상승폭을 조정
        //atk_Speed = 1.0f + (_nowLevel * 0.05f);     // 공격 속도: 중간 속도로 설정하며 레벨업 시 약간 증가
        atk_Speed = Mathf.Clamp((1.0f + (_nowLevel * 0.05f)), 1.0f, 2f);//*/

        max_Player_Hp += 3f;
        base_atk_Dmg += 0.6f;
        atk_Speed += 0.05f;
        defensePoint += 3;
    }
    public void GetPlayerstamina(float _sn)
    {
        if (_sn == -1234)
        {
            cur_Player_Sn = max_Player_Sn;
            return;
        }

        Debug.Log("회복량: " + _sn);
        if ((max_Player_Sn - cur_Player_Sn) <= _sn)//overflow
        {
            cur_Player_Sn = max_Player_Sn;
        }
        else
        {
            cur_Player_Sn += _sn;
        }
    }
}
[System.Serializable]
public class SaveData
{
    public string playerName;

    public int p_level;
    public int p_gold;

    public int questId;
    public int questActionIndex;

    public float p_max_hp;
    public float p_cur_hp;
    public float p_max_sn;
    public float p_cur_sn;
    public float p_max_mp;
    public float p_cur_mp;
    public float p_atk_speed;
    public float p_atk_range;
    public float p_base_atk_Dmg;
    
    public float p_max_Exp;
    public float p_cur_Exp;

    public bool tutorialClear;
    public int p_defensePoint;

    public List<Item> listInven;
    public List<Item> listEquip;
    public List<Item> shops;

    public List<PartyData> listPartyData;
    public List<PartyData> listPartyDeparture;
    public PlayerDifficulty p_playerDifficulty;

    public SaveData(string name, int level, int gold, int qID, int qActID, 
        float max_hp, float cur_hp, float max_sn, float cur_sn, float max_mp, float cur_mp, 
        float a_spd, float a_range, float a_dmg, 
        float max_exp, float cur_exp, bool _tutorialClear, int _defensePoint,
        List<Item> _invenItem, List<Item> _invenEquip, List<Item> _shopSlots,
        List<PartyData> _party, List<PartyData> _departure, PlayerDifficulty _playerDifficulty
        )
    {
        //this.pd = pd;
        this.playerName = name;
        this.p_level = level;
        this.p_gold = gold;

        this.questId = qID;
        this.questActionIndex = qActID;

        this.p_max_hp = max_hp;
        this.p_cur_hp = cur_hp;

        this.p_max_sn = max_sn;
        this.p_cur_sn = cur_sn;
        
        this.p_max_mp = max_mp;
        this.p_cur_mp = cur_mp;

        this.tutorialClear = _tutorialClear;
        this.p_defensePoint = _defensePoint;

        this.p_atk_speed = a_spd;
        this.p_atk_range = a_range;
        this.p_base_atk_Dmg = a_dmg;

        this.p_max_Exp = max_exp;
        this.p_cur_Exp = cur_exp;

        this.listInven = _invenItem;
        this.listEquip = _invenEquip;
        this.shops = _shopSlots;

        this.listPartyData = _party;
        this.listPartyDeparture = _departure;
        this.p_playerDifficulty = _playerDifficulty;
    }

}

public static class SaveSystem
{
    private static string SavePath => Path.Combine(Application.persistentDataPath, "saves/");// 이렇게 하면 SavePath가 Unity에서 지정한 persistentDataPath에 saves 폴더를 생성합니다.
    public static void Save(SaveData saveData, string saveFileName)
    {
        if (!Directory.Exists(SavePath))// 디렉토리가 없다면 새 디렉토리를 생성하는 조건문
        {
            Directory.CreateDirectory(SavePath);
        }

        string saveJson = JsonUtility.ToJson(saveData);

        string saveFilePath = SavePath + saveFileName + ".json";
        File.WriteAllText(saveFilePath, saveJson);
        Debug.Log("Save Success: " + saveFilePath);
    }
    public static SaveData Load(string saveFileName)
    {
        string saveFilePath = SavePath + saveFileName + ".json";

        if (!File.Exists(saveFilePath))
        {
            Debug.LogWarning("No such saveFile exists. Creating a new one...");
            SaveData noneSave = new SaveData("", 0,0,0,0, 0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f, false, 0, null, null, null, null, null, PlayerDifficulty.None);
            Save(noneSave, saveFileName);  // Create a new save file
            return noneSave;
        }

        string saveFile = File.ReadAllText(saveFilePath);
        SaveData saveData = JsonUtility.FromJson<SaveData>(saveFile);
        return saveData;
    }
    public static bool DataCheck(string saveFileName)
    {
        string saveFilePath = SavePath + saveFileName + ".json";

        if (!File.Exists(saveFilePath))
        {
            Debug.LogWarning("No such saveFile exists.");
            return false;
        }

        return true;
    }

}