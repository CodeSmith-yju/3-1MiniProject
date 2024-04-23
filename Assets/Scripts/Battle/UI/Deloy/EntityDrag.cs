using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class EntityDrag : MonoBehaviour
{
    private Camera cam;
    private Vector3 drag;
    private Vector3 initPos;
    private bool isDragging = false;
    public Tilemap deloy;
    Tilemap wall;

    private void Start()
    {
        cam = Camera.main;
        deloy = GameObject.FindGameObjectWithTag("Deloy").GetComponent<Tilemap>();
        wall = GameObject.FindGameObjectWithTag("Wall").GetComponent<Tilemap>();
    }

    private void OnMouseDown()
    {
        if (BattleManager.Instance._curphase == BattleManager.BattlePhase.Deploy)
        {
            isDragging = true;
            initPos = transform.position;
            drag = transform.position - cam.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    private void Update()
    {
        if (BattleManager.Instance._curphase == BattleManager.BattlePhase.Deploy)
        {
            if (deloy == null)
            {
                deloy = GameObject.FindGameObjectWithTag("Deloy").GetComponent<Tilemap>();
            }
        }    
    }

    private void OnMouseDrag()
    {
        if (BattleManager.Instance._curphase == BattleManager.BattlePhase.Deploy)
        {
            if (isDragging)
            {
                NavMeshAgent nav = GetComponent<NavMeshAgent>();

                nav.enabled = false;

                var curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
                var curPosition = cam.ScreenToWorldPoint(curScreenSpace) + drag;
                transform.position = curPosition;
            }
        }
        
    }

    private void OnMouseUp()
    {
        if (BattleManager.Instance._curphase == BattleManager.BattlePhase.Deploy)
        {
            if (isDragging)
            {
                Vector3 curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
                Vector3 curPosition = cam.ScreenToWorldPoint(curScreenSpace) + drag;

                // ���� ����� ���� �߽� ��ǥ�� ���մϴ�.
                Vector3Int cellPosition = deloy.WorldToCell(curPosition);
                Vector3 snappedPosition = deloy.GetCellCenterWorld(cellPosition);

                NavMeshAgent nav = GetComponent<NavMeshAgent>();

                nav.enabled = true;


                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                foreach (GameObject player in players)
                {
                    Vector3 playerCellPosition = deloy.WorldToCell(player.transform.position);
                    if (playerCellPosition == cellPosition)
                    {
                        // ��ġ�� ���� �÷��̾� �±׸� ���� ������Ʈ�� ������ �ش� ������Ʈ�� ��ġ�� �ٲߴϴ�.
                        Vector3 temp = initPos;
                        transform.position = player.transform.position;
                        player.transform.position = temp;
                    }
                }


                if (!deloy.HasTile(cellPosition) || wall.HasTile(cellPosition))
                {
                    transform.position = initPos;
                }
                else
                {
                    transform.position = snappedPosition;
                }

                isDragging = false;
            }
        }
    }
}
