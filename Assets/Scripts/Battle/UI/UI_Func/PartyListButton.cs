using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyListButton : MonoBehaviour
{
    /*public GameObject targetObject; // ���� ���� ��� ������Ʈ
    public float animationDuration = 1f; // �ִϸ��̼� ���� �ð�
    public float open = 200f; // ���� ���¿����� ����
    private Vector3 originalPosition; // ������Ʈ�� ���� ��ġ
    private bool isOpen = false; // ���� ���� ����
    private Coroutine toggleCoroutine;
    private bool isFirst = true;*/

    /*private void Start()
    {
        originalPosition = targetObject.transform.position;
    }

    private void Update()
    {
        if (BattleManager.Instance._curphase == BattleManager.BattlePhase.Deploy && isFirst)
        {
            Toggle();
            isFirst = false;
        }
    }

    public void Toggle()
    {
        if (toggleCoroutine != null)
        {
            return;
        }
        toggleCoroutine = StartCoroutine(ToggleAnimation());
    }

    private IEnumerator ToggleAnimation()
    {
        float elapsedTime = 0f;
        Vector3 targetPosition;

        if (isOpen)
        {
            targetPosition = originalPosition;
        }
        else
        {
            targetPosition = originalPosition + Vector3.right * open;
        }

        while (elapsedTime < animationDuration)
        {
            targetObject.transform.position = Vector3.Lerp(targetObject.transform.position, targetPosition, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        targetObject.transform.position = targetPosition;
        isOpen = !isOpen;

        toggleCoroutine = null;
    }*/

    public GameObject obj_Side;
    public Vector2 vec_From;
    public Vector2 vec_To;
    public float f_Set_Timer;
    bool isOpen = false;

    private void OnEnable()
    {
        isOpen = false;
        Clicked_Side();
    }


    public void Clicked_Side()
    {
        if (!isOpen)
        {
            Open_Side();
        }
        else
        {
            Close_Side();
        }
    }

    private void Open_Side()
    {
        StartCoroutine(Open_Side_Co());
    }

    private IEnumerator Open_Side_Co()
    {
        Debug.Log("������ ��");
        float timer = 0;
        while (timer < f_Set_Timer)
        {
            timer += Time.deltaTime;
            Vector2 temp = Vector2.Lerp(vec_From, vec_To, timer / f_Set_Timer);
            yield return null;
            obj_Side.GetComponent<RectTransform>().anchoredPosition = temp;
        }
        obj_Side.GetComponent<RectTransform>().anchoredPosition = vec_To;

        isOpen = true;
        yield break;
    }

    private void Close_Side()
    {
        StartCoroutine(Close_Side_Co());
    }

    private IEnumerator Close_Side_Co()
    {
        float timer = 0;
        while (timer < f_Set_Timer)
        {
            timer += Time.deltaTime;
            Vector2 temp = Vector2.Lerp(vec_To, vec_From, timer / f_Set_Timer);
            yield return null;
            obj_Side.GetComponent<RectTransform>().anchoredPosition = temp;
        }
        obj_Side.GetComponent<RectTransform>().anchoredPosition = vec_From;

        isOpen = false;
        yield break;
    }


}
