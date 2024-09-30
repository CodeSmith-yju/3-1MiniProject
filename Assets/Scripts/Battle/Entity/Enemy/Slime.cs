using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    protected override void Start()
    {
        base.Start();
        // 최대 체력, 최대 마나, 공격력, 공격속도, 사거리, 근접유무, 스킬유무, 경험차, 골드, 아이템 드랍
        InitStat(20, 0, 1, 1, 1f, true, false, 1, SetRandomGold(30), ItemResources.instance.itemRS[2]);

        if (BattleManager.Instance.dialogue != null && BattleManager.Instance.dialogue.isTutorial)
            item_Drop_Check = false;
        else
            item_Drop_Check = ShouldDropItem(40);

        Debug.Log("경험치 설정 : " + exp_Cnt);
        Debug.Log("골드 설정 : " + gold_Cnt);
        Debug.Log("아이템 드랍 유무 설정 : " + item_Drop_Check);

    }

/*    protected override void Update()
    {
        base.Update();
        // 스킬 있으면 추가
    }

    public void Skill()
    {

    }*/
}
