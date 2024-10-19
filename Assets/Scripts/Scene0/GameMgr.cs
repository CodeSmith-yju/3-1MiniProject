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
public class GameMgr : MonoBehaviour
{
    public static GameMgr single { get; private set; }
    public static List<PlayerData> playerData { get; private set; }//여기 수정함 06-02
    public bool tutorial = false;
    private bool loadChecker = false;
    public int selectedResolution;
    public string input_Name = "";
    public SaveState saveState = SaveState.None;
    [Header("PopUp")]
    public PopUp popUp;

    private void Awake()
    {
        single = this;
        if (playerData == null)
        {
            playerData = new();
            selectedResolution = -1;
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

