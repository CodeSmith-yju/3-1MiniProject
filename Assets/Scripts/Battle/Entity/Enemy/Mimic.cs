using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mimic : Enemy
{
    protected override void Start()
    {
        base.Start();
        // �ִ� ü��, �ִ� ����, ����, ���ݷ�, ���ݼӵ�, ��Ÿ�, ��������, ��ų����, ����ġ, ���, �����۵��
        InitStat(80, 0, 8, 4, 0.75f, 1.2f, true, false, GetExp(5), SetRandomGold(150), new Item().GenerateRandomItem(18));

        if (BattleManager.Instance.dialogue != null && BattleManager.Instance.dialogue.isTutorial)
            item_Drop_Check = false;
        else
            item_Drop_Check = ShouldDropItem(20);

        Debug.Log("����ġ ���� : " + exp_Cnt);
        Debug.Log("��� ���� : " + gold_Cnt);
        Debug.Log("������ ��� ���� ���� : " + item_Drop_Check);

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
