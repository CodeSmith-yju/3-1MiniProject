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

        foreach (PlayerData player in GameMgr.playerData)
        {
            foreach (GameObject obj in BattleManager.Instance.party_List)
            {
                if (obj.GetComponent<Entity_Unique>().index == player.playerIndex)
                {
                    InitStat(player.playerIndex);
                }
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