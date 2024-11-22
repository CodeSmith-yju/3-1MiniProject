using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum SaveState
{
    None,
    DungeonSave,
    TownSave,
}
public enum PlayerDifficulty
{
    None = 0,          // 선택되지 않음
    Tutorial_Before = 1, // 튜토리얼 진행 전
    
    Tutorial_WearEquipBefore = 2, //장비장착 전,후
    Tutorial_WearEquipAfter = 3,

    Tutorial_PartyBefor = 4, // 파티원 모집 전,후-> 후 이상으로 갔을때 P키로 상태창열리게함.
    Tutorial_PartyAfter = 5,

    Tutorial_DungeonBefor = 6,
    Tutorial_DungeonClearAndNotTalk = 7, // 튜토리얼 클리어 후
    Tutorial_After = 8, //튜토클 이후 접수원 상담 

    Easy_Before = 20,     // 쉬움 난이도 진행 전
    Easy_After,      // 쉬움 난이도 클리어 후

    Normal_Before = 30,   // 보통 난이도 진행 전
    Normal_After,    // 보통 난이도 클리어 후

    Hard_Before = 40,     // 어려움 난이도 진행 전
    Hard_After,      // 어려움 난이도 클리어 후

    FinalBoss = 50        // 최종 던전
}
public class GameMgr : MonoBehaviour
{
    public static GameMgr single { get; private set; }
    public static List<PlayerData> playerData { get; private set; }//여기 수정함 06-02

    public bool shopCleaner;
    public bool tutorial = false;
    private bool loadChecker = false;
    public int selectedResolution;
    public string input_Name = "";
    public SaveState saveState = SaveState.None;
    public bool firstStart = true;
    [Header("PopUp")]
    public PopUp popUp;

    private void Awake()
    {
        single = this;
        if (playerData == null)
        {
            playerData = new();
            selectedResolution = -1;
            shopCleaner = false;
        }
        else
        {
            playerData.Clear();
        }

    }

    public bool OnSelectPlayer(string name)
    {
        playerData.Clear();
        playerData.Add(new PlayerData(name)); // 여기 수정함 06-04

        bool succ = playerData != null;
        if (!succ)
            return false;
        if (name == null)
            return false;

        Debug.Log("캐릭터 생성 성공");

        if (loadChecker == false)
        {
            //SceneManager.LoadScene("Town");
            LoadingSceneController.LoadScene("Town");
        }

        return true;
        
    }

    public bool IsGameLoad(bool cheker)
    {
        this.loadChecker = cheker;
        return loadChecker;
    }
    public bool LoadChecker()
    {
        return this.loadChecker;
    }

    public bool SetShopClean(bool _bool)
    {
        shopCleaner = _bool;
        return shopCleaner;
    }
    public bool GetShopClean()
    {
        return shopCleaner;
    }
    public void SetPlayerDifficulty(int _index)
    {
        playerData[0].playerDifficulty = (PlayerDifficulty)_index;
        Debug.Log("SetDifficulty: "+ (PlayerDifficulty)_index);
    }
    public int GetPlayerDifficulty()
    {
        Debug.Log("GetDifficulty: " + playerData[0].playerDifficulty);
        return (int)playerData[0].playerDifficulty;
    }

    public bool GetPartyCommit()
    {
        if ((PlayerDifficulty)GetPlayerDifficulty() == PlayerDifficulty.Tutorial_PartyBefor)
        {
            SetPlayerDifficulty(5);
            return true;
        }

        return false;
    }
}

