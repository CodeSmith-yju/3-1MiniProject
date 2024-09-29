using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class AddUserName : MonoBehaviour
{
    [Header("캐릭터 이름")]
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
        text_Warning.text = "최대 "+ maxLength + "글자까지 입력 가능합니다.";
    }

    private void OnInputValueChanged(string field_InputPlayerName)
    {
        if (field_InputPlayerName.Length > maxLength)
        {
            // 입력된 이름이 maxLength글자를 넘으면 maxLength글자로 자르고 다시 설정
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
            // 10글자를 초과한 경우 처리
            GameMgr.single.popUp.SetPopUp("이름은 " + maxLength + "글자를 초과할 수 없습니다.", PopUpState.None);
            if (GameMgr.single.popUp.gameObject.activeSelf == false)
            {
                GameMgr.single.popUp.gameObject.SetActive(true);
                Debug.Log("Run if");
            }
        }
        else
        {
            Debug.Log("유효한 이름입니다.");
            GameMgr.single.input_Name = playerName;
            GameMgr.single.popUp.SetPopUp(playerName + "로\n 게임을 시작하시겠습니까?", PopUpState.GameStart);
            if (GameMgr.single.popUp.gameObject.activeSelf == false)
            {
                GameMgr.single.popUp.gameObject.SetActive(true);
                Debug.Log("Run if");
            }
        }

        hoverSoundEv.ishover = false;
    }

}
