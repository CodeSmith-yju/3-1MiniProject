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

    void ListnerSet()//Ŭ�� �̺�Ʈ ������ �߰��ߴµ� ���׳��� �ϴܻ�
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
                BattleManager.Instance.ui.OpenPopup(BattleManager.Instance.ui.exit_Popup); // ���� �ߴ� ���â
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
        text_PopUp.text = _str;  // �˾� �ؽ�Ʈ ����
        popUpState = _state;     // �˾� ���� ����

        if (popUpState == PopUpState.None)
            btn_No.gameObject.SetActive(false);
        else
            btn_No.gameObject.SetActive(true);

        if (!gameObject.activeSelf)  // ��Ȱ��ȭ�� ���¶�� �˾��� Ȱ��ȭ
        {
            gameObject.SetActive(true);
            Debug.Log("�˾� Ȱ��ȭ: " + _str);
        }
        else
        {
            Debug.LogWarning("�˾��� �̹� Ȱ��ȭ�Ǿ� �ֽ��ϴ�.");
        }

        Canvas.ForceUpdateCanvases();  // ������ UI ����
    }

    public void SetPopUp(string _str, PopUpState _state, int Lv)
    {
        text_PopUp.text = _str;  // �˾� �ؽ�Ʈ ����
        popUpState = _state;     // �˾� ���� ����

        if (popUpState == PopUpState.None)
            btn_No.gameObject.SetActive(false);
        else
            btn_No.gameObject.SetActive(true);

        if (!gameObject.activeSelf)  // ��Ȱ��ȭ�� ���¶�� �˾��� Ȱ��ȭ
        {
            gameObject.SetActive(true);
            Debug.Log("�˾� Ȱ��ȭ: " + _str);
        }
        else
        {
            Debug.LogWarning("�˾��� �̹� Ȱ��ȭ�Ǿ� �ֽ��ϴ�.");
        }

        Canvas.ForceUpdateCanvases();  // ������ UI ����
    }
    public void GameQuite()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
