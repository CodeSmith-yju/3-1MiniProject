using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BasketBox : MonoBehaviour
{
    public ShopMgr shopMgr;
    [SerializeField] ShopSlot mySlot;
    [SerializeField] Item myItem;

    public TextMeshProUGUI BasketStack;
    public TextMeshProUGUI BasketName;
    public int stack;
    private int maxStack;
    public Image Icon;

    public Button btnPlus;
    public Button btnMinus;
    public void Init(ShopSlot _shopslot)
    {
        mySlot = _shopslot;
        myItem = _shopslot.GetItem();

        maxStack = myItem.itemStack;
        stack = 1;
        Icon.sprite = myItem.itemImage;
        BasketStack.text = stack.ToString();
        BasketName.text = myItem.itemName;

    }
    public void ClickPlus()
    {
        if (StackCk(mySlot))
        {
            if (stack < maxStack)
            {
                stack++;
                shopMgr.CalcPrice(mySlot.slotPirce);
                if (stack == maxStack)
                {
                    btnPlus.interactable = false;
                    btnMinus.interactable = true;
                }
                else
                {
                    btnPlus.interactable = true;
                }
            }
        }

        BasketStack.text = stack.ToString();
    }

    public void ClickMinus()
    {
        if (0 < stack)
        {
            stack--;
            shopMgr.CalcPrice(mySlot.slotPirce * -1);
            if (stack == 0)
            {
                btnMinus.interactable = false;
                btnPlus.interactable = true;
            }
            else
            {
                btnMinus.interactable = true;
            }
        }

        BasketStack.text = stack.ToString();
    }

    bool StackCk(ShopSlot _shopSlot)
    {
        if (mySlot.GetItem().itemType == Item.ItemType.Consumables || mySlot.GetItem().itemType == Item.ItemType.Ect)
            return true;
        return false;
    }

    public int BasketShopIndex()
    {
        return mySlot.shopIndex;
    }
}
