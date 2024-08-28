using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[System.Serializable]
public class Item
{
    public enum ItemType//04-01 Add
    {
        Equipment_Arrmor,
        Equipment_Boots,
        Equipment_Helmet,
        Equipment_Weapon,
        Consumables,
        Ect
    }
    public ItemType itemType;

    // 아이탬 개수 & 강화수치 stack 변수 추가
    public int itemStack = 0;
    public int modifyStack = 0;
    
    public string itemName;
    public Sprite itemImage;
    public List<ItemEffect> efts;
    //04-01 Add
    public string itemTitle;
    public string itemDesc;

    public int itemIndex;
    //04-22
    public bool isDraggable;
    //05-08
    public int itemCode;
    //06
    public float itemPower;
    //08
    public int itemPrice;// 아이템가격
    public bool Use()
    {
        bool isUsed = false;
        
        foreach (ItemEffect eft in efts)
        {
            isUsed = eft.ExcuteRole();
        }

        return isUsed;
    }
    //뭔가 장비아이템을 사용할때 사용될 메서드를 여기다 만들어야하는듯..?
    public Item ModifyPowerSet(Item _item)
    {
        if (_item.modifyStack > 0)
        {
            _item.itemPower *= 1.1f;
            _item.itemPrice = _item.itemPrice + (int)(_item.itemPrice * 0.1f) * _item.modifyStack;
        }

        return _item;
    }

}

