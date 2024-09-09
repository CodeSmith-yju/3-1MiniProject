using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
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

    // ������ ���� & ��ȭ��ġ stack ���� �߰�
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
    public int itemPrice;// �����۰���
    //09
    public string PrimaryCode;
    public Item()//�����ڸ� ���� ������ ���� �� ������ PrimaryCode ����
    {
        PrimaryCode = GenerateUniqueCode(12);
    }
    private string GenerateUniqueCode(int length) // 12���� �̻��� ������ ���ڿ��� �����ϴ� �޼���
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        StringBuilder code = new StringBuilder();
        System.Random random = new System.Random();

        for (int i = 0; i < length; i++)
        {
            code.Append(chars[random.Next(chars.Length)]);
        }

        return code.ToString();
    }
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
    public Item UpgradeModifyPowerSet(Item _item)
    {
        if (_item.modifyStack > 0)
        {
            switch (_item.modifyStack)
            {
                /*case 0:
                    break;*/
                default:
                    _item.modifyStack++;
                    _item.itemPower = ItemResources.instance.itemRS[_item.itemCode].itemPower + (ItemResources.instance.itemRS[_item.itemCode].itemPower)*0.5f*_item.modifyStack;
                    _item.itemPrice = ItemResources.instance.itemRS[_item.itemCode].itemPrice + (int)((ItemResources.instance.itemRS[_item.itemCode].itemPrice * 0.1)*_item.modifyStack);
                    break;
            }
        }
        return _item;
    }

}

