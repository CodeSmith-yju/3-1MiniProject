using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class Blacksmith : MonoBehaviour
{
    [SerializeField] private InSlot Prefab;

    [SerializeField] private List<InSlot> renovateItems;
    [SerializeField] private List<InSlot> invenItems;

    [SerializeField] private Transform renovatetr;
    [SerializeField] private Transform inventr;
    bool samenessCk;

    private void Start()
    {
        //�����ʽ���� �ѹ�
        MakeRenovateItems();
        //OpenBlacksmith();
    }
    void OpenBlacksmith()
    {
        samenessCk = false;
        // TODO: invenList Add. (foreach Inventory.items[i].PK != BlackSmithSlot.item.PK) ������ ����invenList�ͺ��Ͽ� Destroy or As it is 

        // ���ʽ��� = ����Ʈ�� ���ٸ� �ʱ�ȭ == invenItems ??= new List<InSlot>();
        if (invenItems == null)
        {
            invenItems = new List<InSlot>();
        }

        // ������ ����Ʈ�� �����Ѵٸ�.
        if (invenItems.Count > 0)
        {
            //������ ����Ʈ�� ���� �������� ����Ʈ�� �������� Ȯ��
            if (Inventory.Single.items.Count == invenItems.Count)
            {
                // �ϴ� �����ϴٰ� �����ϰ� ����
                samenessCk = true;

                for (int i = 0; i < invenItems.Count; i++)
                {
                    if (invenItems[i].GetItem().PrimaryCode != Inventory.Single.items[i].PrimaryCode)
                    {
                        // �ٸ� �������� �߰ߵǸ� false�� ����
                        samenessCk = false; 
                        break; // ���̰� �߰ߵǸ� �� �̻� ���� �ʿ䰡 ����
                    }
                }

                if (!samenessCk)
                {
                    // ���� �������� ��� �����ϰ� ����Ʈ �ʱ�ȭ
                    Refresh();
                }
            }
            else
            {
                // ������ ������ �ٸ��� ������ ���� ������ ��
                Refresh();
            }
        }
        //���� ����� �ڵ�
        if (!samenessCk)
        {
            MakeInvenItems();
        }
        // TODO: revnovateItems
    }

    void Refresh()
    {
        int cnt = invenItems.Count;
        for (int i = 0; i < cnt; i++)
        {
            Destroy(invenItems[i]);
        }
        invenItems.Clear();
    }

    void MakeInvenItems()
    {
        for (int i = 0; i < Inventory.Single.items.Count; i++)
        {
            //�ϴ� ��� ��µǰ� �ߴµ� ���������Ѿ������� �ڵ常�ȴٴ��� �ϴ½����� ���� �ʿ�
            //if (Inventory.Single.items[i].itemType != Item.ItemType.Consumables && Inventory.Single.items[i].itemType != Item.ItemType.Ect)
            if (Inventory.Single.items[i].itemCode > 7 && Inventory.Single.items[i].itemCode < 12)
            {
                InSlot slot = Instantiate(Prefab, inventr);

                // ������ ���� �ʱ�ȭ
                slot.Init(this, Inventory.Single.items[i], true);

                // ������ ������ ����Ʈ�� �߰�
                invenItems.Add(slot);
            }
        }
    }

    void MakeRenovateItems()
    {
        for (int i = 8; i < 12; i++)
        {
            InSlot slot = Instantiate(Prefab, renovatetr);

            // ������ ���� �ʱ�ȭ
            slot.Init(this, ItemResources.instance.itemRS[i + 4], false);

            // ������ ������ ����Ʈ�� �߰�
            invenItems.Add(slot);
        }
    }

    //TODO: LeftTop Side one = Renovate
    //TODO: LeftTop Side two = Enhance
    //TODO: Click(OneTab || Two Tab)
    //TODO: LeftBottom Side View List

}
