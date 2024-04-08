using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class BaseEntity : MonoBehaviour
{
    protected float hp;
    protected float mp;
    protected float atkDmg;
    protected float atkRange = 1f;

    NavMeshAgent agent;
    SpriteRenderer sprite;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        sprite = GetComponent<SpriteRenderer>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }


    // �����̿� �ִ� ���� Ÿ���ϴ� �޼ҵ�

    public GameObject FindTarget()
    {
        string targetTag = (tag == "Player") ? "Enemy" : "Player";
        // Ÿ�� ������Ʈ �迭 ã��
        GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);
        // ���� ��ġ
        Vector3 currentPosition = transform.position;
        // ���� ����� ��� ã��
        GameObject nearestTarget = targets.OrderBy(target => Vector3.Distance(currentPosition, target.transform.position)).FirstOrDefault();
        // ã�� ��� ��ȯ
        return nearestTarget;
    }


    // 1�ʸ��� Ÿ���� ������Ʈ �ϴ� �޼ҵ�
    public IEnumerator UpdateTarget()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f); // 1�� ���
            FindTarget();
        }
    }

    // Idle �����̰ų� Attack �����϶� �ִ��� ���Ҽ� �ְ� �켱���� ������ �޼��� ( NavMeshPlus ���� ���� )
    public void SetMovementPriority(bool isMoving)
    {
        int priority = isMoving ? 50 : 30; // �̵� ���̸� �켱������ 50����, �ƴϸ� 30�� ����
        agent.avoidancePriority = priority;
    }

    
    // Ÿ������ ���� �̵��ϴ� �޼��� ( NavMeshPlus �̿� )
    public void MoveToTarget()
    {
        Transform target = FindTarget().transform;
        if(target != null) 
        {
            agent.isStopped = false;
            agent.SetDestination(target.position);
            SetMovementPriority(true);
        }
        
    }

    // ���� ��Ÿ��� ������ �̵� ���߰� ���� �غ�
    public void StopMove()
    {
        if (IsAttack())
        {
            agent.isStopped = true;
            SetMovementPriority(false);
        }
    }


    // ���� ��Ÿ��� ���� �������� True or False ��ȯ�ϴ� �޼���
    public bool IsAttack()
    {
        bool isAttack;

        Transform target = FindTarget().transform;

        Vector2 tVec = (Vector2)(target.localPosition - transform.position);
        float tDis = tVec.sqrMagnitude;

        if (tDis <= atkRange * atkRange)
        {
            isAttack = true;
        }
        else
        {
            isAttack = false;
        }

        return isAttack;
    }

    public void Attack()
    {
        Debug.Log("������");
    }
}
