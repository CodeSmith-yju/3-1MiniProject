using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class BaseEntity : MonoBehaviour
{
    public float max_Hp;
    public float cur_Hp;
    public float max_Mp;
    public float cur_Mp;
    public float atkDmg;
    public float atkSpd;
    public float atkRange;
    public bool isAttack = false;
    public bool isAtkDone = false;
    public bool able_Skill = false;

    NavMeshAgent agent;

    

    // �÷��̾�, �� ������Ʈ�� � �ൿ�� �ϴ��� ����
    public enum State
    {
        Idle,
        Move,
        Attack,
        Skill,
        Death
    }

    public State _curstate;
    protected StateManager _stateManager;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }


    protected virtual void Start()
    {
        _curstate = State.Idle;
        _stateManager = new StateManager(new IdleState(this));

        max_Hp = 10f;
        cur_Hp = max_Hp;
        max_Mp = 5f;
        cur_Mp = max_Mp;
        atkDmg = 1f;
        atkRange = 1f;
        atkSpd = 1f;
        able_Skill = false;
    }

    // ��Ʋ ���� �� ���� ��Ʋ ����
    protected virtual void Update()
    {
        if (BattleManager.Instance._curphase == BattleManager.BattlePhase.Battle)
        {
            switch (_curstate)
            {
                case State.Idle:
                    if (FindTarget() != null)
                    {
                        if (IsAttack(atkRange))
                        {
                            ChangeState(State.Attack);
                        }
                        else
                        {
                            ChangeState(State.Move);
                        }
                    }
                    break;

                case State.Move:
                    if (FindTarget() != null)
                    {
                        if (IsAttack(atkRange))
                        {
                            ChangeState(State.Attack);
                        }
                    }
                    else
                    {
                        ChangeState(State.Idle);
                    }
                    break;

                case State.Attack:
                    if (FindTarget() != null)
                    {
                        if (!IsAttack(atkRange))
                        {
                            ChangeState(State.Move);
                        }

                        if (isAtkDone)
                        {
                            Debug.Log("���� �Ϸ� - Idle�� ���� ���� (���ο� Ÿ�� ����)");
                            isAtkDone = false;
                            ChangeState(State.Idle);
                        }
                    }
                    else
                    {
                        Debug.Log("Ÿ�� ����");
                        isAtkDone = false;
                        ChangeState(State.Idle);
                    }
                    break;
                case State.Death:
                    Destroy(gameObject);
                    break;
            }

            _stateManager.UpdateState();

            // ���� ü���� 0�� �Ǹ� Death ���·� ���ϰ� ����â�� ���� ������ ǥ��
            if (cur_Hp <= 0)
            {
                _curstate = State.Death;
            }

            if (able_Skill)
            {
                if (cur_Mp == max_Mp && cur_Mp != 0)
                {
                    _curstate = State.Skill;
                }
            }
            

        }

        /*if (gameObject != null)
        {
            if (transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(-0.75f, 0.75f, 1f);
            }
            if (transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(0.75f, 0.75f, 1f);
            }
        }*/
    }

    // ���� ��ȯ �޼���
    protected void ChangeState(State newState)
    {
        _curstate = newState;

        switch (_curstate)
        {
            case State.Idle:
                _stateManager.ChangeState(new IdleState(this));
                break;
            case State.Move:
                _stateManager.ChangeState(new MoveState(this));
                break;
            case State.Attack:
                _stateManager.ChangeState(new AttackState(this));
                break;
        }
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
        return nearestTarget != null ? nearestTarget : null;
    }


    // 1�ʸ��� Ÿ���� ������Ʈ �ϴ� �޼ҵ�
    public IEnumerator UpdateTarget()
    {
        if (FindTarget() != null) 
        {
            while (true)
            {
                yield return new WaitForSeconds(0.5f); // 1�� ���
                FindTarget();
            }
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
        else
        {
            return;
        }
        
    }

    // ���� ��Ÿ��� ������ �̵� ���߰� ���� �غ�
    public void StopMove()
    {
        if (isAttack)
        {
            agent.isStopped = true;
            SetMovementPriority(false);
        }
    }


    // ���� ��Ÿ��� ���� �������� True or False ��ȯ�ϴ� �޼���
    public bool IsAttack(float range)
    {
       
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

    public IEnumerator Attack()
    {
        if (_curstate == State.Attack)
        {
            BaseEntity target = FindTarget()?.GetComponent<BaseEntity>();

            if (target != null && isAttack)
            {
                while (true)
                {
                    yield return new WaitForSeconds(atkSpd);

                    if (target == null || target.cur_Hp <= 0)
                    {
                        // ���� ����
                        isAtkDone = true;
                        break;
                    }
                    Debug.Log("������");
                    if (able_Skill)
                    {
                        cur_Mp++;
                    }

                    float getDmgHp = target.cur_Hp - atkDmg;
                    target.cur_Hp = getDmgHp;
                    Debug.Log(target.cur_Hp + " " + target.name);
                }
            }
        }
    }
}
