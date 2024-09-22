using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    [Header("Text")]
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textDesc;
    public TextMeshProUGUI textStat;
    //public TextMeshProUGUI textPower;
    [Header("Image")]
    public Image imgIcon;
    public Image powerIcon;

    private float canvaseWidth;
    private RectTransform tooltipRect;

    public void SetupTooltip(Item _item)//(string _name, string _title, string _desc,Sprite _img)
    {
        textName.text = _item.itemName;
        textDesc.text = _item.itemTitle;
        textStat.text = _item.itemDesc;

        imgIcon.sprite = _item.itemImage;
        powerIcon.sprite = _item.typeIcon;
    }
    public void TooltipSetting(float _canvasWidth, RectTransform _tooltipRect)
    {
        //ItemResources.instance.AfterIconSet();
        canvaseWidth = _canvasWidth;
        tooltipRect = _tooltipRect;
    }

    public void MoveTooltip()
    {
        transform.position = Input.mousePosition;
        // 04-15 ToolTip
        tooltipRect = GetComponent<RectTransform>();

        if (tooltipRect.anchoredPosition.x + tooltipRect.sizeDelta.x > canvaseWidth)
            tooltipRect.pivot = new Vector2(1, 0);
        else
            tooltipRect.pivot = new Vector2(0, 0);
    }
}
