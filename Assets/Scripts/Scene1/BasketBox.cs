using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BasketBox : MonoBehaviour
{
    public ShopMgr shopMgr;
    [SerializeField] ShopSlot mySlot;
    [SerializeField] Item myItem;//디버그용으로 넣어둔거임 이제 필요없긴한데 놔둬도 딱히 문제는 안될듯

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
        /*수정 전
         * btnPlus.interactable = stack < maxStack;
        btnMinus.interactable = stack > 0;
        btnPlus와 btnMinus의 상태 초기화*/

        Debug.Log("Run UpdateBtn interactable");
        //수정 후
        btnPlus.interactable = true;
        btnMinus.interactable = true;

        // stack과 maxStack의 관계에 따라 버튼 상태 설정
        if (stack >= maxStack)
        {
            btnPlus.interactable = false; // maxStack에 도달하면 btnPlus 비활성화
            Debug.Log("stack is Max");
        }
        if (stack <= 0)
        {
            btnMinus.interactable = false; // stack이 0이면 btnMinus 비활성화
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
