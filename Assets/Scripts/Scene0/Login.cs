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

    /*private void Awake()//인스펙터창에서 할당함
    {
        input_ID.onValueChanged.AddListener(InputID);
        input_PW.onValueChanged.AddListener(InputPW);
    }*/

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

        // DB에서 ID와 PW가 일치하는지 확인
        bool loginSuccess = DBConnector.SelectUser(id, pw);

        if (loginSuccess)
        {
            Debug.Log("로그인 성공");
            gameObject.transform.parent.gameObject.GetComponent<MainMenuMgr>().LoginSuccess();
            // 로그인 성공 처리 (예: 다음 씬으로 이동)
        }
        else
        {
            Debug.Log("로그인 실패: ID 또는 PW가 잘못되었습니다.");
            // 로그인 실패 처리 (예: 오류 메시지 표시)
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
