using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Purple_Slime : Enemy
{
    protected override void Start()
    {
        base.Start();
        // �ִ� ü��, �ִ� ����, ����, ���ݷ�, ���ݼӵ�, ��Ÿ�, ��������, ��ų����, ������, ���, ������ ���
        InitStat(25, 0, 5, 2.5f, 1, 1.2f, true, false, GetExp(3.5f), SetRandomGold(50), new Item().GenerateRandomItem(16));

        if (BattleManager.Instance.dialogue != null && BattleManager.Instance.dialogue.isTutorial)
            item_Drop_Check = false;
        else
            item_Drop_Check = ShouldDropItem(50);

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
