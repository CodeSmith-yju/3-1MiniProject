using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUseUIDescInit : MonoBehaviour
{
    string item_Name;
    public TMP_Text desc_Text;
    public Button use_Bnt;

    public void Init(string name)
    {
        item_Name = name;
        desc_Text.text = item_Name + "을\n사용 하시겠습니까?";
    }



}
