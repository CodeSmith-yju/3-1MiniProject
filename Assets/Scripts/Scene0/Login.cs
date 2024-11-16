using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    [Header("ID input")]
    [SerializeField] private TMP_InputField input_ID;
    public string id;

    [Header("PW input")]
    [SerializeField] private TMP_InputField input_PW;
    public string pw;

    [Header("Button")]
    [SerializeField] private Button btnLogin;
    [SerializeField] private Button btnNewAccount;

    public BtnAddUserNameSoundEvent hoverbtnLoginSoundEv;
    public BtnAddUserNameSoundEvent hoverNewAccountSoundEv;

    [Header("AddAccount")]
    [SerializeField] AddAccount ac;

    [Header("Focus")]
    [SerializeField] List<TMP_InputField> logins;
    [SerializeField] List<TMP_InputField> addaccounts;

    private void Awake()
    {
        //인스펙터창에서 할당함
        /* input_ID.onValueChanged.AddListener(InputID);
        input_PW.onValueChanged.AddListener(InputPW);*/
        RefreshFocusObj();
    }

    public int focus_MaxIndex;
    public int focus_nowIndex;
    private void Update()
    {
        GameObject selectedObject = EventSystem.current.currentSelectedGameObject;
        SetFocusNowIndex(selectedObject);

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            focus_nowIndex++;
            if (focus_nowIndex > focus_MaxIndex)// focus_nowIndex가 focus_MaxIndex를 초과하면 0으로 리셋
                focus_nowIndex = 0;

            ChangeFocusOnInputField();
        }
    }
    void SetFocusNowIndex(GameObject selectedObject)
    {
        if (selectedObject == input_ID)
        {
            focus_nowIndex = 0;
        }
        else if (selectedObject == input_PW)
        {
            focus_nowIndex = 1;
        }

    }
    void RefreshFocusObj()
    {
        focus_MaxIndex = -1;
        focus_nowIndex = -1;

        logins ??= new();
        addaccounts ??= new();

        logins.Clear();
        logins.Add(input_ID);
        logins.Add(input_PW);

        addaccounts.Clear();
        for (int i = 0; i < 3; i++)
        {
            addaccounts.Add(ac.GetInputFields(i));
        }

        focus_MaxIndex = ac.gameObject.activeSelf ? 2 : 1;
    }
    public void ChangeFocusOnInputField()
    {
        if (ac.gameObject.activeSelf == false)//Login Active
        {
            EventSystem.current.SetSelectedGameObject(logins[focus_nowIndex].gameObject);
        }
        else if (ac.gameObject.activeSelf == true)//AddAccount Active
        {
            EventSystem.current.SetSelectedGameObject(addaccounts[focus_nowIndex].gameObject);
        }
    }
    public void OpenLoginOrAddAccount(bool activeLogin)
    {
        if (activeLogin)
        {
            EventSystem.current.SetSelectedGameObject(logins[0].gameObject);
            focus_MaxIndex = 1;
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(addaccounts[0].gameObject);
            focus_MaxIndex = 2;
        }
        focus_nowIndex = 0;
    }

    private void InputFieldChanged(string input, ref string field, BtnAddUserNameSoundEvent soundEvent)
    {
        field = input;

        // 사운드 재생
        AudioManager.single.PlaySfxClipChange(0);

        // hover 상태 변경
        soundEvent.ishover = !string.IsNullOrEmpty(field);
    }

    public void InputID(string field_InputPlayerName)
    {
        InputFieldChanged(field_InputPlayerName, ref id, hoverbtnLoginSoundEv);
    }

    public void InputPW(string field_InputPlayerName)
    {
        InputFieldChanged(field_InputPlayerName, ref pw, hoverbtnLoginSoundEv);
    }

    private string RemoveUnderLine(string inputText)
    {
        string removeText = inputText.Replace("<u>", "").Replace("</u>", "");
        return removeText;
    }

    public void Sign_In()
    {
        id = RemoveUnderLine(id);
        pw = RemoveUnderLine(pw);
        Debug.Log("입력한 id : " + id);
        Debug.Log("입력한 pw : " + pw);

        // DB에서 ID와 PW가 일치하는지 확인하고 uID를 가져옴
        int uid = DBConnector.SelectUserAndGetUID(id, pw);

        if (uid > 0)
        {
            // uID 값을 DBConnector의 uid에 설정
            DBConnector.SetUID(uid);

            Debug.Log("로그인 성공, uid: " + uid);
            gameObject.transform.parent.gameObject.GetComponent<MainMenuMgr>().LoginSuccess();
            gameObject.transform.parent.gameObject.GetComponent<MainMenuMgr>().SaveDataCk();
            // 로그인 성공 처리 (예: 다음 씬으로 이동)
        }
        else
        {
            //Debug.Log("로그인 실패: ID 또는 PW가 잘못되었습니다.");
            GameMgr.single.popUp.SetPopUp("로그인 실패:\n ID 또는 PW가 잘못되었습니다", PopUpState.None);
            if (GameMgr.single.popUp.gameObject.activeSelf == false)
            {
                GameMgr.single.popUp.gameObject.SetActive(true);
                Debug.Log("Run if");
            }
            // 로그인 실패 처리 (예: 오류 메시지 표시)
        }

        hoverbtnLoginSoundEv.ishover = false;
    }


    public void AddNewAccount()
    {
        Debug.Log("Run AddAcnt ");
        OpenLoginPannel(false);
        OpenLoginOrAddAccount(false);

        ClearLoginPannel();

        hoverNewAccountSoundEv.ishover = false;
    }

    public void ClearLoginPannel()
    {
        input_ID.text = "";
        input_PW.text = "";
        id = input_ID.text;
        pw = input_PW.text;
    }

    public void OpenLoginPannel(bool onoff)
    {
        Debug.Log("Run Onoff : "+onoff);

        gameObject.transform.GetChild(1).gameObject.SetActive(onoff);
        ac.gameObject.SetActive(!onoff);
    }

    public void OnClickQuiteGame()
    {
        GameMgr.single.popUp.SetPopUp("게임을 종료 하시겠습니까?", PopUpState.Quite);
        if (GameMgr.single.popUp.gameObject.activeSelf == false)
        {
            GameMgr.single.popUp.gameObject.SetActive(true);
            Debug.Log("Run if");
        }
    }
}
