using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Preview : MonoBehaviour
{
    [SerializeField] Item myItem;
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI text_name;
    [SerializeField] TextMeshProUGUI text_modify_stack;
    [SerializeField] TextMeshProUGUI text_pre_stat;

    public void Init(Item _item)
    {
        myItem = _item;
        icon.sprite = myItem.itemImage;
        text_name.text = myItem.itemName;

        text_modify_stack.gameObject.SetActive(true);
        text_modify_stack.text = "+"+ (myItem.modifyStack + 1).ToString();

        string str = SetUpStatText(myItem);
        if (_item.itemType == Item.ItemType.Equipment_Arrmor || _item.itemType == Item.ItemType.Equipment_Boots)
        {
            float _f = (myItem.GetPreViewPower(myItem) * 100);
            text_pre_stat.text = str + (myItem.itemPower * 100).ToString() + " -> " + _f + "(<color=red> +" + (_f - myItem.itemPower * 100).ToString("F0") + "</color>)";
        }
        else
        {
            float _f = myItem.GetPreViewPower(myItem);
            text_pre_stat.text = str + myItem.itemPower.ToString() + " -> " + _f.ToString() + "(<color=red> +" + (_f - myItem.itemPower).ToString() + "</color>)";
        }
    }
    string SetUpStatText(Item _item)
    {
        string _text = "";
        switch (_item.itemType)
        {
            case Item.ItemType.Equipment_Arrmor:
                _text = "���ݻ�Ÿ� +";
                break;
            case Item.ItemType.Equipment_Boots:
                _text = "���ݼӵ� +";
                break;
            case Item.ItemType.Equipment_Helmet:
                _text = "ü�� +";
                break;
            case Item.ItemType.Equipment_Weapon:
                _text = "���ݷ� +";
                break;
            default:
                _text = _item.itemDesc;
                break;
        }
        return _text;
    }
}
