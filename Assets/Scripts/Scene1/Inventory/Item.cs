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

    public string itemName;

    // 아이탬 개수 & 강화수치 stack 변수 추가
    public int itemStack = 0;
    public int modifyStack = 0;

    public Sprite itemImage;
    public Sprite typeIcon;

    //04-01 Add
    public string itemTitle;
    public string itemDesc;

    public int itemIndex;
    //05-08
    public int itemCode;
    //06
    public float itemPower;
    //08
    public int itemPrice;// 아이템가격
    //09
    public string PrimaryCode;

    // Init 생성자
    public Item(int code, ItemType type, string name, int stack, int modify, float power, int price, string title, string desc, int index) : this()
    {
        itemCode = code;
        Init(code, type, name, stack, modify, power, price, title, desc, index);
    }
    // Init 메서드
    void Init(int code, ItemType type, string name, int stack, int modify, float power, int price, string title, string desc, int index)
    {
        itemCode = code;
        itemType = type;
        itemName = name;

        itemStack = stack;
        modifyStack = modify;

        itemPower = power;
        itemPrice = price;

        itemTitle = title;
        itemDesc = desc;

        itemIndex = index;
    }

    public Item()//생성자를 통해 아이템 생성 시 고유한 PrimaryCode 생성
    {
        PrimaryCode = GenerateUniqueCode(12);
    }
    public string GetNewPK(int _12)
    {
        return GenerateUniqueCode(12);
    }
    private string GenerateUniqueCode(int length) // 12글자 이상의 무작위 문자열을 생성하는 메서드
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
    public string GetItemUniqueCode()
    {
        //Debug.Log("누가 나를 불렀느뇨: "+PrimaryCode);
        return PrimaryCode.ToString();
    }

    //뭔가 장비아이템을 사용할때 사용될 메서드를 여기다 만들어야하는듯..?
    public Item UpgradeModifyPowerSet(Item _item)
    {
        float _f = GetPreViewPower(_item);
        if (_item.modifyStack > -1)
        {
            _item.modifyStack++;
            _item.itemPower = _f;
            _item.itemPrice = ItemResources.instance.itemRS[_item.itemCode].itemPrice + (int)((ItemResources.instance.itemRS[_item.itemCode].itemPrice * 0.1) * _item.modifyStack);
            /*switch (_item.modifyStack)
            {
                case 0:
                    break
                default:
                    _item.modifyStack++;
                    _item.itemPower = ItemResources.instance.itemRS[_item.itemCode].itemPower + (ItemResources.instance.itemRS[_item.itemCode].itemPower) * 0.5f * _item.modifyStack;
                    _item.itemPrice = ItemResources.instance.itemRS[_item.itemCode].itemPrice + (int)((ItemResources.instance.itemRS[_item.itemCode].itemPrice * 0.1) * _item.modifyStack);
                    break
            }*/
        }
        return _item;
    }
    public float GetPreViewPower(Item _item)
    {
        //float _i = _item.modifyStack; // _item.modifyStack을 바로 사용
        Item tmp = ItemResources.instance.itemRS[_item.itemCode - 4];
        float power = _item.itemPower;
        /*switch (_item.itemType)
        {
            case ItemType.Equipment_Arrmor:
                power += tmp.itemPower;
                break;
            case ItemType.Equipment_Boots:
                power += tmp.itemPower;
                break;
            default:
                power += tmp.itemPower / 2;
                break;
        }*/
        power += tmp.itemPower;

        Debug.Log("Before: " + _item.itemPower + " / After: " + power + " | _i: ");
        return RoundToTwoDecimalPlaces(power);
    }
    public float RoundToTwoDecimalPlaces(float value)
    {
        return (float)Math.Round(value, 3); // 소수점 두 자리로 반올림
    }
    public void IconSet(Item _item)
    {
        switch (_item.itemType)
        {
            case ItemType.Equipment_Arrmor://Range 40
                _item.typeIcon = Inventory.Single.typeIconRS[0];
                break;
            case ItemType.Equipment_Boots://Atk spd 07
                _item.typeIcon = Inventory.Single.typeIconRS[1];
                break;
            case ItemType.Equipment_Helmet://HP 43
                _item.typeIcon = Inventory.Single.typeIconRS[2];
                break;
            case ItemType.Equipment_Weapon://Atk 72
                _item.typeIcon = Inventory.Single.typeIconRS[3];
                break;
            case ItemType.Consumables:// + 11
                _item.typeIcon = Inventory.Single.typeIconRS[4];
                break;
            case ItemType.Ect:// 29
                _item.typeIcon = Inventory.Single.typeIconRS[5];
                break;
            default:
                break;
        }
    }

    public Item GenerateRandomItem(int r)
    {
        Item _item = new()
        {
            itemCode = ItemResources.instance.itemRS[r].itemCode,
            itemName = ItemResources.instance.itemRS[r].itemName,
            itemType = ItemResources.instance.itemRS[r].itemType,
            itemTitle = ItemResources.instance.itemRS[r].itemTitle,
            itemImage = ItemResources.instance.itemRS[r].itemImage,
            itemPrice = ItemResources.instance.itemRS[r].itemPrice,
            itemPower = ItemResources.instance.itemRS[r].itemPower,
            itemDesc = ItemResources.instance.itemRS[r].itemDesc,
            itemStack = ItemResources.instance.itemRS[r].itemStack,
            modifyStack = ItemResources.instance.itemRS[r].modifyStack,
            typeIcon = ItemResources.instance.itemRS[r].typeIcon,
            //PK는 자동생성되게 ㄱㄱ
        };

        return _item;
    }
}

