using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyListInit : MonoBehaviour
{
    public GameObject party_Prefab;
    private List<GameObject> party = new List<GameObject>();

/*    private void Start()
    {
        if (BattleManager.Instance._curphase == BattleManager.BattlePhase.Deploy)
        {
            SpawnPartyList();
        }
    }*/

    private void OnEnable()
    {
         DestroyPartyList();
         SpawnPartyList();
    }


    private void SpawnPartyList()
    {
        for (int i = 0; i < BattleManager.Instance.party_List.Count; i++)
        {
            GameObject obj = Instantiate(party_Prefab, transform);
            UnitPlacement unit = obj.GetComponent<UnitPlacement>();

            unit.InitList(BattleManager.Instance.party_List[i], BattleManager.Instance.party_List[i].GetComponent<SpriteRenderer>().sprite);

            party.Add(obj);
        }
        
        /*for (int i = 0; i < GameUiMgr.single.poolMoveInSlot.Count; i++)
        {
            if (GameUiMgr.single.poolMoveInSlot[i].partyData != null)
            {
                GameObject obj = Instantiate(party_Prefab, transform);
                UnitPlacement unit = obj.GetComponent<UnitPlacement>();

                unit.InitList(GameUiMgr.single.poolMoveInSlot[i].partyData.obj_Data , GameUiMgr.single.poolMoveInSlot[i].partyData.obj_Data.GetComponent<SpriteRenderer>().sprite);

                party.Add(obj);
            }
        }*/
    }

    private void DestroyPartyList()
    {
        foreach (GameObject obj in party)
        {
            if (obj != null)
            {
                Destroy(obj); // ������ ������ Ŭ�� ����
            }
            else
            {
                return;
            }
        }
        party.Clear();
    }
}
