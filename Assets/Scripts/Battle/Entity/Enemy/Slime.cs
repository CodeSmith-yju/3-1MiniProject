using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    protected override void Start()
    {
        base.Start();
        // �ִ� ü��, �ִ� ����, ���ݷ�, ���ݼӵ�, ��Ÿ�, ��������, ��ų����, ������, ���, ������ ���
        InitStat(20, 0, 1, 1, 1f, true, false, 1, SetRandomGold(30), ItemResources.instance.itemRS[2]);

        if (BattleManager.Instance.dialogue != null && BattleManager.Instance.dialogue.isTutorial)
            item_Drop_Check = false;
        else
            item_Drop_Check = ShouldDropItem(40);

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
