using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mimic : Enemy
{
    protected override void Start()
    {
        base.Start();
        // 최대 체력, 최대 마나, 공격력, 공격속도, 사거리, 근접유무, 스킬유무
        InitStat(45, 0, 3, 0.75f, 1.5f, true, false, 5, 175);
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
