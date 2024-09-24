using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddAccount : MonoBehaviour
{
    [Header("ID input")]
    [SerializeField] private TMP_InputField input_ID;
    public string id;

    [Header("PW input")]
    [SerializeField] private TMP_InputField input_PW;
    [SerializeField] private TMP_InputField input_PW2;
    public string pw;
    public string pw2;

    [Header("Button")]
    [SerializeField] private Button btnCommit;

    public BtnAddUserNameSoundEvent hoverCommitSoundEv;

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
        InputFieldChanged(field_InputPlayerName, ref id, hoverCommitSoundEv);
    }

    public void InputPW(string field_InputPlayerName)
    {
        InputFieldChanged(field_InputPlayerName, ref pw, hoverCommitSoundEv);
    }

    private string RemoveUnderLine(string inputText)
    {
        string removeText = inputText.Replace("<u>", "").Replace("</u>", "");
        return removeText;
    }

    public void AddNewAccount()
    {
        // AccountPage.SetActive(true);

        id = RemoveUnderLine(id);
        pw = RemoveUnderLine(pw);

        // 회원가입 로직 실행
        bool registrationSuccess = DBConnector.InsertUser(id, pw);

        if (registrationSuccess)
        {
            Debug.Log("회원가입 성공");
            // 회원가입 성공 시 처리 (예: 로그인 화면으로 전환)
        }
        else
        {
            Debug.LogError("회원가입 실패: ID가 중복되었거나 다른 문제가 발생했습니다.");
            // 회원가입 실패 시 처리 (예: 오류 메시지 출력)
        }


        hoverCommitSoundEv.ishover = false;
    }

    public void ClearLoginPannel()
    {
        input_ID.text = "";
        input_PW.text = "";
        id = input_ID.text;
        pw = input_PW.text;
    }
}
