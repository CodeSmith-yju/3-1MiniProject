using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Purple_Slime : Enemy
{
    protected override void Start()
    {
        base.Start();
        // 최대 체력, 최대 마나, 방어력, 공격력, 공격속도, 사거리, 근접유무, 스킬유무, 경험차, 골드, 아이템 드랍
        InitStat(25, 0, 5, 2.5f, 1, 1.2f, true, false, GetExp(3.5f), SetRandomGold(50), new Item().GenerateRandomItem(16), false);

        if (BattleManager.Instance.dialogue != null && BattleManager.Instance.dialogue.isTutorial)
            item_Drop_Check = false;
        else
            item_Drop_Check = ShouldDropItem(50);

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
