using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyStatInit : MonoBehaviour
{
    public GameObject party_Stat_Prefab;

    private void Awake()
    {
        for (int i = 0; i < BattleManager.Instance.party_List.Count; i++)
        {
            GameObject obj = Instantiate(party_Stat_Prefab, transform);
            StatManager stat_Obj = obj.GetComponent<StatManager>();

            PlayerData data = GameMgr.playerData[i];

            Ally ally = BattleManager.Instance.party_List[i].GetComponent<Ally>();

            stat_Obj.InitStat(data, data.player_level, data.GetPlayerName());
        }
    }
}
