using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RewardPopupInit : MonoBehaviour
{
    public TMP_Text popup_Title;
    public Transform inner_Main; // ������ ������ ���� �ϰų� �� ��
    public bool isBox;

    public TMP_Text total_Gold;
    public TMP_Text total_Exp;

    // ����, ��_����
    public void Init(string title, bool isBox) 
    {
        this.popup_Title.text = title;
        this.isBox = isBox;
    }

    public void TotalRewardInit(string gold, string exp)
    {
        total_Gold.text = gold + " Gold";
        total_Exp.text = exp + " Exp";
    }

}
