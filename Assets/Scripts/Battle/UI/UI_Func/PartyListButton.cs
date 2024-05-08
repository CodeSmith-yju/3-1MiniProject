using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyListButton : MonoBehaviour
{
    public GameObject targetObject; // ���� ���� ��� ������Ʈ
    public float animationDuration = 1f; // �ִϸ��̼� ���� �ð�
    public float open = 200f; // ���� ���¿����� ����
    private Vector3 originalPosition; // ������Ʈ�� ���� ��ġ
    private bool isOpen = false; // ���� ���� ����
    private Coroutine toggleCoroutine;
    private bool isFirst = true;
    bool allInactive = true;
    private Transform[] list;

    private void Start()
    {
        originalPosition = targetObject.transform.position;
        list = GameObject.Find("Party_Inner").GetComponentsInChildren<Transform>();
    }

    private void OnEnable()
    {
        isFirst = true;
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
    }
}
