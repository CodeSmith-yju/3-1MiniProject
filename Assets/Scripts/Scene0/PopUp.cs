using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public enum PopUpState
{
    None,
    NoStamina,
    Quite,
    GameStart,
    GameSave,
    GameReLoad,
    Dungeon,
    DungeonExit,
    SnPotion,
    PartyCommit,
}
public class PopUp : MonoBehaviour
{
    [Header("Text")]
    public TextMeshProUGUI text_PopUp;
    public TextMeshProUGUI warningPartyCommit;

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
            case PopUpState.NoStamina:
                if (gameObject.activeSelf == true)
                {
                    Debug.Log("소지금: "+GameMgr.playerData[0].player_Gold + "잔금: "+ (int)(GameMgr.playerData[0].player_Gold * 0.9f));
                    GameMgr.playerData[0].player_Gold -= (int)(GameMgr.playerData[0].player_Gold *0.1f);
                    if (GameMgr.playerData[0].player_Gold < 0)
                    {
                        GameMgr.playerData[0].player_Gold = 0;
                    }
                    GameMgr.playerData[0].GetPlayerstamina(-1234);
                    Debug.Log("스태미나 회복: " + GameMgr.playerData[0].cur_Player_Sn );

                    GameMgr.single.IsGameLoad(true);
                    GameUiMgr.single.GameSave();
                    GameMgr.single.SetShopClean(true);
                    LoadingSceneController.LoadScene("Town");

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

                if (BattleManager.Instance.dialogue != null && BattleManager.Instance.dialogue.isTutorial)
                {
                    BattleManager.Instance.dialogue.ONOFF(false);
                    BattleManager.Instance.ui.dialogue_Bg.SetActive(false);
                }
                   

                BattleManager.Instance.ui.OpenPopup(BattleManager.Instance.ui.exit_Popup); // 던전 중단 결과창
                break;
            case PopUpState.SnPotion:
                GameMgr.playerData[0].GetPlayerstamina(GameUiMgr.single.nowSlot.item.itemPower);
                for (int i = 0; i < Inventory.Single.items.Count; i++)
                {
                    if (GameUiMgr.single.nowSlot.item.PrimaryCode == Inventory.Single.items[i].PrimaryCode)
                    {
                        AudioManager.single.PlaySfxClipChange(14);
                        Inventory.Single.RemoveItem(Inventory.Single.items[i]);
                        GameUiMgr.single.RedrawSlotUI();
                    }
                }

                GameUiMgr.single.SliderChange();
                break;
            case PopUpState.PartyCommit:
                GameUiMgr.single.EmploymentCompleted();
                GameUiMgr.single.popUp.gameObject.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(500, 300);
                
                if (GameMgr.single.GetPartyCommit())
                {
                    Debug.Log("----------------------------------------: " + (PlayerDifficulty)GameMgr.single.GetPlayerDifficulty());
                    GameUiMgr.single.UpdatePlayerRankAndQuestText((PlayerDifficulty)GameMgr.single.GetPlayerDifficulty());
                }
                else
                {
                    Debug.LogWarning("퀘스트 상태가 정상적으로 갱신되지 않았음.");
                }
                warningPartyCommit.gameObject.SetActive(false);
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

        if (popUpState == PopUpState.None || popUpState == PopUpState.NoStamina)
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
        if (_state == PopUpState.PartyCommit)
        {
            GameUiMgr.single.popUp.warningPartyCommit.gameObject.SetActive(true);
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
