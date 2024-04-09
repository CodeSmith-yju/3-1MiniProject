using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    public float activationDistance = 0.1f; // Ȱ��ȭ �Ÿ�

    private void Update()
    {
        // ���� ���̶���Ʈ ������Ʈ�� ��ġ
        Vector3 highlightPosition = transform.position;

        // ���� ����� �÷��̾� ã��
        GameObject closestPlayer = FindClosestPlayer(highlightPosition);

        // Ȱ��ȭ �Ÿ� ���� ���� ��쿡�� �ش� ���̶���Ʈ�� Ȱ��ȭ
        if (closestPlayer != null && Vector3.Distance(highlightPosition, closestPlayer.transform.position) <= activationDistance)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private GameObject FindClosestPlayer(Vector3 position)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject closestPlayer = null;
        float minDistance = float.MaxValue;

        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(position, player.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestPlayer = player;
            }
        }

        return closestPlayer;
    }
}
