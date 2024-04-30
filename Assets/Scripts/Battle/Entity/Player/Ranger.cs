using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.UI;

public class Ranger : BaseEntity
{
    private EntityStat stat;
    Transform cur_target;

    protected override void Start()
    {
        base.Start();
        Debug.Log("Ranger ����");

        // ���� id, �ִ� HP, �ִ� MP, ���ݷ�, ���ݼӵ�, ���ݻ�Ÿ� ������ �ʱ�ȭ
        stat = new EntityStat
            (1, 15, 5, 2, 1, 8, false);

        entity_id = stat.id;
        max_Hp = stat.max_Hp;
        cur_Hp = max_Hp;
        max_Mp = stat.max_Mp;
        cur_Mp = 0;
        atkDmg = stat.atkDmg;
        SetAttackSpeed(stat.atkSpd);
        atkRange = stat.atkRange;
        able_Skill = stat.isSkill;
        isMelee = true; // �ӽ÷� ���� ���ְ� �Ȱ��� ������� ���� ���Ŀ� ����ü�� �߻��ϴ� ������� �ٲ� ����
    }

    protected override void Update()
    {
        base.Update();
        if (_curstate == State.Skill)
        {
            Skill();
        }
        cur_target = target;
    }


    public void Skill()
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
    }

    public void Arrow()
    {
        if (_curstate == State.Attack) 
        {
            StartCoroutine(ArrowCoroutine(cur_target));
        }
    }

    private IEnumerator ArrowCoroutine(Transform target)
    {
        GameObject arrow = BattleManager.Instance.pool.GetObject(0);
        arrow.transform.position = transform.position;

        


        if (target != null)
        {
            Vector3 direction = (target.position - arrow.transform.position).normalized;

            // ȭ���� ȸ�� ����
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            arrow.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            while (Vector3.Distance(arrow.transform.position, target.position) > 0.1f)
            {
                // ����ü �̵�
                arrow.transform.position = Vector3.MoveTowards(arrow.transform.position, target.position, (10f * atkSpd) * Time.deltaTime);
               
                yield return null; // ���� �����ӱ��� ���
            }
            arrow.SetActive(false);
            Debug.Log("ȭ�� ��");
        }
        else
        {
            arrow.SetActive(false);
        }
      
    }

}