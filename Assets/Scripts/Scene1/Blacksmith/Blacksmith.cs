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

    public void OpenBlacksmith()
    {
        samenessCk = false;
        // TODO: invenList Add. (foreach Inventory.items[i].PK != BlackSmithSlot.item.PK) ������ ����invenList�ͺ��Ͽ� Destroy or As it is 

        // ���ʽ��� = ����Ʈ�� ���ٸ� �ʱ�ȭ
        invenItems ??= new List<InSlot>();
        
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

    private void MakeInvenItems()
    {
        for (int i = 0; i < Inventory.Single.items.Count; i++)
        {
            InSlot slot = Instantiate(Prefab, inventr);

            // ������ ���� �ʱ�ȭ
            slot.Init(Inventory.Single.items[i]);

            // ������ ������ ����Ʈ�� �߰�
            invenItems.Add(slot);
        }
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
}
