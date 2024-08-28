using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public delegate void OnSlotCountChange(int val);//delegate Definition
    public OnSlotCountChange onSlotCountChange;// Definition make is Instance

    public delegate void OnChangeItem();
    public OnChangeItem onChangeItem;

    public List<Item> items = new();

    private int slotCnt;
    public int SlotCnt
    {
        get => slotCnt;
        set
        {
            slotCnt = value;
            onSlotCountChange.Invoke(slotCnt);
        }
    }

    #region singletone
    private static Inventory single;
    public static Inventory Single
    {
        get
        {
            if (single == null)
            {
                single = FindObjectOfType<Inventory>();

                if (single == null)
                {
                    Debug.Log("#Make A New Inventory");
                    var instanceContainer = new GameObject("Inventory");
                    single = instanceContainer.AddComponent<Inventory>();
                    DontDestroyOnLoad(instanceContainer);
                }
            }
            return single;
        }
    }
    private void Awake()
    {
        // Ensure there's only one instance of Inventory
        if (single == null)
        {
            single = this;
            DontDestroyOnLoad(gameObject);
        }
        /*else if (single != this)
        {
            Debug.Log("#Destroy Inventory");
            Destroy(gameObject.GetComponentInChildren<Inventory>());
        }*/
    }

    #endregion

    private void Start()
    {
        slotCnt = 35;
    }

    //method
    public bool AddItem(Item _item)
    {
        if (items.Count < slotCnt)//소유하고있는 아이템들의 개수가, 인벤토리 최대슬롯보다 작을때
        {
            // TODO: _item의 Tpye이 Consumer || Ect 일때, 뒤로 돌아가는 for문을 돌려서 현재 가진 가장아랫부분의, 동일한 itemCode인 item의 stack을 확인하여 stack이 9미만이면 stack ++ 9라면 add _item 하는 코드
            // Item의 중첩가능 개수를 위함, if(_item.itemCode == items[i].itemCode)문 안에 swtchi 문을 넣어서 아이탬 코드별로 중첩가능한 개수를 다르게 줄 수도 있을듯.
            // ++ AddItem 메서드 사용하는부분에서 해당 item의 강화수치와 스택 적용해줘야함// 그리고 최초의 소모품, ect가 생성될때면 여기에서 그냥 stack 1로 주면될듯
            if (_item.itemType == Item.ItemType.Consumables || _item.itemType == Item.ItemType.Ect)
            {
                for (int i = items.Count - 1; i > -1; i--)
                {
                    if (_item.itemCode == items[i].itemCode)
                    {
                        if (items[i].itemStack < 9)
                        {
                            items[i].itemStack++;

                            onChangeItem?.Invoke();// 대리자 호출 간소화, if (onChangeItem != null) {onChangeItem.Invoke();} <- 정상적으로 쓰면 이런 코드임
                            return true;
                        }
                    }
                }
            }
            Item newItem = new Item
            {
                itemType = _item.itemType,
                itemStack = 1,
                modifyStack = _item.modifyStack,
                itemName = _item.itemName,
                itemImage = _item.itemImage,
                //efts = new List<ItemEffect>(_item.efts),
                efts = _item.efts != null ? new List<ItemEffect>(_item.efts) : new List<ItemEffect>(),  // null 체크 추가
                itemTitle = _item.itemTitle,
                itemDesc = _item.itemDesc,
                itemIndex = items.Count,
                isDraggable = _item.isDraggable,
                itemCode = _item.itemCode,
                itemPower = _item.itemPower
            };

            // 추가될 아이템이 소모품이 아닐 경우.
            items.Add(newItem);
            Debug.Log(newItem.itemIndex);

            if (onChangeItem != null)
                onChangeItem.Invoke();

            return true;
        }
        return false;
    }

    public void RemoveItem(Item _item)
    {
        if (_item.itemIndex >= 0 && _item.itemIndex < items.Count)// 지우려는 아이템이 정상적으로 인벤토리에 추가된 아이템인지 확인 _item.Index >= 0 && _item.Index < items.Count 
        {
            //일단여기에 조건 넣어서 소모품일때, ect는 item의 stack을 확인하고 stack이 2이상이면 stack -- 후return, 그게아니면 stack -- 하고( 1 -> 0), remove 하도록 해보겠음
            if (_item.itemType == Item.ItemType.Consumables || _item.itemType == Item.ItemType.Ect)
            {
                if (_item.itemStack >= 1)
                {
                    _item.itemStack--;

                    if (_item.itemStack <= 0)
                    {
                        items.RemoveAt(_item.itemIndex);
                        //GameUiMgr.single.RedrawSlotUI();
                        //아이템이 제거될 때 남은 아이템들의 itemIndex 수정
                        for (int i = _item.itemIndex; i < items.Count; i++)
                        {
                            items[i].itemIndex = i;
                        }
                    }
                }
            }
            else
            {
                items.RemoveAt(_item.itemIndex);

                // 아이템이 제거될 때 남은 아이템들의 itemIndex 수정
                for (int i = 0; i < items.Count; i++)
                {
                    items[i].itemIndex = i;
                }

                onChangeItem?.Invoke();
            }
        }
        else
        {
            Debug.Log("Invalid index to remove item!");
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FieldItem"))
        {
            FieldItems fieldItem = collision.GetComponent<FieldItems>();
            if (AddItem(fieldItem.GetItem()))
            {
                fieldItem.DestroyItem();
            }
            else
            {
                Debug.Log("Inventory Is Full");
            }
        }
    }

    public void SortingStackItems()
    {
        List<Item> soltingNow = new();// 일단 정렬용 스왑 배열 하나 만들고

        //현재 인벤토리의 모든 아이템들을 체크해서 소모품아이템들 들어내고
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemType == Item.ItemType.Consumables || items[i].itemType == Item.ItemType.Ect)
            {
                Item originalItem = items[i];

                Item item = new Item
                {
                    itemCode = originalItem.itemCode,
                    itemName = originalItem.itemName,
                    itemType = originalItem.itemType,
                    itemImage = originalItem.itemImage,
                    itemPrice = originalItem.itemPrice,
                    itemPower = originalItem.itemPower,
                    itemDesc = originalItem.itemDesc,
                    itemStack = items[i].itemStack,
                    modifyStack = items[i].modifyStack
                };

                soltingNow.Add(item);
                items.Remove(item);
            }
        }

        //들어낸 아이템들 갯수만큼 for문돌리는데
        for (int i = 0; i < soltingNow.Count; i++)
        {
            //for문돌아가는동안 해당아이템의 stack만큼 소모품이면 Add해야하니까
            for (int j = 0; j < soltingNow[i].itemStack; j++)
            {
                //해당 소모품과 동일한 아이템을 생성해서 기존의 아이템과 분리좀 해 주고
                Item ni = ItemResources.instance.itemRS[soltingNow[i].itemCode];
                AddItem(ni);
            }
        }

    }
}
