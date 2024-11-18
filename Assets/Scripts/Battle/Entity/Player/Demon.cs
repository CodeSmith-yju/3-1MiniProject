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
        Debug.Log("Demon ����");
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
            cur_Hp += atkDmg * 0.3f; // ���ݷ��� 30���� ��ŭ ü�� ȸ��
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
                // ����
                BattleManager.Instance.ui.GenerateLog(GameUiMgr.single.entityIconRS.GetPortraitIcon(job), "����");
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
        isMadness = true; // �޴� ���� 15% ���� �����
        isArea_Atk = true; // ���� ������ �ϵ��� �߰�
        isDrain = true; // ���� ������ ����
        atkDmg *= 1.3f; // ���� ���ݷ��� 30���� ��ŭ ���
        atkSpd *= 1.1f; // ���� ���ݼӵ����� 10���� ��ŭ ���
        atkRange += 0.5f; // ���� ��Ÿ� ����

        able_Skill = false; // ���� �Ŀ��� ��ų�� �����Ƿ� ����� �� ���� ����
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
            //return statManager.player_Icon = GameUiMgr.single.entityIconRS.GetDoublePortraitIcon(Ally.Job.Demon); // ���� �ʻ�ȭ
        }
        else
        {
            return statManager.player_Icon.sprite = GameUiMgr.single.entityIconRS.GetPortraitIcon(Ally.Job.Demon); // ��� �ʻ�ȭ

        }

        return null;
    }

}