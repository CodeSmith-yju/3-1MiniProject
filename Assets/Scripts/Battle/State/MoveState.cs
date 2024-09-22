using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class MoveState : BaseState
{

    public MoveState(BaseEntity e) : base(e) { }

    public override void OnStateEnter()
    {
        if (entity != null && entity.FindTarget() != null)
        {
            Debug.Log("Move Enter ½ÇÇàµÊ");
            entity.ani.ResetTrigger("isAtk");
            entity.ani.SetBool("isMove", true);

            entity.MoveToTarget();
        }
    }

    public override void OnStateUpdate()
    {
        if (entity != null && entity.FindTarget() != null)
        {
            entity.MoveToTarget();
        }
    }

    public override void OnStateExit()
    {
        if (entity != null && entity.FindTarget() != null)
        {
            entity.ani.SetBool("isMove", false);
            entity.StopCoroutine(entity.UpdateTarget());
            //entity.target = entity.FindTarget().transform;
        }
    }
}
