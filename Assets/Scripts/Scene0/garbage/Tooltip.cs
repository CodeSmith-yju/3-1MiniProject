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
        if (_item.modifyStack > 0)
        {
            textName.text = "<color=red>+" + _item.modifyStack + "</color> " + _item.itemName;
        }
        else
            textName.text = _item.itemName;
        textDesc.text = _item.itemTitle;
        //textStat.text = _item.itemDesc;
        textStat.text = SetUpStatText(_item);

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

    string SetUpStatText(Item _item)
    {
        string _text = "";
        switch (_item.itemType)
        {
            case Item.ItemType.Equipment_Arrmor:
                _text = "방어도 + " + _item.itemPower.ToString();
                break;
            case Item.ItemType.Equipment_Boots:
                _text = "공격속도 + " + (_item.itemPower *100).ToString();
                break;
            case Item.ItemType.Equipment_Helmet:
                _text = "체력 + " + (_item.itemPower).ToString();
                break;
            case Item.ItemType.Equipment_Weapon:
                _text = "공격력 + " + (_item.itemPower).ToString();
                break;
            /*case Item.ItemType.Consumables:
                break;
            case Item.ItemType.Ect:
                break;*/
            default:
                _text = _item.itemDesc;
                break;
        }
        return _text;
    }
}
