using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{
    public IdleState(BaseEntity e) : base(e) { }

    public override void OnStateEnter()
    {
        if (BattleManager.Instance._curphase == BattleManager.BattlePhase.Battle)
        {
            if (entity != null && entity.FindTarget() != null && entity.FindTarget().GetComponent<BaseEntity>()._curstate != BaseEntity.State.Death)
            {
                entity.StopMove();
                entity.ani.ResetTrigger("isAtk");
                entity.ani.SetBool("isMove", false);
                entity.StartCoroutine(entity.UpdateTarget());
            }
        }
    }

    public override void OnStateUpdate()
    {
        
    }

    public override void OnStateExit()
    {
        if (BattleManager.Instance._curphase == BattleManager.BattlePhase.Battle)
        {
            if (entity.isAttack)
            {
                entity.StopCoroutine(entity.UpdateTarget());
            }
        }
    }
}
