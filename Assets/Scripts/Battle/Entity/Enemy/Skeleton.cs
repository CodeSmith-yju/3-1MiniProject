using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    protected override void Start()
    {
        base.Start();
        // �ִ� ü��, �ִ� ����, ���ݷ�, ���ݼӵ�, ��Ÿ�, ��������, ��ų����, ����ġ, ���, ������ ���
        InitStat(80, 0, 5, 0.75f, 1.3f, true, false, 10, 250, ItemResources.instance.itemRS[4]);

        if (!BattleManager.Instance.dialogue.isTutorial)
            item_Drop_Check = ShouldDropItem(25);

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
