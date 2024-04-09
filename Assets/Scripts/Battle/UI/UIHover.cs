using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private float hoverDuration = 1f; // ȣ�� �ִϸ��̼� ���� �ð�
    public float hover = 75f; // ȣ�� �� �̹����� �ö󰡴� ����

    private Vector3 originalPosition; // �̹����� ���� ��ġ
    private bool isHovering = false; // ȣ�� ������ ����
    private Coroutine hoverCoroutine;

    private void Start()
    {
        // �̹����� ���� ��ġ ����
        originalPosition = transform.position;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // ���콺�� �̹����� �����ϸ� ȣ�� �ִϸ��̼� ����
        if (hoverCoroutine != null)
        {
            StopCoroutine(hoverCoroutine);
        }
        hoverCoroutine = StartCoroutine(HoverAnimation(true));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // ���콺�� �̹������� ������ ȣ�� �ִϸ��̼� ����
        if (hoverCoroutine != null)
        {
            StopCoroutine(hoverCoroutine);
        }
        hoverCoroutine = StartCoroutine(HoverAnimation(false));
    }

    private IEnumerator HoverAnimation(bool isHovering)
    {
        // �̹����� �̹� ȣ�� ���̸� �ߺ� ���� ����
        if (this.isHovering == isHovering)
            yield break;

        this.isHovering = isHovering;

        Vector3 targetPosition;

        // ȣ�� �ִϸ��̼� ( ��ġ ��Ƽâ�� ����������, ������ â�� �ö������ )

        targetPosition = isHovering ? originalPosition + Vector3.up * hover : originalPosition;

        
       
        float elapsedTime = 0f;

        while (elapsedTime < hoverDuration)
        {
            // �ð��� ���� �̹����� �ε巴�� �̵���Ŵ
            transform.position = Vector3.Lerp(transform.position, targetPosition, elapsedTime / hoverDuration);
            elapsedTime += Time.deltaTime;
            yield return null; // ���� �����ӱ��� ���
        }

        // �ִϸ��̼� �Ϸ� �� ��Ȯ�� ��ġ�� ����
        transform.position = targetPosition;
    }
}
