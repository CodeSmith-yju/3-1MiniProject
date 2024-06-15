using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Enemy
{
    protected override void Start()
    {
        base.Start();
        // �ִ� ü��, �ִ� ����, ���ݷ�, ���ݼӵ�, ��Ÿ�, ��������, ��ų����
        InitStat(15, 0, 1, 0.8f, 6f, false, false);

        exp_Cnt = 3;

        Debug.Log("����ġ ���� : " +  exp_Cnt);
    }


    public override void RangeAttack(BaseEntity target)
    {
        base.RangeAttack(target);
        GameObject obj_Arrow = BattleManager.Instance.pool.GetObject(1); // �� ������
        obj_Arrow.transform.position = transform.GetChild(0).position;
        Arrow arrow = obj_Arrow.GetComponent<Arrow>();
        arrow.Shoot(this, target);
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
