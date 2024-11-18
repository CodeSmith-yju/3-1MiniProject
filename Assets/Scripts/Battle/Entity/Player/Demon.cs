using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class Demon : Ally
{
    [SerializeField] RuntimeAnimatorController metamorphosis_Ani;
    bool isMeta = false;
    bool isMetaCheck = false;
    bool isDrain = false;

    protected override void Start()
    {
        base.Start();
        Debug.Log("Demon 생성");
        isMeta = false;
        isMetaCheck = false;
        isDrain = false;
        able_Skill = true;
        type = Class.Melee;
        job = Job.Demon;
    }


    protected override void Update()
    {
        base.Update();

        if (isMeta && isMetaCheck)
        {
            statManager.player_Icon.sprite = ChangePortrate(isMeta);
            isMetaCheck = false;
        }
        else if (!isMeta && !isMetaCheck)
        {
            statManager.player_Icon.sprite = ChangePortrate(isMeta);
            
            isMetaCheck = true;
        }

    }

    protected override void MeleeAttack(BaseEntity target)
    {
        base.MeleeAttack(target);

        if (isDrain)
        {
            cur_Hp += atkDmg * 0.3f; // 공격력의 30프로 만큼 체력 회복
        }
    }

    protected override void Skill()
    {
        base.Skill();
        if (_curstate == State.Skill)
        {
            StopCoroutine(SetAttack());
            if (!isMeta)
            {
                // 변신
                BattleManager.Instance.ui.GenerateLog(GameUiMgr.single.entityIconRS.GetPortraitIcon(job), "폭주");
                ani.SetBool("isSkill", true);

                cur_Mp = 0;
            }
        }
        else
        {
            return;
        }
    }

    private void MetaAfterStat()
    {
        isMadness = true; // 받는 피해 15% 증가 디버프
        isArea_Atk = true; // 광역 공격을 하도록 추가
        isDrain = true; // 광역 공격을 포함
        atkDmg *= 1.3f; // 현재 공격력의 30프로 만큼 상승
        atkSpd *= 1.1f; // 현재 공격속도에서 10프로 만큼 상승
        atkRange += 0.5f; // 공격 사거리 증가

        able_Skill = false; // 변신 후에는 스킬이 없으므로 사용할 수 없게 변경
    }

    public void Metamorphosis()
    {
        ani.SetBool("isSkill", false);
        ani.runtimeAnimatorController = metamorphosis_Ani;
        isMeta = true;
        MetaAfterStat();
        ChangeState(State.Idle);
        using_Skill = false;
    }

    private Sprite ChangePortrate(bool isMeta)
    {
        if (isMeta)
        {
            //return statManager.player_Icon = GameUiMgr.single.entityIconRS.GetDoublePortraitIcon(Ally.Job.Demon); // 변신 초상화
        }
        else
        {
            return statManager.player_Icon.sprite = GameUiMgr.single.entityIconRS.GetPortraitIcon(Ally.Job.Demon); // 평소 초상화

        }

        return null;
    }

}