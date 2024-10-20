using System.Collections;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public Rigidbody2D rigid;
    public Animator animator;

    float h, v;
    Vector2 moveDirection;

    Vector3 dirVec;// Ray를 발사하고 충돌을 체크할때 사용할, 현재 바라보고 있는 방향 값을 가진 변수 선언 
    GameObject scanRayObjcet;// Ray에 충돌한 오브젝트를 저장할 변수

    void Update()
    {
        // 입력 값을 확인
        bool left = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
        bool right = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);
        bool up = Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W);
        bool down = Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S);

        // 이동 방향 설정
        h = (right ? 1 : 0) - (left ? 1 : 0);
        v = (up ? 1 : 0) - (down ? 1 : 0);

        // 애니메이션 상태 업데이트
        UpdateAnimation();

        // Scan Ray Object
        if (GameUiMgr.single.move_doit && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) && scanRayObjcet != null || (GameUiMgr.single.isActionTalk && Input.GetMouseButtonDown(0)))
        {
            GameUiMgr.single.HideSubButtons();
            //Debug.Log("This is : "+ scanRayObjcet.name);// 레이에 맞고 스페이스바를 통해 상호작용하여 Scan Object에 저장된 물체가 있을 시 오브젝트 이름 출력 
            //Debug.Log(scanRayObjcet.name); // 04- 23 Debug

            ObjectData obj = scanRayObjcet.GetComponent<ObjectData>();
            if (obj.id == 4000)
            {
                if (GameUiMgr.single.questMgr.questId >= 50)
                {
                    rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
                }
            }
            else if (obj.id == 9000)
            {
                if (GameUiMgr.single.questMgr.questId >= 30)
                {
                    if (GameUiMgr.single.questMgr.questId > 30 || GameUiMgr.single.questMgr.questId == 30 && GameUiMgr.single.questMgr.questActionIndex >= 1)
                    {
                        GameUiMgr.single.panelPartyBoard.SetActive(true);
                        Debug.Log("id: " + obj.id);
                        return;
                    }
                }
                else
                    return;
            }
            else if (obj.id == 8000)
            {
                Debug.Log("8000 실행");
                Debug.Log("스태미나가좀이상한데 못들어가야되는데 팝업넣었는데?");
                Debug.Log("현재 스태미나: " + GameMgr.playerData[0].max_Player_Sn);
                Debug.Log("현재 스태미나: " + GameMgr.playerData[0].cur_Player_Sn);
                if (GameMgr.playerData[0].cur_Player_Sn < 15)
                {
                    GameUiMgr.single.popUp.SetPopUp("기력이 부족합니다.\n소지금을 10%잃고 회복합니다.", PopUpState.NoStamina);
                    return;
                }

                if (GameUiMgr.single.questMgr.questId >= 40 && GameUiMgr.single.questMgr.questId < 50)
                {
                    GameUiMgr.single.isDungeon = true;
                    GameUiMgr.single.OpenDungeonUi();
                    //GameUiMgr.single.textEquipPanel.text = "던전에 입장하시겠습니까?";//OK버튼 클릭했을때 다른효과가 나와야하는데 생각조금 더 해봐야함
                    //GameUiMgr.single.addEquipPanel.gameObject.SetActive(true);
                    return;
                }
                else if (GameUiMgr.single.questMgr.questId >= 50)
                {
                    GameUiMgr.single.OpenDungeonUi();
                    return;
                }
                else
                {
                    // ToDo: CallBack Img.SetActive(true);
                    Debug.Log("튜토리얼 던전에 진입할 수 없습니다.");
                    return;
                }
            }
            else if (obj.id == 11000)
            {
                Debug.Log("Id : 11000");
                GameUiMgr.single.ChangePlayerPlace(PlaceState.Town);
                return;
            }

            GameUiMgr.single.TalkAction(scanRayObjcet);// Ray로 상호작용한 Object의 정보를 twonMgr로 넘겨서 그곳에 있는 대화창에 정보를 출력하게 함.
        }
    }

    private void FixedUpdate()
    {
        moveDirection = new Vector2(h, v).normalized;
        if (!GameUiMgr.single.isActionTalk && GameUiMgr.single.move_doit)
        {
            rigid.velocity = moveDirection * 4f;
        }
    }

    void UpdateAnimation()
    {
        if (GameUiMgr.single.isActionTalk)
        {
            h = 0;
            v = 0;
        }

        bool isMovingLeft = h < 0;
        bool isMovingRight = h > 0;
        bool isMovingUp = v > 0;
        bool isMovingDown = v < 0;
        bool isIdle = !isMovingLeft && !isMovingRight && !isMovingUp && !isMovingDown;

        // 애니메이터의 파라미터 업데이트
        animator.SetBool("IsMovingLeft", isMovingLeft);
        animator.SetBool("IsMovingRight", isMovingRight);
        animator.SetBool("IsMovingUp", isMovingUp);
        animator.SetBool("IsMovingDown", isMovingDown);
        animator.SetBool("IsIdle", isIdle);

        // 애니메이션 상태에 따라 캐릭터의 이동을 조정
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Player_Left_Walk"))
        {
            // Left 애니메이션이 실행 중일 때
            h = -1; // 왼쪽 이동
            v = 0;  // 상하 이동 없음
            dirVec = Vector3.left;// RayCast Direction
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (stateInfo.IsName("Player_Right_Walk"))
        {
            // Right 애니메이션이 실행 중일 때
            h = 1; // 오른쪽 이동
            v = 0;  // 상하 이동 없음
            dirVec = Vector3.right;
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (stateInfo.IsName("Player_Up_Walk"))
        {
            // Up 애니메이션이 실행 중일 때
            h = 0; // 좌우 이동 없음
            v = 1; // 위쪽 이동
            dirVec = Vector3.up;
        }
        else if (stateInfo.IsName("Player_Down_Walk"))
        {
            // Down 애니메이션이 실행 중일 때
            h = 0; // 좌우 이동 없음
            v = -1; // 아래쪽 이동
            dirVec = Vector3.down;
        }
        else if (stateInfo.IsName("Player_Idle"))
        {
            // Idle 애니메이션이 실행 중일 때
            h = 0;
            v = 0;
        }

        if (GameUiMgr.single.move_doit)
        {
            StopPosition(false);
        }
        else
        {
            StopPosition(true);
            dirVec = Vector3.zero;
            rigid.velocity = Vector2.zero;
        }

        // RayCast Draw
        DrawPlayerRay();
    }

    void DrawPlayerRay()
    {
        // 다른 오브젝트와의 충돌을 체크하기위한 Ray
        Debug.DrawRay(rigid.position, dirVec * 0.8f, new Color(0, 1, 0));// DrawRay (Scene창에서만 보임)를 발사하는데, 현재 캐릭터의 위치값, 길이(float), 색갈(new Color)이 필요
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, dirVec, 1.3f, LayerMask.GetMask("Object"));// 마스크가 Object인 경우에만 충돌(이미 플레이어가 사용중인 콜라이더와의 충돌을 피하기 위함.)

        //레이가 오브젝트와 충돌할 시 ScanObject에 저장 
        if (rayHit.collider != null)
            scanRayObjcet = rayHit.collider.gameObject;// RayCast된 오브젝트를 변수로 저장하여 활용 할 수 있게함
        else
            scanRayObjcet = null;// 충돌이없다면 scanObj는 비워져야함.
    }
    void StopPosition(bool _onoff)
    {
        if (_onoff)
        {
            rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY; // 두 제약 조건을 함께 설정
        }
        else
        {
            rigid.constraints = RigidbodyConstraints2D.None;
            rigid.freezeRotation = true;
        }
        
    }
}
