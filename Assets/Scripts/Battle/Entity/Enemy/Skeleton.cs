using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    protected override void Start()
    {
        base.Start();
        // �ִ� ü��, �ִ� ����, ���ݷ�, ���ݼӵ�, ��Ÿ�, ��������, ��ų����
        InitStat(50, 0, 5, 1, 1.6f, true, false);
    }

    /*    protected override void Update()
        {
            base.Update();
            // ��ų ������ �߰�
        }

        public void Skill()
        {

        }*/
}
