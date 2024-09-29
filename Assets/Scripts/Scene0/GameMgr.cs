using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMgr : MonoBehaviour
{
    public static GameMgr single { get; private set; }
    public static List<PlayerData> playerData { get; private set; }//여기 수정함 06-02

    private bool loadChecker = false;

    [Header("PopUp")]
    public PopUp popUp;

    private void Awake()
    {
        single = this;
        if (playerData == null)
        {
            playerData = new();
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

}

