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
        if (items.Count < slotCnt)//�����ϰ��ִ� �����۵��� ������, �κ��丮 �ִ뽽�Ժ��� ������
        {
            // TODO: _item�� Tpye�� Consumer || Ect �϶�, �ڷ� ���ư��� for���� ������ ���� ���� ����Ʒ��κ���, ������ itemCode�� item�� stack�� Ȯ���Ͽ� stack�� 9�̸��̸� stack ++ 9��� add _item �ϴ� �ڵ�
            // Item�� ��ø���� ������ ����, if(_item.itemCode == items[i].itemCode)�� �ȿ� swtchi ���� �־ ������ �ڵ庰�� ��ø������ ������ �ٸ��� �� ���� ������.
            // ++ AddItem �޼��� ����ϴºκп��� �ش� item�� ��ȭ��ġ�� ���� �����������// �׸��� ������ �Ҹ�ǰ, ect�� �����ɶ��� ���⿡�� �׳� stack 1�� �ָ�ɵ�
            if (_item.itemType == Item.ItemType.Consumables || _item.itemType == Item.ItemType.Ect)
            {
                for (int i = items.Count - 1; i > -1; i--)
                {
                    if (_item.itemCode == items[i].itemCode)
                    {
                        if (items[i].itemStack < 9)
                        {
                            items[i].itemStack++;

                            onChangeItem?.Invoke();// �븮�� ȣ�� ����ȭ, if (onChangeItem != null) {onChangeItem.Invoke();} <- ���������� ���� �̷� �ڵ���
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
                efts = _item.efts != null ? new List<ItemEffect>(_item.efts) : new List<ItemEffect>(),  // null üũ �߰�
                itemTitle = _item.itemTitle,
                itemDesc = _item.itemDesc,
                itemIndex = items.Count,
                isDraggable = _item.isDraggable,
                itemCode = _item.itemCode,
                itemPower = _item.itemPower
            };

            // �߰��� �������� �Ҹ�ǰ�� �ƴ� ���.
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
        if (_item.itemIndex >= 0 && _item.itemIndex < items.Count)// ������� �������� ���������� �κ��丮�� �߰��� ���������� Ȯ�� _item.Index >= 0 && _item.Index < items.Count 
        {
            //�ϴܿ��⿡ ���� �־ �Ҹ�ǰ�϶�, ect�� item�� stack�� Ȯ���ϰ� stack�� 2�̻��̸� stack -- ��return, �װԾƴϸ� stack -- �ϰ�( 1 -> 0), remove �ϵ��� �غ�����
            if (_item.itemType == Item.ItemType.Consumables || _item.itemType == Item.ItemType.Ect)
            {
                if (_item.itemStack >= 1)
                {
                    _item.itemStack--;

                    if (_item.itemStack <= 0)
                    {
                        items.RemoveAt(_item.itemIndex);
                        //GameUiMgr.single.RedrawSlotUI();
                        //�������� ���ŵ� �� ���� �����۵��� itemIndex ����
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

                // �������� ���ŵ� �� ���� �����۵��� itemIndex ����
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
        List<Item> soltingNow = new();// �ϴ� ���Ŀ� ���� �迭 �ϳ� �����

        //���� �κ��丮�� ��� �����۵��� üũ�ؼ� �Ҹ�ǰ�����۵� ����
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

        //�� �����۵� ������ŭ for�������µ�
        for (int i = 0; i < soltingNow.Count; i++)
        {
            //for�����ư��µ��� �ش�������� stack��ŭ �Ҹ�ǰ�̸� Add�ؾ��ϴϱ�
            for (int j = 0; j < soltingNow[i].itemStack; j++)
            {
                //�ش� �Ҹ�ǰ�� ������ �������� �����ؼ� ������ �����۰� �и��� �� �ְ�
                Item ni = ItemResources.instance.itemRS[soltingNow[i].itemCode];
                AddItem(ni);
            }
        }

    }
}
