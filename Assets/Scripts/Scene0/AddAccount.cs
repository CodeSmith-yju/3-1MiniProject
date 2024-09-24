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

        // ȸ������ ���� ����
        bool registrationSuccess = DBConnector.InsertUser(id, pw);

        if (registrationSuccess)
        {
            Debug.Log("ȸ������ ����");
            // ȸ������ ���� �� ó�� (��: �α��� ȭ������ ��ȯ)
        }
        else
        {
            Debug.LogError("ȸ������ ����: ID�� �ߺ��Ǿ��ų� �ٸ� ������ �߻��߽��ϴ�.");
            // ȸ������ ���� �� ó�� (��: ���� �޽��� ���)
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
