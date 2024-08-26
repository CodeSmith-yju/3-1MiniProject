using DarkPixelRPGUI.Scripts.UI.Equipment;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ShopMgr : MonoBehaviour
{
    [SerializeField] List<ShopSlot> shopSlots;
    [SerializeField] List<ShopSlot> playerShopItems;

    [SerializeField] ShopSlot slotPrefab;
    public ShopState shopState;

    [Header("Tab")]
    [SerializeField] Button buyTab;
    [SerializeField] Button sellTab;

    [Header("Pannel")]
    public GameObject buyPannel;
    public GameObject sellPannel;

    public Transform tfBuy;
    public Transform tfSell;

    //[SerializeField] Button btnBuy;// TODO: Add SoundEv 

    private void Start()
    {
        shopState = ShopState.BUY;
        OpenTap(shopState);

        //이 아래의 for문 생성로직은 GameUIMgr로 빼내야할거같음 여기서관리하기엔조금? 연계된 기능이많은듯
        for (int i = 0; i < 18; i++)
        {
            // 아이템을 생성
            Item newItem = ItemResources.instance.itemRS[Random.Range(0, 6)];

            // 프리팹을 인스턴스화하여 ShopSlot을 생성
            ShopSlot slot = Instantiate(slotPrefab, tfBuy);

            // 생성된 슬롯 초기화
            slot.Init(newItem, ShopState.BUY);

            // 생성된 슬롯을 리스트에 추가
            shopSlots.Add(slot);
        }
    }

    void SellItems()
    {
        // 기존의 playerShopItems 슬롯을 모두 비활성화
        if (playerShopItems.Count > 0)
        {
            foreach (ShopSlot _slot in playerShopItems)
            {
                _slot.gameObject.SetActive(false);
            }
        }

        // 인벤토리의 모든 아이템을 가져와 슬롯에 추가
        for (int i = 0; i < Inventory.Single.items.Count; i++)
        {
            Item _item = ItemResources.instance.itemRS[Inventory.Single.items[i].itemCode];
            _item.itemStack = Inventory.Single.items[i].itemStack;

            // 비활성화된 슬롯을 재사용하거나, 새로운 슬롯을 생성
            ShopSlot sellSlot = PoolShopSlot(playerShopItems);
            if (sellSlot == null)
            {
                sellSlot = Instantiate(slotPrefab, tfSell);
            }
            // 슬롯 초기화
            sellSlot.Init(_item, ShopState.SELL);

            playerShopItems.Add(sellSlot);
        }
    }

    ShopSlot PoolShopSlot(List<ShopSlot> _s)
    {
        foreach (ShopSlot slot in _s)
        {
            if (!slot.gameObject.activeSelf)
            {
                slot.gameObject.SetActive(true);
                return slot;
            }
        }
        return null;
    }

    void OpenTap(ShopState _state)// isOpen == true '판매'탭 활성, isOpen == false '구매'탭 활성
    {
        if (_state == ShopState.BUY)
        {
            buyTab.interactable = false;
            sellTab.interactable = true;

            //TODO: Buy ImageObejct Active, Sell ImageObject unActive
            buyPannel.SetActive(true);
            sellPannel.SetActive(false);
        }
        else if (_state == ShopState.SELL)
        {
            buyTab.interactable = true;
            sellTab.interactable = false;

            buyPannel.SetActive(false);
            sellPannel.SetActive(true);

            SellItems();
        }
    }

    public List<ShopSlot> GetShopSlots()
    {
        return shopSlots;
    }
    public void SetShopSlots(ShopSlot _shopSlot)
    {
        shopSlots.Add(_shopSlot);
    }

    // Tab Open
    public void ClickBuyTab() { OpenTap(ShopState.BUY); }
    public void ClickSellTab() { OpenTap(ShopState.SELL); }
}
