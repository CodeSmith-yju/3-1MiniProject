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
    public TextMeshProUGUI myGoldText;

    public int basketsPrice;
    //[SerializeField] Button btnBuy;// TODO: Add SoundEv 

    private void Awake()
    {
        shopSlots = new();
    }
    private void Start()
    {
        if (baskets == null)
            baskets = new List<BasketBox>();

        shopState = ShopState.BUY;
        OpenTap(shopState);
        //RefreshShopItems();
    }
    private int[] numbers = { 0, 6, 8, 9, 10, 11 };

    // 랜덤 숫자 출력 함수
    public int GetRandomNumber()
    {
        // Random.Range는 0부터 numbers.Length - 1까지의 인덱스를 반환
        int randomIndex = UnityEngine.Random.Range(0, numbers.Length);
        return numbers[randomIndex];
    }
    public void RefreshShopItems()
    {
        //Debug.Log("Make Shop Items");
        //이 아래의 for문 생성로직은 GameUIMgr로 빼내야할거같음 여기서관리하기엔조금? 연계된 기능이많은듯
        for (int i = 0; i < 4; i++)
        {
            // 원본 아이템을 선택
            int randomIndex = GetRandomNumber();
            if (i == 0)
                randomIndex = 0;

            Item originalItem = ItemResources.instance.itemRS[randomIndex];
            

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
                modifyStack = originalItem.modifyStack,
                typeIcon = originalItem.typeIcon,
            };

            if (newItem.itemType == Item.ItemType.Consumables)
            {
                newItem.itemStack = Random.Range(1, 10);
            }/*
            else
            {
                newItem.modifyStack = Random.Range(1, 4);
                newItem.UpgradeModifyPowerSet(newItem);
            }*/

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
    public void ReLoadShopItems(List<Item> _loadShopitem)//최종수정
    {
        if (_loadShopitem == null || _loadShopitem.Count == 0)
        {
            Debug.LogWarning("No shop items to load.");
            return;
        }

        // 상점 슬롯 초기화
        if (shopSlots.Count > 0)
        {
            foreach (var slot in shopSlots)
            {
                Destroy(slot.gameObject);
            }
            shopSlots.Clear();
        }

        Debug.Log("Run ReLoad shopItem");

        for (int i = 0; i < _loadShopitem.Count; i++)
        {
            DBConnector.LoadItemByCodeFromDB(_loadShopitem[i].itemCode, ref _loadShopitem[i].itemImage, ref _loadShopitem[i].typeIcon);
            ShopSlot slot = Instantiate(slotPrefab, tfBuy);
            slot.ShopMgrSet(this);
            slot.Init(_loadShopitem[i], ShopState.BUY);
            slot.shopIndex = i;
            shopSlots.Add(slot);

            Debug.Log($"Shop Item Loaded: {_loadShopitem[i].itemName}");
        }
    }
    /*public void ReLoadShopItems(List<Item> _loadShopitem)
    {
        // 예외 처리 추가
        if (_loadShopitem == null || _loadShopitem.Count == 0)
        {
            Debug.LogWarning("No shop items to load.");
            return;
        }

        if (shopSlots.Count > 0)
        {
            Debug.Log("Existing shop slots found.");
            // 여기에서 shopSlots을 초기화하거나 필요하다면 처리할 수 있음.
        }

        Debug.Log("Run ReLoad shopItem");

        for (int i = 0; i < _loadShopitem.Count; i++)
        {
            ShopSlot slot = Instantiate(slotPrefab, tfBuy);
            Debug.Log("슬롯 프리펩을 생성하여 배치");

            // 생성된 슬롯 초기화
            slot.ShopMgrSet(this);
            slot.Init(_loadShopitem[i], ShopState.BUY);
            slot.shopIndex = i;

            // 생성된 슬롯을 리스트에 추가
            shopSlots.Add(slot);
        }
    }*/

    /*public void ReLoadShopItems(List<Item> _loadShopitem)
    {
        if (shopSlots.Count > 0)
            Debug.Log("아니 만들어둔게 남아있다고?");
        Debug.Log("Run ReLoad shopItem");
        for (int i = 0; i < _loadShopitem.Count; i++)
        {
            ShopSlot slot = Instantiate(slotPrefab, tfBuy);
            Debug.Log("슬롯 프리펩을 생성하여 배치");
            // 생성된 슬롯 초기화
            slot.ShopMgrSet(this);
            slot.Init(_loadShopitem[i], ShopState.BUY);
            slot.shopIndex = i;

            // 생성된 슬롯을 리스트에 추가
            shopSlots.Add(slot);

        }
    }*/
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
                itemDesc = originalItem.itemDesc,
                itemPower = Inventory.Single.items[i].itemPower,
                itemPrice = Inventory.Single.items[i].itemPrice,
                itemStack = Inventory.Single.items[i].itemStack,
                modifyStack = Inventory.Single.items[i].modifyStack,
                PrimaryCode = Inventory.Single.items[i].PrimaryCode,
                typeIcon = originalItem.typeIcon,
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

    public void OpenTap(ShopState _state)// isOpen == true '판매'탭 활성, isOpen == false '구매'탭 활성
    {
        shopState = _state;

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
        GoldSet();
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

        if (shopState == ShopState.BUY)
        {
            if (basketsPrice <= GameMgr.playerData[0].player_Gold)
            {
                PayText.GetComponentInParent<Button>().interactable = true;
            }
            else
            {
                PayText.GetComponentInParent<Button>().interactable = false;
            }
        }
        
    }

    // Tab Open
    public void ClickBuyTab() { OpenTap(ShopState.BUY); }
    public void ClickSellTab() { OpenTap(ShopState.SELL); }

    public void OnClickPayBtn()
    {
        if (shopState == ShopState.BUY)// 구매
        {
            if (basketsPrice <= GameMgr.playerData[0].player_Gold)//basketsPrice <= GameMgr.playerData[0].player_Gold
            {
                //TODO: baskets 아이템들이 Inventory로 들어가고, 해당 shopSlots[i].soldOut.Active(true), shopSlots.Clear();
                //shopSlots[baskets[i].BasketShopIndex()].GetItem().itemType == Item.ItemType.Consumables || shopSlots[baskets[i].BasketShopIndex()].GetItem().itemType == Item.ItemType.Ect
                for (int i = 0; i < baskets.Count; i++)
                {
                    if (baskets[i].stack == 0 || !baskets[i].gameObject.activeSelf)
                    {
                        continue;
                    }

                    if (baskets[i].GetBasketShopSlot().GetItem().itemType == Item.ItemType.Consumables || baskets[i].GetBasketShopSlot().GetItem().itemType == Item.ItemType.Ect)
                    {
                        for (int j = 0; j < baskets[i].stack; j++)
                        {
                            //Item item = ItemResources.instance.itemRS[baskets[i].GetBasketShopSlot().GetItem().itemCode];
                            Item item = baskets[i].GetBasketShopSlot().GetItem();
                            Inventory.Single.AddItem(item);
                        }
                    }
                    else
                    {
                        Inventory.Single.AddItem(baskets[i].GetBasketShopSlot().GetItem());
                    }
                    baskets[i].GetBasketShopSlot().SoldNow(baskets[i].stack); // 구매된만큼의 stack 차감
                }
                //구매가 성공적으로 종료된 상황.
                GameMgr.playerData[0].player_Gold -= basketsPrice;
                RefeshBaskets();
            }
            else
            {
                //TODO: 구매실패 or 그냥 장바구니금액 > 보유골드 이면 버튼자체가 활성화 안되게 해야할듯.
                Debug.Log("구매 실패");
            }
        }
        else if (shopState == ShopState.SELL)// 판매
        {
            List<Item> RemoveItems = new();
            for (int i = 0; i < baskets.Count; i++)
            {
                if (baskets[i].stack == 0 || !baskets[i].gameObject.activeSelf)
                {
                    continue;
                }

                if (baskets[i].stack == playerShopItems[baskets[i].BasketShopIndex()].GetItem().itemStack)// && baskets[i].stack > 1)
                {
                    //인벤토리의 아이템이 제거되어야 하는 상황
                    foreach (var _item in Inventory.Single.items)
                    {
                        if (_item.PrimaryCode == playerShopItems[baskets[i].BasketShopIndex()].GetItem().PrimaryCode)
                        {
                            baskets[i].GetBasketShopSlot().gameObject.SetActive(false);
                            //Inventory.Single.RemoveItem(_item);
                            RemoveItems.Add(_item);
                            /*for (int j = 0; j < baskets[i].GetBasketShopSlot().GetItem().itemStack; j++)
                            {
                                RemoveItems.Add(_item);
                            }*/
                            Debug.Log("Remove Item :"+_item.itemName);
                            continue;
                        }
                        Debug.Log("not Remove Item :" + _item.itemName);
                    }
                }
                else
                {
                    //인벤토리의 아이템의 stack이 변경되는 상황

                    foreach (var _item in Inventory.Single.items)
                    {
                        if (_item == playerShopItems[baskets[i].BasketShopIndex()].GetItem())
                        {
                            _item.itemStack -= baskets[i].stack;
                            return;
                        }
                    }
                    baskets[i].GetBasketShopSlot().SoldNow(baskets[i].stack); // 구매된만큼의 stack 차감
                }//이새끼있는데까지 아마 안올거임
            }
            
            for (int i = 0; i < RemoveItems.Count; i++)
            {
                int _stack = 0;
                if (RemoveItems[i].itemStack > 1)
                {
                    _stack = RemoveItems[i].itemStack;
                    for (int j = 0; j < _stack; j++)
                    {
                        Inventory.Single.RemoveItem(RemoveItems[i]);
                    }
                }
                else
                    Inventory.Single.RemoveItem(RemoveItems[i]);
            }
            RemoveItems.Clear();
            //판매가 성공적으로 종료된 상황.
            GameMgr.playerData[0].player_Gold += basketsPrice;
            RefeshBaskets();
        }

        GameUiMgr.single.RedrawSlotUI();
        //거래가 종료되면 결과가 반영되어야 함.
        Debug.Log("거래 종료 후 골드: " + GameMgr.playerData[0].player_Gold);
        GameUiMgr.single.SliderChange();
        GoldSet();
        GameUiMgr.single.GoldChanger();
    }

    void RefeshBaskets()
    {
        basketsPrice = 0;
        for (int i = 0; i < baskets.Count; i++)
        {
            baskets[i].gameObject.SetActive(false);
            baskets[i].GetBasketShopSlot().UseImgSet(false);
            calcPrice.text = basketsPrice.ToString();
        }
    }
    void GoldSet()
    {
        myGoldText.text = GameMgr.playerData[0].player_Gold.ToString();
    }
}
