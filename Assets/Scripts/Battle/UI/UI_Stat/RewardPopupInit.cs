using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RewardPopupInit : MonoBehaviour
{
    public TMP_Text popup_Title;
    public Transform inner_Gold_Exp; // ���, ����ġ
    public Transform inner_Item; // ������
    public TMP_Text null_Item_Text;
    public bool isBox;

    // ����, ��_����
    public void Init(string title, bool isBox) 
    {
        this.popup_Title.text = title;
        this.isBox = isBox;
    }
}
