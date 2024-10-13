using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffTooltip : MonoBehaviour
{
    [SerializeField] TMP_Text buff_Desc;
    [SerializeField] Image buff_Icon;

    public void Init(string desc, Sprite icon)
    {
        this.buff_Desc.text = desc;
        buff_Icon.sprite = icon;
    }

}
