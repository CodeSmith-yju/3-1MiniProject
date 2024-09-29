using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private BaseEntity shooter;
    private BaseEntity target;
    public float speed = 10f;
    float total_Dmg;
    
    public void Shoot(BaseEntity shooter, BaseEntity target, float dmg) 
    {
        this.shooter = shooter;
        this.target = target;
        this.total_Dmg = dmg;
    }

    private void Update()
    {
        if (target != null)
        {
            // TODO : �̵�
            // Todo : Ÿ�� ��ġ�� �� ��ġ�� ���� Angle ����

            Collider2D target_Col = target.GetComponent<Collider2D>();

            Vector3 target_Pos = target_Col.transform.TransformPoint(target_Col.offset);

            Vector2 direction = (target_Pos - transform.position).normalized;
            transform.right = direction;

            transform.position = Vector2.MoveTowards(transform.position, target_Pos, Time.deltaTime * speed);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BaseEntity hiter = collision.GetComponent<BaseEntity>();
        if (target == hiter)
        {
            // TODO : �ִϸ��̼� �ϼ��� Ȱ��ȭ �ʿ�
            //enemy.ani.SetTrigger("isHit");

            shooter.RangeHit(hiter, total_Dmg);

            this.gameObject.SetActive(false);
        }
    }
}
