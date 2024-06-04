using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator animator;

    public float speed;
    private float h, v;
    private string lastInputDirection = "None";

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        GetInput();
        UpdateDirection();
        Move();
        PlayAnimation();
    }

    private void GetInput()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
    }

    /*private void UpdateDirection()
    {
        if (h != 0)
            lastInputDirection = "Horizontal";
        else if (v != 0)
            lastInputDirection = "Vertical";
    }*/
    private void UpdateDirection()
    {
        // ���� �Է��� �ִ� ���
        if (h != 0)
        {
            lastInputDirection = "Horizontal";
        }
        // ���� �Է��� �ִ� ���
        else if (v != 0)
        {
            lastInputDirection = "Vertical";
        }
        // �� �� ���� ���
        else
        {
            lastInputDirection = "None";
        }
    }


    /*private void Move()
    {
        Vector2 moveVec = Vector2.zero;

        // �÷��̾ ���ÿ� ���� �� ���� �Է��� �־��� ��, �̵� ������ �����մϴ�.
        moveVec.x = h; // ���� �Է��� x �������� �����մϴ�.
        moveVec.y = v; // ���� �Է��� y �������� �����մϴ�.

        rigid.velocity = moveVec.normalized * speed; // ����ȭ�� �̵� ���͸� ����մϴ�.

        // �Է��� ���� �� ������ �Է� ������ �ʱ�ȭ�մϴ�.
        if (h == 0 && v == 0)
            lastInputDirection = "None";
    }*/

    private void Move()
    {
        Vector2 moveVec = Vector2.zero;

        // �÷��̾ ���� �� ���� �Է��� ��� �־��� ��, �̵� ������ �����մϴ�.
        if (lastInputDirection == "Horizontal")
        {
            moveVec.x = h;
            moveVec.y = v; // ���� �Է��� �־��� ��Ȳ���� ���� �Է��� ����մϴ�.
        }
        else if (lastInputDirection == "Vertical")
        {
            moveVec.x = h; // ���� �Է��� �־��� ��Ȳ���� ���� �Է��� ����մϴ�.
            moveVec.y = v;
        }

        rigid.velocity = moveVec * speed;

        // �Է��� ���� �� ������ �Է� ������ �ʱ�ȭ�մϴ�.
        if (h == 0 && v == 0)
            lastInputDirection = "None";
    }


    private void PlayAnimation()
    {
        if (animator.GetInteger("hAxisRaw") != (int)h)
        {
            animator.SetInteger("hAxisRaw", (int)h);
            animator.SetBool("isChange", true);
        }
        else if (animator.GetInteger("vAxisRaw") != (int)v)
        {
            animator.SetInteger("vAxisRaw", (int)v);
            animator.SetBool("isChange", true);
        }
        else
        {
            animator.SetBool("isChange", false);
        }
    }
}
