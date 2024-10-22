using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puppet_Human : Enemy
{
    protected override void Start()
    {
        base.Start();
        // �ִ� ü��, �ִ� ����, ���ݷ�, ���ݼӵ�, ��Ÿ�, ��������, ��ų����, ������, ���, ������ ���
        InitStat(50, 0, 3.5f, 1, 1.2f, true, false, GetExp(5), SetRandomGold(100), new Item().GenerateRandomItem(6));

        if (BattleManager.Instance.dialogue != null && BattleManager.Instance.dialogue.isTutorial)
            item_Drop_Check = false;
        else
            item_Drop_Check = ShouldDropItem(30);

        Debug.Log("����ġ ���� : " + exp_Cnt);
        Debug.Log("��� ���� : " + gold_Cnt);
        Debug.Log("������ ��� ���� ���� : " + item_Drop_Check);

    }
}
