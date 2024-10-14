using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public enum PopUpState
{
    None,
    Quite,
    GameStart,
    GameSave,
    GameReLoad,
    Dungeon,
    DungeonExit,
    SnPotion,
}
public class PopUp : MonoBehaviour
{
    [Header("Text")]
    public TextMeshProUGUI text_PopUp;
    
    [Header("Buttons")]
    public Button btn_Yes;
    public Button btn_No;

    [SerializeField] PopUpState popUpState;

    void ListnerSet()//클릭 이벤트 리스너 추가했는데 버그나서 일단뺌
    {
        btn_Yes.onClick.RemoveAllListeners();
        btn_No.onClick.RemoveAllListeners();

        btn_Yes.onClick.AddListener(OnClickYesBtn);
        btn_No.onClick.AddListener(OnClickNoBtn);
    }

    public void OnClickYesBtn()
    {
        switch (popUpState)
        {
            case PopUpState.None:
                if (gameObject.activeSelf == true)
                {
                    Debug.Log("None Btn Clicked");
                    gameObject.SetActive(false);
                }
                else
                    Debug.Log("Bug");
                break;
            case PopUpState.GameStart:
                Canvas canvas = FindObjectOfType<Canvas>();
                canvas.GetComponent<FadeInEffect>().FadeOFFAndLoadScene();
                break;
            case PopUpState.GameSave:
                GameUiMgr.single.GameSave();
                break;
            case PopUpState.GameReLoad:
                GameMgr.single.IsGameLoad(true);
                LoadingSceneController.LoadScene("Town");
                break;
            case PopUpState.Quite:
                GameQuite();
                break;
            case PopUpState.Dungeon:
                GameUiMgr.single.MoveInDungeon();
                break;
            case PopUpState.DungeonExit:
                if (Time.timeScale == 0)
                {
                    Time.timeScale = 1;
                }
                if (BattleManager.Instance.ui.option_UI.activeSelf)
                    BattleManager.Instance.ui.option_UI.SetActive(false);
                BattleManager.Instance.ui.OpenPopup(BattleManager.Instance.ui.exit_Popup); // 던전 중단 결과창
                break;
            case PopUpState.SnPotion:
                GameMgr.playerData[0].GetPlayerstamina(GameUiMgr.single.nowSlot.item.itemPower);
                Inventory.Single.RemoveItem(GameUiMgr.single.nowSlot.item);
                break;
            default:
                break;
        }

        gameObject.SetActive(false);
    }
    public void OnClickNoBtn()
    {
        gameObject.SetActive(false);
    }
    public void SetPopUp(string _str, PopUpState _state)
    {
        text_PopUp.text = _str;  // 팝업 텍스트 설정
        popUpState = _state;     // 팝업 상태 설정

        if (popUpState == PopUpState.None)
            btn_No.gameObject.SetActive(false);
        else
            btn_No.gameObject.SetActive(true);

        if (!gameObject.activeSelf)  // 비활성화된 상태라면 팝업을 활성화
        {
            gameObject.SetActive(true);
            Debug.Log("팝업 활성화: " + _str);
        }
        else
        {
            Debug.LogWarning("팝업이 이미 활성화되어 있습니다.");
        }

        Canvas.ForceUpdateCanvases();  // 강제로 UI 갱신
    }

    public void SetPopUp(string _str, PopUpState _state, int Lv)
    {
        text_PopUp.text = _str;  // 팝업 텍스트 설정
        popUpState = _state;     // 팝업 상태 설정

        if (popUpState == PopUpState.None)
            btn_No.gameObject.SetActive(false);
        else
            btn_No.gameObject.SetActive(true);

        if (!gameObject.activeSelf)  // 비활성화된 상태라면 팝업을 활성화
        {
            gameObject.SetActive(true);
            Debug.Log("팝업 활성화: " + _str);
        }
        else
        {
            Debug.LogWarning("팝업이 이미 활성화되어 있습니다.");
        }

        Canvas.ForceUpdateCanvases();  // 강제로 UI 갱신
    }
    public void GameQuite()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
