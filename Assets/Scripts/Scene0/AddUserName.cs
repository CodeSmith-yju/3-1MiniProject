using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class AddUserName : MonoBehaviour
{
    [Header("ĳ���� �̸�")]
    [SerializeField] private TMP_InputField field_InputPlayerName;
    public string playerName;
    int maxLength = 8;
    public TextMeshProUGUI text_Warning;

    public BtnAddUserNameSoundEvent hoverSoundEv;
    // Btn Start
    [SerializeField] private Button btnStart;

    private void Awake()
    {
        field_InputPlayerName.onValueChanged.AddListener(OnInputValueChanged);
        btnStart.interactable = false;
        text_Warning.text = "�ִ� "+ maxLength + "���ڱ��� �Է� �����մϴ�.";
    }

    private void OnInputValueChanged(string field_InputPlayerName)
    {
        if (field_InputPlayerName.Length > maxLength)
        {
            // �Էµ� �̸��� maxLength���ڸ� ������ maxLength���ڷ� �ڸ��� �ٽ� ����
            this.field_InputPlayerName.text = field_InputPlayerName.Substring(0, maxLength);
            return;
        }
        /*if (string.IsNullOrEmpty(field_InputPlayerName))
        {
            btnStart.interactable = false;
        }
        else
        {
            playerName = field_InputPlayerName;
            btnStart.interactable = true;
        }*/

        playerName =field_InputPlayerName;
        btnStart.interactable = !string.IsNullOrEmpty(field_InputPlayerName);

        //06-14 Add Text IO Event Sound
        AudioManager.single.PlaySfxClipChange(0);

        if (!string.IsNullOrEmpty(playerName))
        {
            hoverSoundEv.ishover = true;
        }
        else
        {
            hoverSoundEv.ishover = false;
        }
    }
    private string RemoveUnderLine(string inputText)
    {
        string removeText = inputText.Replace("<u>", "").Replace("</u>", "");
        return removeText;
    }

    public void OnStartGame()
    {
        RemoveUnderLine(playerName);
        if (string.IsNullOrEmpty(playerName))
        {
            btnStart.interactable = false;
            return;
        }
        if (playerName.Length > maxLength)
        {
            // 10���ڸ� �ʰ��� ��� ó��
            GameMgr.single.popUp.SetPopUp("�̸��� " + maxLength + "���ڸ� �ʰ��� �� �����ϴ�.", PopUpState.None);
            if (GameMgr.single.popUp.gameObject.activeSelf == false)
            {
                GameMgr.single.popUp.gameObject.SetActive(true);
                Debug.Log("Run if");
            }
        }
        else
        {
            Debug.Log("��ȿ�� �̸��Դϴ�.");
            GameMgr.single.input_Name = playerName;
            GameMgr.single.popUp.SetPopUp(playerName + "��\n ������ �����Ͻðڽ��ϱ�?", PopUpState.GameStart);
            if (GameMgr.single.popUp.gameObject.activeSelf == false)
            {
                GameMgr.single.popUp.gameObject.SetActive(true);
                Debug.Log("Run if");
            }
        }

        hoverSoundEv.ishover = false;
    }

}
