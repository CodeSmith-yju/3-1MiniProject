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
    Tutorial_After = 2,  // 튜토리얼 클리어 후

    Easy_Before = 10,     // 쉬움 난이도 진행 전
    Easy_After,      // 쉬움 난이도 클리어 후

    Normal_Before = 20,   // 보통 난이도 진행 전
    Normal_After,    // 보통 난이도 클리어 후

    Hard_Before = 30,     // 어려움 난이도 진행 전
    Hard_After,      // 어려움 난이도 클리어 후

    FinalBoss = 40        // 최종 던전
}
public class GameMgr : MonoBehaviour
{
    public static GameMgr single { get; private set; }
    public static List<PlayerData> playerData { get; private set; }//여기 수정함 06-02
    public PlayerDifficulty playerDifficulty { get; private set; }

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

}

