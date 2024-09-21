using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    protected override void Start()
    {
        base.Start();
        // 최대 체력, 최대 마나, 공격력, 공격속도, 사거리, 근접유무, 스킬유무
        InitStat(80, 0, 5, 0.75f, 1.3f, true, false, 10, 250);


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
