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



    // 플레이어, 적 오브젝트의 어떤 행동을 하는지 설정
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


    // 배틀 시작 시 오토 배틀 로직
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
                            Debug.Log("Idle 상태에서 Attack 상태로 변경 " + gameObject.name);
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
                            Debug.Log("공격 완료 - Idle로 상태 변경 (새로운 타겟 지정) ( 실행 오브젝트 : " + name + " )");
                            ChangeState(State.Idle);
                            isAtkDone = false;                           
                        }
                    }
                    else
                    {
                        Debug.Log("타겟 없음");
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

            if (_curstate != State.Death)
            {
                cur_atk_CoolTime += Time.deltaTime;
            }

            // 현재 체력이 0이 되면 Death 상태로 변하고 상태창도 죽은 것으로 표시
            if (cur_Hp <= 0)
            {
                isDie = true;
                _curstate = State.Death;
            }

            /*if (able_Skill)
            {
                if (cur_Mp == max_Mp && cur_Mp != 0)
                {
                    _curstate = State.Skill;
                }
            }*/
            

        }
        
        // 위치에 따른 바라보는 방향 조절
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

    // 상태 변환 메서드
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




    // 가까이에 있는 적을 타겟하는 메소드

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


    // 0.3초마다 타겟을 업데이트 하는 메소드
    /*    public IEnumerator UpdateTarget()
        {
            if (FindTarget() != null) 
            {
                while (true)
                {
                    if (FindTarget() == null)
                    {
                        Debug.Log("타겟이 없으므로 멈춤");
                        StopMove();
                        break;
                    }

                    FindTarget();
                    yield return new WaitForSeconds(0.3f); // 대기

                }
            }
            else
            {
                Debug.Log("타겟 없음");
                ChangeState(State.Idle);
                yield break;
            }
        }*/

    // 
    public IEnumerator UpdateTarget()
    {
        while (true)
        {
            var target = FindTarget();
            if (target == null || target.GetComponent<BaseEntity>()._curstate == State.Death)
            {
                Debug.Log("타겟이 없으므로 멈춤");
                StopMove();
                ChangeState(State.Idle);
                yield break;
            }

            // 타겟이 바뀌거나 죽었는지 계속 체크
            yield return new WaitForSeconds(0.1f); // 타겟을 빠르게 찾음
        }
    }

    // Idle 상태이거나 Attack 상태일때 최대한 피할수 있게 우선순위 높히는 메서드 ( NavMeshPlus 에셋 관련 )
    public void SetMovementPriority(bool isMoving)
    {
        int priority = isMoving ? 50 : 30; // 이동 중이면 우선순위를 50으로, 아니면 30로 설정
        agent.avoidancePriority = priority;
    }

    
    // 타겟으로 향해 이동하는 메서드 ( NavMeshPlus 이용 )
    public void MoveToTarget()
    {
        Collider2D target = FindTarget().GetComponent<Collider2D>();
        if (target != null && _curstate != State.Death) 
        {
            Vector3 targetPosition = target.transform.TransformPoint(target.offset);

            agent.isStopped = false;
            agent.SetDestination(targetPosition);
            SetMovementPriority(true);
        }
        else if (target == null & _curstate != State.Death)
        {
            agent.ResetPath();
            StopMove();
            return;
        }
        else
        {
            return;
        }
        
    }

    // 공격 사거리에 들어오면 이동 멈추고 공격 준비
    public void StopMove()
    {
        if (_curstate != State.Move)
        {
            agent.isStopped = true;
            SetMovementPriority(false);
        }
    }


    // 공격 사거리에 오면 논리형으로 True or False 반환하는 메서드
    public bool IsAttack(float range)
    {
        //Transform target = FindTarget().transform;

        Collider2D target = FindTarget().GetComponent<Collider2D>();
        Collider2D my = GetComponent<Collider2D>();
        Vector3 targetPosition = target.transform.TransformPoint(target.offset);
        Vector3 myPosition = my.transform.TransformPoint(my.offset);

        //Vector2 tVec = (Vector2)(target.position - transform.position);
        Vector2 tVec = (Vector2)(targetPosition - myPosition);
        float tDis = tVec.sqrMagnitude;
        //float targetRadius = target.GetComponent<NavMeshAgent>().radius;


        /*if (isMelee)
         {
             //float meleeRange_Value = range + targetRadius;
             //float meleeRange = meleeRange_Value * meleeRange_Value;
             if (tDis <= range * range)
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
         }*/

        if (tDis <= range * range)
        {
            isAttack = true;
        }
        else
        {
            isAttack = false;
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
                        // 공격 중지
                        isAtkDone = true;
                        break;
                    }

                    if (!isMelee)
                    {
                        target = FindTarget()?.GetComponent<BaseEntity>();
                    }

                    if (cur_atk_CoolTime >= atk_CoolTime)
                    {
                        cur_atk_CoolTime = 0;

                        if (able_Skill && cur_Mp == max_Mp)
                        {
                            _curstate = State.Skill;
                            break;
                        }
                        else
                        {
                            if (isMelee)
                            {
                                MeleeAttack(target);
                                // 스킬을 사용할 수 있으면 공격 시 1Mp 회복
                                if (able_Skill)
                                    cur_Mp++;
                            }
                            else
                            {
                                RangeAttack(target);
                                // 스킬을 사용할 수 있으면 공격 시 1Mp 회복
                                if (able_Skill)
                                    cur_Mp++;
                            }
                        }
                    }
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
        Debug.Log("공격함 ( " + name + " -> " + target.name + " )");

        float getDmgHp = target.cur_Hp - atkDmg;
        target.cur_Hp = getDmgHp;
        Debug.Log(target.cur_Hp + " " + target.name);
    }

    public virtual void RangeAttack(BaseEntity target) 
    {
        Debug.Log("공격함 ( " + name + " -> " + target.name + " )");
        ani.SetTrigger("isAtk");
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
