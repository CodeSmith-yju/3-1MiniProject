using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RewardPopupInit : MonoBehaviour
{
    public TMP_Text popup_Title;
    public Transform inner_Gold_Exp; // 아이템 프리팹 생성 하거나 할 때
    public Transform inner_Item;
    public bool isBox;

    // 제목, 인_제목
    public void Init(string title, bool isBox) 
    {
        this.popup_Title.text = title;
        this.isBox = isBox;
    }
}
