using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    public AttackState(BaseEntity e) : base(e) { }

    public override void OnStateEnter()
    {
        if (entity != null && entity.FindTarget() != null)
        {
            entity.StopMove();
            entity.StartCoroutine(entity.Attack());
        }
        
    }

    public override void OnStateUpdate()
    {
    }

    public override void OnStateExit()
    {
        if (entity != null && entity.FindTarget() != null)
        {
            entity.StopCoroutine(entity.Attack());
        }
    }
}
