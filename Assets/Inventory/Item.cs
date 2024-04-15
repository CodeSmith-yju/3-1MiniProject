using System.Collections;
using System.Collections.Generic;
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
    //public int itemStack = 1;
    public string itemName;
    public Sprite itemImage;
    public List<ItemEffect> efts;
    //04-01 Add
    public string itemTitle;
    public string itemDesc;

    public int itemIndex;

    public bool Use()
    {
        bool isUsed = false;
        
        foreach (ItemEffect eft in efts)
        {
            isUsed = eft.ExcuteRole();
        }

        return isUsed;
    }
    //���� ���������� ����Ҷ� ���� �޼��带 ����� �������ϴµ�..?
}

