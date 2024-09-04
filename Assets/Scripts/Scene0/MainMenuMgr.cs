using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuMgr : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject OptionMenu;
    [SerializeField] AddUserName addUserName;

    //[SerializeField] Button btnLoadGame;

    [SerializeField] List<Sprite> imgListBG;
    public Image imgMenuBG;

    [Header("Title Buttons")]
    public List<Button> TitleBtns;

    [Header("Title Button Sprite")]
    public List<Sprite> TitleBtnSprites;
    private void Awake()
    {
        Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);// 게임 시작 시 1920*1080 창모드 실행

        mainMenu.SetActive(true);
        OptionMenu.SetActive(false);
        addUserName.gameObject.SetActive(false);

        if (SaveSystem.DataCheck("save") == false)
        {
            TitleBtns[0].gameObject.SetActive(false);//btnLoadGame.gameObject.SetActive(false);
        }
        else 
        {
            TitleBtns[0].gameObject.SetActive(true);
        }

        string SavePath = Path.Combine(Application.persistentDataPath, "saves/");
        Debug.Log(SavePath);
    }
    
    private void Start()
    {
        ChangeBG();
        //RefreshTitleBtns();
    }

    public void OnClickedGameStart()
    {
        //mainMenu.SetActive(false);
        addUserName.gameObject.SetActive(true);
    }
    public void OnClickedOptions()
    {
        //mainMenu.SetActive(false);
        OptionMenu.SetActive(true);
    }
    public void OnClickedReLoadGame()
    {
        GameMgr.single.IsGameLoad(true);
        //SceneManager.LoadScene("Town");
        LoadingSceneController.LoadScene("Town");
    }


    public void OnClickedSelectQuite()
    {
        mainMenu.SetActive(true);
        addUserName.gameObject.SetActive(false);

    }
    public void OnClickedOptionsQuite()
    {
        //mainMenu.SetActive(true);
        OptionMenu.SetActive(false);
    }

    public void OnClickedQuite()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }


    //05-14 Change BG Method
    private void ChangeBG()
    {
        // 현재 시간을 가져옵니다.
        int currentHour = System.DateTime.Now.Hour;

        // 시간대에 따라 다른 작업을 수행합니다.
        if (currentHour >= 6 && currentHour < 14)
        {
            // 오전 6시부터 오후 2시까지
            DoMorningWork();
            Debug.Log(currentHour);
        }
        else if (currentHour >= 14 && currentHour < 20)
        {
            // 오후 2시부터 오후 8시까지
            DoEveningWork();
            Debug.Log(currentHour);
        }
        else
        {
            // 그 외의 시간대 (오후 8시부터 다음 날 오전 6시까지)
            DoNightWork();
            Debug.Log(currentHour);
        }
    }
    private void DoMorningWork()
    {
        Debug.Log("현재는 오전 시간대입니다. 아침 작업을 수행합니다.");
        // 아침 작업 수행 코드 작성
    }

    private void DoEveningWork()
    {
        Debug.Log("현재는 오후 시간대입니다. 저녁 작업을 수행합니다.");
        // 저녁 작업 수행 코드 작성
        imgMenuBG.sprite = imgListBG[0];

    }

    private void DoNightWork()
    {
        Debug.Log("현재는 밤 시간대입니다. 야간 작업을 수행합니다.");
        // 야간 작업 수행 코드 작성
    }

    /*public void RefreshTitleBtns()
    {
        foreach (var _btn in TitleBtns)
        {
            _btn.image.sprite = TitleBtnSprites[0];
        }
    }*/

}
