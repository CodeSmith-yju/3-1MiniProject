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
        // TODO: invenList Add. (foreach Inventory.items[i].PK != BlackSmithSlot.item.PK) 문으로 기존invenList와비교하여 Destroy or As it is 

        // 최초실행 = 리스트가 없다면 초기화
        invenItems ??= new List<InSlot>();
        
        // 기존의 리스트가 존재한다면.
        if (invenItems.Count > 0)
        {
            //기존의 리스트와 새로 만들어야할 리스트가 동일한지 확인
            if (Inventory.Single.items.Count == invenItems.Count)
            {
                // 일단 동일하다고 가정하고 시작
                samenessCk = true;

                for (int i = 0; i < invenItems.Count; i++)
                {
                    if (invenItems[i].GetItem().PrimaryCode != Inventory.Single.items[i].PrimaryCode)
                    {
                        // 다른 아이템이 발견되면 false로 설정
                        samenessCk = false; 
                        break; // 차이가 발견되면 더 이상 비교할 필요가 없음
                    }
                }

                if (!samenessCk)
                {
                    // 기존 아이템을 모두 삭제하고 리스트 초기화
                    Refresh();
                }
            }
            else
            {
                // 아이템 개수가 다르면 무조건 새로 만들어야 함
                Refresh();
            }
        }
        //새로 만드는 코드
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

            // 생성된 슬롯 초기화
            slot.Init(Inventory.Single.items[i]);

            // 생성된 슬롯을 리스트에 추가
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
