using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private BaseEntity shooter;
    private BaseEntity target;
    public float speed = 10f;
    
    public void Shoot(BaseEntity shooter, BaseEntity target) 
    {
        this.shooter = shooter;
        this.target = target;
    }

    private void Update()
    {
        if (target != null)
        {
            // TODO : �̵�
            // Todo : Ÿ�� ��ġ�� �� ��ġ�� ���� Angle ����

            Vector2 direction = (target.transform.position - transform.position).normalized;
            transform.right = direction;

            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, Time.deltaTime * speed);
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

            shooter.RangeHit(hiter);

            this.gameObject.SetActive(false);
        }
    }
}
