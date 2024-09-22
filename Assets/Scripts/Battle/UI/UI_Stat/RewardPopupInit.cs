using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RewardPopupInit : MonoBehaviour
{
    public TMP_Text popup_Title;
    public Transform inner_Gold_Exp; // 골드, 경험치
    public Transform inner_Item; // 아이템
    public TMP_Text null_Item_Text;
    public bool isBox;

    // 제목, 인_제목
    public void Init(string title, bool isBox) 
    {
        this.popup_Title.text = title;
        this.isBox = isBox;
    }
}
