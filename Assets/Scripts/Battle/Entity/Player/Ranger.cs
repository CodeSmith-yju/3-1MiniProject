using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class Ranger : Ally
{
    protected override void Start()
    {
        base.Start();
        Debug.Log("Ranger ����");
        foreach (GameObject player in BattleManager.Instance.party_List)
        {
            BaseEntity player_Stat = player.GetComponent<BaseEntity>();

            if (player_Stat.entity_index == GetComponent<BaseEntity>().entity_index)
            {
                InitStat(entity_index);
            }
        }
    }

    protected override void Update()
    {
        base.Update();
        /*if (_curstate == State.Skill)
        {
            Skill();
        }*/
        //cur_target = target;
    }


    /*public void Skill()
    {
        if (_curstate == State.Skill)
        {
            
            StopAllCoroutines();
            if (isAttack)
            {
                
                BaseEntity target = FindTarget().GetComponent<BaseEntity>();
                Debug.Log("Ÿ���� ������ 2���� �������� �ѹ� ����" + " " + (atkDmg * 2) + "������");
                target.cur_Hp -= atkDmg * 2;
                Debug.Log(target.cur_Hp + " " + target.name);
            }
            else
            {
                return;
            }
            cur_Mp = 0;
            ChangeState(State.Idle);
        }
    }*/
}