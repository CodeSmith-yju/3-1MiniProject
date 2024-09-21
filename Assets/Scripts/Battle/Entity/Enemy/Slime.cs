using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    protected override void Start()
    {
        base.Start();
        // 최대 체력, 최대 마나, 공격력, 공격속도, 사거리, 근접유무, 스킬유무
        InitStat(12, 0, 1, 1, 1f, true, false, 1, 50);
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
