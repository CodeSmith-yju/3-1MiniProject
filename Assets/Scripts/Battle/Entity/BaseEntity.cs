using System.Collections;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.GraphicsBuffer;

public class BaseEntity : MonoBehaviour
{
    public int entity_index;
    public float max_Hp;
    public float cur_Hp;
    public float max_Mp;
    public float cur_Mp;
    public float atkDmg;
    public float atkSpd;
    public float atkRange;
    public bool isAttack = false;
    public bool isAtkDone = false;
    public bool isDie = false;
    private bool isDieInProgress = false;
    public bool isMelee = false;
    public bool able_Skill = false;

    private float atk_CoolTime;
    private float cur_atk_CoolTime;

    //public Transform target;
    public NavMeshAgent agent;
    SpriteRenderer sprite;
    public Animator ani;
    protected EntityStat stat;



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
        sprite = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }


    protected virtual void Start()
    {
        _curstate = State.Idle;
        _stateManager = new StateManager(new IdleState(this));
    }

    public void SetAttackSpeed(float speed)
    {
        atkSpd = speed;

        atk_CoolTime = 1f / atkSpd;

        cur_atk_CoolTime = atk_CoolTime;
        
        if (atkSpd > 1)
        {
            ani.SetFloat("AtkSpeed", atkSpd);
        }
        else
        {
            ani.SetFloat("AtkSpeed", 1);
        }
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
                            Debug.Log("Idle ���¿��� Attack ���·� ���� " + gameObject.name);
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
                        if (isAtkDone)
                        {
                            Debug.Log("���� �Ϸ� - Idle�� ���� ���� (���ο� Ÿ�� ����) ( ���� ������Ʈ : " + name + " )");
                            ChangeState(State.Idle);
                            isAtkDone = false;                           
                        }
                    }
                    else
                    {
                        Debug.Log("Ÿ�� ����");
                        ChangeState(State.Idle);
                        isAtkDone = false;
                        
                    }
                    break;
                case State.Death:
                    if (isDie && !isDieInProgress)
                    {
                        isDieInProgress = true;
                        StartCoroutine(Die());
                    }
                    break;
            }

            _stateManager.UpdateState();

            // ���� ü���� 0�� �Ǹ� Death ���·� ���ϰ� ����â�� ���� ������ ǥ��
            if (cur_Hp <= 0)
            {
                isDie = true;
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

        if (gameObject != null)
        {
            Vector3 Direction = agent.velocity.normalized;
            if (Direction.x < 0)
            {
                sprite.flipX = true;

            }
            else if (Direction.x > 0)
            {
                sprite.flipX = false;
            }
        }
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
        GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);
        Vector3 currentPosition = transform.position;

        GameObject nearestTarget = targets
            .Select(target => new { obj = target, distance = Vector3.Distance(currentPosition, target.transform.position) })
            .Where(x => x.obj.GetComponent<BaseEntity>() != null && x.obj.GetComponent<BaseEntity>()._curstate != State.Death)
            .OrderBy(x => x.distance)
            .FirstOrDefault()?.obj;

        return nearestTarget;
    }


    // 1�ʸ��� Ÿ���� ������Ʈ �ϴ� �޼ҵ�
    public IEnumerator UpdateTarget()
    {
        if (FindTarget() != null) 
        {
            while (true)
            {
                if (FindTarget() == null)
                {
                    Debug.Log("Ÿ���� �����Ƿ� ����");
                    StopMove();
                    break;
                }


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
        if (_curstate != State.Move)
        {
            agent.isStopped = true;
            SetMovementPriority(false);
        }
    }


    // ���� ��Ÿ��� ���� �������� True or False ��ȯ�ϴ� �޼���
    public bool IsAttack(float range)
    {
        Transform target = FindTarget().transform;

        Vector2 tVec = (Vector2)(target.position - transform.position);
        float tDis = tVec.sqrMagnitude;
        float targetRadius = target.GetComponent<NavMeshAgent>().radius;


        if (isMelee)
        {
            float meleeRange_Value = range + targetRadius;
            float meleeRange = meleeRange_Value * meleeRange_Value;

            if (tDis <= meleeRange)
            {
                isAttack = true;
            }
            else
            {
                isAttack = false;
            }
        }
        else
        {
            if (tDis <= range * range)
            {
                isAttack = true;
            }
            else
            {
                isAttack = false;
            }
        }
        

        return isAttack;
    }

    public IEnumerator SetAttack()
    {
        if (_curstate == State.Attack && _curstate != State.Death)
        {
            BaseEntity target = FindTarget()?.GetComponent<BaseEntity>();

            if (target != null && isAttack)
            {
                while (true)
                {

                    if (target == null || target.cur_Hp <= 0 || this._curstate == State.Death)
                    {
                        // ���� ����
                        isAtkDone = true;
                        break;
                    }

                    if (cur_atk_CoolTime >= atk_CoolTime)
                    {
                        cur_atk_CoolTime = 0;

                        if (isMelee)
                        {
                            MeleeAttack(target);
                        }
                        else
                        {
                            RangeAttack(target);
                        }
                    }
                    cur_atk_CoolTime += Time.deltaTime;
                    yield return null;
                }
            }
        }
        else
        {
            yield break;
        }
    }

    private void MeleeAttack(BaseEntity target)
    {
        ani.SetTrigger("isAtk");
        Debug.Log("������ ( " + name + " -> " + target.name + " )");

        if (able_Skill)
        {
            cur_Mp++;
        }

        float getDmgHp = target.cur_Hp - atkDmg;
        target.cur_Hp = getDmgHp;
        Debug.Log(target.cur_Hp + " " + target.name);
    }

    private void RangeAttack(BaseEntity target) 
    {
        Debug.Log("������ ( " + name + " -> " + target.name + " )");
        ani.SetTrigger("isAtk");
        GameObject obj_Arrow = BattleManager.Instance.pool.GetObject(0);
        obj_Arrow.transform.position = transform.GetChild(0).position;
        Arrow arrow = obj_Arrow.GetComponent<Arrow>();
        arrow.Shoot(this, target);
    }

    public void RangeHit(BaseEntity target)
    {
        float getDmgHp = target.cur_Hp - atkDmg;
        target.cur_Hp = getDmgHp;
        Debug.Log($"Hit to {target.name}! {target.cur_Hp}");
    }

    private IEnumerator Die()
    {
        isDie = false;
        ani.SetTrigger("isDie");
        agent.enabled = false;
        if (gameObject.CompareTag("Player"))
        {
            if (BattleManager.Instance.deploy_Player_List.Contains(gameObject))
            {
                BattleManager.Instance.deploy_Player_List.Remove(gameObject);
            }
        }
        else if (gameObject.CompareTag("Enemy"))
        {
            if (BattleManager.Instance.deploy_Enemy_List.Contains(gameObject))
            {
                BattleManager.Instance.deploy_Enemy_List.Remove(gameObject);
            }
        }
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
        isDieInProgress = false;
    }

}
