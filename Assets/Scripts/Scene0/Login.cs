using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

    /*private void Awake()//�ν�����â���� �Ҵ���
    {
        input_ID.onValueChanged.AddListener(InputID);
        input_PW.onValueChanged.AddListener(InputPW);
    }*/

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

        // DB���� ID�� PW�� ��ġ�ϴ��� Ȯ��
        bool loginSuccess = DBConnector.SelectUser(id, pw);

        if (loginSuccess)
        {
            Debug.Log("�α��� ����");
            gameObject.transform.parent.gameObject.GetComponent<MainMenuMgr>().LoginSuccess();
            // �α��� ���� ó�� (��: ���� ������ �̵�)
        }
        else
        {
            Debug.Log("�α��� ����: ID �Ǵ� PW�� �߸��Ǿ����ϴ�.");
            // �α��� ���� ó�� (��: ���� �޽��� ǥ��)
        }

        hoverbtnLoginSoundEv.ishover = false;
    }

    public void AddNewAccount()
    {
        Debug.Log("Run AddAcnt ");
        OpenLoginPannel(false);

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
}
