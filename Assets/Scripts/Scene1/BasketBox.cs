using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BasketBox : MonoBehaviour
{
    public ShopMgr shopMgr;
    [SerializeField] ShopSlot mySlot;
    [SerializeField] Item myItem;//����׿����� �־�а��� ���� �ʿ�����ѵ� ���ֵ� ���� ������ �ȵɵ�

    public TextMeshProUGUI BasketStack;
    public TextMeshProUGUI BasketName;
    public int stack;
    [SerializeField]private int maxStack;
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

        UpdateButtonState();
    }
    public void ClickPlus()
    {
        if (stack < maxStack)
        {
            stack++;
            shopMgr.CalcPrice(mySlot.slotPirce);
            UpdateButtonState();
        }

        BasketStack.text = stack.ToString();
    }

    public void ClickMinus()
    {
        if (0 < stack)
        {
            stack--;
            shopMgr.CalcPrice(mySlot.slotPirce * -1);
            UpdateButtonState();
        }

        BasketStack.text = stack.ToString();
    }

    private void UpdateButtonState()
    {
        /*���� ��
         * btnPlus.interactable = stack < maxStack;
        btnMinus.interactable = stack > 0;
        btnPlus�� btnMinus�� ���� �ʱ�ȭ*/

        Debug.Log("Run UpdateBtn interactable");
        //���� ��
        btnPlus.interactable = true;
        btnMinus.interactable = true;

        // stack�� maxStack�� ���迡 ���� ��ư ���� ����
        if (stack >= maxStack)
        {
            btnPlus.interactable = false; // maxStack�� �����ϸ� btnPlus ��Ȱ��ȭ
            Debug.Log("stack is Max");
        }
        if (stack <= 0)
        {
            btnMinus.interactable = false; // stack�� 0�̸� btnMinus ��Ȱ��ȭ
            Debug.Log("stack is 0");
        }
    }

    public int BasketShopIndex()
    {
        return mySlot.shopIndex;
    }
    public ShopSlot GetBasketShopSlot()
    {
        return mySlot;
    }
    public Item GetBasketItem()
    {
        return myItem;
    }
}
