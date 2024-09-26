using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    protected override void Start()
    {
        base.Start();
        // 최대 체력, 최대 마나, 공격력, 공격속도, 사거리, 근접유무, 스킬유무, 경험치, 골드, 아이템 드랍
        InitStat(80, 0, 5, 0.75f, 1.3f, true, false, 10, 250, ItemResources.instance.itemRS[4]);

        if (!BattleManager.Instance.dialogue.isTutorial)
            item_Drop_Check = ShouldDropItem(25);

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
