using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ShopState
{
    BUY,
    SELL,
    SOLDOUT
}
public class ShopSlot : MonoBehaviour
{
    [SerializeField] private Item item;
    [SerializeField] private ShopState state;
    [SerializeField] private ShopMgr shopMgr;
    public int slotPirce = 0;
    public int shopIndex = 0;

    public TextMeshProUGUI textItemPrice;
    public TextMeshProUGUI textItemStack;
    public TextMeshProUGUI textItemModifyStack;
    public Image imgSlodOut;
    public Image imgUse;

    public Image slotIcon;

    private Button slotMy;
    public void Init(Item _item, ShopState _state)
    {
        item = _item;
        state = _state;

        //Debug.Log($"Item Price: {item.itemPrice}");
        Debug.Log("지금생성된 아이템의 스택: " + _item.itemStack);

        slotPirce = item.itemPrice;
        if (state == ShopState.SELL)
        {
            slotPirce = (int)(slotPirce * 0.7);
        }
        slotIcon.sprite = item.itemImage;
        textItemPrice.text = slotPirce.ToString();
        DrawBuyShop(item);

        slotMy = GetComponent<Button>();
    }

    public void DrawBuyShop(Item _item)//상점 아이템 표시 초기화
    {
        if (_item.itemType == Item.ItemType.Consumables || _item.itemType == Item.ItemType.Ect)
        {
            // 소모품일 경우 표시
            if (_item.itemStack > 0)
            {
                textItemStack.text = _item.itemStack.ToString();
                textItemStack.gameObject.SetActive(true);
            }
            else
            {
                textItemStack.gameObject.SetActive(false);
                if (state != ShopState.SOLDOUT)
                {
                    state = ShopState.SOLDOUT;
                    imgSlodOut.gameObject.SetActive(true);
                }
                return;
            }
        }
        else
        {
            // 장비아이템일 경우 표시
            if (_item.modifyStack > 0)
            {
                textItemModifyStack.text = "+"+_item.modifyStack.ToString();
                textItemModifyStack.gameObject.SetActive(true);
            }
            else
                textItemModifyStack.gameObject.SetActive(false);
        }
    }
    public Item GetItem()//상점 '판매'아이템 표시 초기화
    {
        return item;
    }

    public void InBasket()
    {
        // 프리팹을 인스턴스화하여 ShopSlot을 생성
        BasketBox basket = PoolBasketBox(shopMgr.baskets);
        if (basket == null)
        {
            basket = Instantiate(shopMgr.prBasketBox, shopMgr.trBasketBox);
        }
        // 생성된 슬롯 초기화
        basket.Init(this);
        basket.shopMgr = shopMgr;
        // 생성된 슬롯을 리스트에 추가
        shopMgr.baskets.Add(basket);

        UseImgSet(true);

        //가격 계산
        shopMgr.CalcPrice(slotPirce);
    }
    BasketBox PoolBasketBox(List<BasketBox> _b)
    {
        foreach (BasketBox _box in _b)
        {
            if (!_box.gameObject.activeSelf)
            {
                _box.gameObject.SetActive(true);
                return _box;
            }
        }
        return null;
    }

    public void ShopMgrSet(ShopMgr _shopMgr)
    {
        shopMgr = _shopMgr;
    }

    public void UseImgSet(bool active)
    {
        if (active)
        {
            slotMy.interactable = false;
            imgUse.gameObject.SetActive(true);
        }
        else
        {
            slotMy.interactable = true;
            imgUse.gameObject.SetActive(false);
        }
    }
}
