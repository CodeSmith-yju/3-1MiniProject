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
        input_PW2.onValueChanged.AddListener(InputPW2);
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
    public void InputPW2(string field_InputPlayerName)
    {
        InputFieldChanged(field_InputPlayerName, ref pw2, hoverCommitSoundEv);
    }

    private string RemoveUnderLine(string inputText)
    {
        string removeText = inputText.Replace("<u>", "").Replace("</u>", "");
        return removeText;
    }

    public void AddNewAccount()
    {
        id = RemoveUnderLine(id);
        pw = RemoveUnderLine(pw);
        pw2 = RemoveUnderLine(pw2);
        
        //비밀번호 무결성 검사 로직 실행
        if (pw.Equals(pw2))
        {
            //비밀번호가 일치

            // 회원가입 로직 실행
            bool registrationSuccess = DBConnector.InsertUser(id, pw);

            if (registrationSuccess)
            {
                Debug.Log("회원가입 성공");
                // 회원가입 성공 시 처리 (예: 로그인 화면으로 전환) 및 초기화

                int userID = DBConnector.SelectUserAndGetUID(id, pw);

                if (userID > 0)
                {
                    DBConnector.SetUID(userID); // UID 설정
                    Debug.Log("UID 설정 완료: " + userID);

                    // 로그인 화면으로 전환
                    gameObject.transform.parent.gameObject.transform.parent.GetComponent<MainMenuMgr>().OnClickedGameStart();
                    ClearNewAccountPannel();
                }
                else
                {
                    Debug.LogError("UID 가져오기 실패: 유저 데이터가 존재하지 않습니다.");
                    GameMgr.single.popUp.SetPopUp("회원가입 실패: UID 가져오기 실패", PopUpState.None);
                }
            }
            else
            {
                GameMgr.single.popUp.SetPopUp("회원가입 실패: ID 중복", PopUpState.None);
                if (GameMgr.single.popUp.gameObject.activeSelf == false)
                {
                    GameMgr.single.popUp.gameObject.SetActive(true);
                    Debug.Log("Run if");
                }
                Debug.LogError("overlap 회원가입 실패: ID가 중복되었거나 다른 문제가 발생했습니다.");
                // 회원가입 실패 시 처리 (예: 오류 메시지 출력)
            }


        }
        else
        {
            //비밀번호 불일치
            GameMgr.single.popUp.SetPopUp("회원가입 실패: \n비밀번호 불일치", PopUpState.None);
            if (GameMgr.single.popUp.gameObject.activeSelf == false)
            {
                GameMgr.single.popUp.gameObject.SetActive(true);
                Debug.Log("Run if");
            }
            Debug.Log("Defects 비밀번호 불일치로 회원가입 미실행");
        }

        hoverCommitSoundEv.ishover = false;
    }

    public void ClearNewAccountPannel()
    {
        input_ID.text = "";
        input_PW.text = "";
        input_PW2.text = "";

        id = input_ID.text;
        pw = input_PW.text;
        pw2 = input_PW2.text;
    }
    void OffAcPannel()
    {
        gameObject.transform.parent.GetComponent<Login>().OpenLoginPannel(true);
    }
}
