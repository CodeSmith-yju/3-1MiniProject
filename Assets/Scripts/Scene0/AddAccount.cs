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

    /*private void Awake()//�ν�����â���� �Ҵ���
    {
        input_ID.onValueChanged.AddListener(InputID);
        input_PW.onValueChanged.AddListener(InputPW);
        input_PW2.onValueChanged.AddListener(InputPW2);
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
        
        //��й�ȣ ���Ἲ �˻� ���� ����
        if (pw.Equals(pw2))
        {
            //��й�ȣ�� ��ġ

            // ȸ������ ���� ����
            bool registrationSuccess = DBConnector.InsertUser(id, pw);

            if (registrationSuccess)
            {
                Debug.Log("ȸ������ ����");
                // ȸ������ ���� �� ó�� (��: �α��� ȭ������ ��ȯ) �� �ʱ�ȭ

                int userID = DBConnector.SelectUserAndGetUID(id, pw);

                if (userID > 0)
                {
                    DBConnector.SetUID(userID); // UID ����
                    Debug.Log("UID ���� �Ϸ�: " + userID);

                    // �α��� ȭ������ ��ȯ
                    gameObject.transform.parent.gameObject.transform.parent.GetComponent<MainMenuMgr>().OnClickedGameStart();
                    ClearNewAccountPannel();
                }
                else
                {
                    Debug.LogError("UID �������� ����: ���� �����Ͱ� �������� �ʽ��ϴ�.");
                    GameMgr.single.popUp.SetPopUp("ȸ������ ����: UID �������� ����", PopUpState.None);
                }
            }
            else
            {
                GameMgr.single.popUp.SetPopUp("ȸ������ ����: ID �ߺ�", PopUpState.None);
                if (GameMgr.single.popUp.gameObject.activeSelf == false)
                {
                    GameMgr.single.popUp.gameObject.SetActive(true);
                    Debug.Log("Run if");
                }
                Debug.LogError("overlap ȸ������ ����: ID�� �ߺ��Ǿ��ų� �ٸ� ������ �߻��߽��ϴ�.");
                // ȸ������ ���� �� ó�� (��: ���� �޽��� ���)
            }


        }
        else
        {
            //��й�ȣ ����ġ
            GameMgr.single.popUp.SetPopUp("ȸ������ ����: \n��й�ȣ ����ġ", PopUpState.None);
            if (GameMgr.single.popUp.gameObject.activeSelf == false)
            {
                GameMgr.single.popUp.gameObject.SetActive(true);
                Debug.Log("Run if");
            }
            Debug.Log("Defects ��й�ȣ ����ġ�� ȸ������ �̽���");
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
