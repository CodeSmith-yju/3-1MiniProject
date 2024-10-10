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
        text_modify_stack.text = myItem.itemPower.ToString();
        if (myItem.modifyStack < 1)
        {
            text_modify_stack.gameObject.SetActive(false);
        }
        else
        {
            text_modify_stack.gameObject.SetActive(true);
        }
        text_pre_stat.text = myItem.itemPower.ToString()+" -> " + myItem.GetPreViewPower(myItem).ToString() + "(+ "+ (myItem.GetPreViewPower(myItem)-myItem.itemPower).ToString() + ")";
    }

}
