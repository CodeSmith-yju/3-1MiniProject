using DarkPixelRPGUI.Scripts.UI.Equipment;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("장바구니")]
    public Transform trBasketBox;
    public BasketBox prBasketBox;

    public List<BasketBox> baskets;

    public TextMeshProUGUI calcPrice;
    public TextMeshProUGUI PayText;

    public int basketsPrice;
    //[SerializeField] Button btnBuy;// TODO: Add SoundEv 

    private void Start()
    {
        shopState = ShopState.BUY;
        OpenTap(shopState);
        RefreshShopItems();
    }

    public void RefreshShopItems()
    {
        Debug.Log("Make Shop Items");
        //이 아래의 for문 생성로직은 GameUIMgr로 빼내야할거같음 여기서관리하기엔조금? 연계된 기능이많은듯
        for (int i = 0; i < 12; i++)
        {
            // 원본 아이템을 선택
            Item originalItem = ItemResources.instance.itemRS[Random.Range(1, 3)];

            // 새로운 아이템 인스턴스를 생성
            Item newItem = new Item
            {
                itemCode = originalItem.itemCode,
                itemName = originalItem.itemName,
                itemType = originalItem.itemType,
                itemImage = originalItem.itemImage,
                itemPrice = originalItem.itemPrice,
                itemPower = originalItem.itemPower,
                itemDesc = originalItem.itemDesc,
                itemStack = originalItem.itemStack,
                modifyStack = originalItem.modifyStack
            };

            if (newItem.itemType == Item.ItemType.Consumables)
            {
                newItem.itemStack = Random.Range(1, 10);
            }
            else
            {
                newItem.modifyStack = Random.Range(1, 4);
                newItem.ModifyPowerSet(newItem);
            }

            // 프리팹을 인스턴스화하여 ShopSlot을 생성
            ShopSlot slot = Instantiate(slotPrefab, tfBuy);

            // 생성된 슬롯 초기화
            slot.Init(newItem, ShopState.BUY);

            slot.shopIndex = i;
            slot.ShopMgrSet(this);
            // 생성된 슬롯을 리스트에 추가
            shopSlots.Add(slot);
        }
    }

    void SetSellItems()
    {
        // 기존의 playerShopItems 슬롯을 모두 비활성화
        if (playerShopItems.Count > 0)
        {
            for (int i = 0; i < playerShopItems.Count; i++)
            {
                playerShopItems[i].gameObject.SetActive(false);
            }
        }

        List<ShopSlot> inactiveSlots = new List<ShopSlot>(playerShopItems);
        playerShopItems.Clear();

        // 인벤토리의 모든 아이템을 가져와 슬롯에 추가
        for (int i = 0; i < Inventory.Single.items.Count; i++)
        {
            Item originalItem = ItemResources.instance.itemRS[Inventory.Single.items[i].itemCode];

            Item _item = new Item
            {
                itemCode = originalItem.itemCode,
                itemName = originalItem.itemName,
                itemType = originalItem.itemType,
                itemImage = originalItem.itemImage,
                itemPrice = originalItem.itemPrice,
                itemPower = originalItem.itemPower,
                itemDesc = originalItem.itemDesc,
                itemStack = Inventory.Single.items[i].itemStack,
                modifyStack = Inventory.Single.items[i].modifyStack
            };

            // 비활성화된 슬롯을 재사용하거나, 새로운 슬롯을 생성
            ShopSlot sellSlot = PoolShopSlot(inactiveSlots);
            if (sellSlot == null)
            {
                sellSlot = Instantiate(slotPrefab, tfSell);
            }
            // 슬롯 초기화
            sellSlot.Init(_item, ShopState.SELL);
            sellSlot.shopIndex = i;
            sellSlot.ShopMgrSet(this);

            playerShopItems.Add(sellSlot);
        }
        inactiveSlots.Clear();
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
        basketsPrice = 0;
        for (int i = 0; i < baskets.Count; i++)
        {
            baskets[i].gameObject.SetActive(false);
        }

        if (_state == ShopState.BUY)
        {
            buyTab.interactable = false;
            sellTab.interactable = true;

            //TODO: Buy ImageObejct Active, Sell ImageObject unActive
            buyPannel.SetActive(true);
            sellPannel.SetActive(false);

            PayText.text = "구매";
            calcPrice.text = basketsPrice.ToString();
        }
        else if (_state == ShopState.SELL)
        {
            buyTab.interactable = true;
            sellTab.interactable = false;

            buyPannel.SetActive(false);
            sellPannel.SetActive(true);

            SetSellItems();

            PayText.text = "판매";
            calcPrice.text = basketsPrice.ToString();

        }

        /*if (baskets.Count > 0)
        {
            for (int i = 0; i < baskets.Count; i++)
            {
                if (_state == ShopState.BUY)
                {
                    shopSlots[baskets[i].BasketShopIndex()].UseImgSet(false);
                }
                else
                {
                    playerShopItems[baskets[i].BasketShopIndex()].UseImgSet(false);
                }
            }
        }*/
        if (baskets.Count > 0)
        {
            for (int i = 0; i < baskets.Count; i++)
            {
                int basketIndex = baskets[i].BasketShopIndex();
                if (_state == ShopState.BUY && basketIndex < shopSlots.Count)
                {
                    shopSlots[basketIndex].UseImgSet(false);
                }
                else if (_state == ShopState.SELL && basketIndex < playerShopItems.Count)
                {
                    playerShopItems[basketIndex].UseImgSet(false);
                }
            }
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

    public void CalcPrice(int _price)
    {
        basketsPrice += _price;
        calcPrice.text = basketsPrice.ToString();
    }

    // Tab Open
    public void ClickBuyTab() { OpenTap(ShopState.BUY); }
    public void ClickSellTab() { OpenTap(ShopState.SELL); }

    public void OnClickPayBtn()
    {
        if (shopState == ShopState.BUY)// 구매
        {
            if (basketsPrice <= 1500)//basketsPrice <= GameMgr.playerData[0].player_Gold
            {
                //TODO: baskets 아이템들이 Inventory로 들어가고, 해당 shopSlots[i].soldOut.Active(true), shopSlots.Clear();
                //shopSlots[baskets[i].BasketShopIndex()].GetItem().itemType == Item.ItemType.Consumables || shopSlots[baskets[i].BasketShopIndex()].GetItem().itemType == Item.ItemType.Ect
                for (int i = 0; i < baskets.Count; i++)
                {
                    if (baskets[i].stack == 0)
                    {
                        continue;
                    }

                    //구매한 아이템의 구매가능갯수만큼 차감, 차감 후 개수가 0일경우 SoldOut
                    int lastStack = shopSlots[baskets[i].BasketShopIndex()].GetItem().itemStack - baskets[i].stack;
                    shopSlots[baskets[i].BasketShopIndex()].GetItem().itemStack = baskets[i].stack;

                    Inventory.Single.AddItem(shopSlots[baskets[i].BasketShopIndex()].GetItem());//추가
                    if (lastStack == 0)
                    {
                        shopSlots[baskets[i].BasketShopIndex()].SoldOut(true);
                    }
                    else
                    {
                        shopSlots[baskets[i].BasketShopIndex()].GetItem().itemStack = lastStack;
                    }
                }
                //정렬
                Inventory.Single.SortingStackItems();
            }
            else
            {
                //TODO: 구매실패 or 그냥 장바구니금액 > 보유골드 이면 버튼자체가 활성화 안되게 해야할듯.
                Debug.Log("구매 실패");
            }
        }
        else if (shopState == ShopState.SELL)// 판매
        {
            // 
        }
    }
}
