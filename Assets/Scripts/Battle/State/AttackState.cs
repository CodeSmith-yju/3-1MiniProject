using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    public AttackState(BaseEntity e) : base(e) { }

    public override void OnStateEnter()
    {
        if (BattleManager.Instance._curphase == BattleManager.BattlePhase.Battle)
        {
            if (entity != null && entity.FindTarget() != null)
            {
                entity.StopMove();

                entity.StartCoroutine(entity.SetAttack());
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
            if (entity != null && entity.FindTarget() != null)
            {
                entity.StopCoroutine(entity.SetAttack());
                entity.ani.ResetTrigger("isAtk");
            }
        }  
    }
}
