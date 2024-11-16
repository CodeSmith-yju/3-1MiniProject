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
        //�ν�����â���� �Ҵ���
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
            if (focus_nowIndex > focus_MaxIndex)// focus_nowIndex�� focus_MaxIndex�� �ʰ��ϸ� 0���� ����
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

        // ���� ���
        AudioManager.single.PlaySfxClipChange(0);

        // hover ���� ����
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
        Debug.Log("�Է��� id : " + id);
        Debug.Log("�Է��� pw : " + pw);

        // DB���� ID�� PW�� ��ġ�ϴ��� Ȯ���ϰ� uID�� ������
        int uid = DBConnector.SelectUserAndGetUID(id, pw);

        if (uid > 0)
        {
            // uID ���� DBConnector�� uid�� ����
            DBConnector.SetUID(uid);

            Debug.Log("�α��� ����, uid: " + uid);
            gameObject.transform.parent.gameObject.GetComponent<MainMenuMgr>().LoginSuccess();
            gameObject.transform.parent.gameObject.GetComponent<MainMenuMgr>().SaveDataCk();
            // �α��� ���� ó�� (��: ���� ������ �̵�)
        }
        else
        {
            //Debug.Log("�α��� ����: ID �Ǵ� PW�� �߸��Ǿ����ϴ�.");
            GameMgr.single.popUp.SetPopUp("�α��� ����:\n ID �Ǵ� PW�� �߸��Ǿ����ϴ�", PopUpState.None);
            if (GameMgr.single.popUp.gameObject.activeSelf == false)
            {
                GameMgr.single.popUp.gameObject.SetActive(true);
                Debug.Log("Run if");
            }
            // �α��� ���� ó�� (��: ���� �޽��� ǥ��)
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
        GameMgr.single.popUp.SetPopUp("������ ���� �Ͻðڽ��ϱ�?", PopUpState.Quite);
        if (GameMgr.single.popUp.gameObject.activeSelf == false)
        {
            GameMgr.single.popUp.gameObject.SetActive(true);
            Debug.Log("Run if");
        }
    }
}
