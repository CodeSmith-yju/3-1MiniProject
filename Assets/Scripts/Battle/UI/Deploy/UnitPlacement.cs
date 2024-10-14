using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class UnitPlacement : MonoBehaviour
{
    public Tilemap deployTilemap;
    public GameObject unitPrefab;
    public Image unit_Icon;
    public Image class_Icon;
    PlayerData data;

    private void Start()
    {
        deployTilemap = GameObject.FindGameObjectWithTag("Wait").GetComponent<Tilemap>();
    }

    public void InitList(GameObject unit, Sprite icon, PlayerData data)
    {
        this.unitPrefab = unit;
        this.unit_Icon.sprite = icon;
        this.data = data;
        //this.class_Icon.sprite = unit.GetComponent<Ally>().class_Icon;
    }

    public void DeployUnit()
        {
            BoundsInt bounds = deployTilemap.cellBounds;

            foreach (Vector3Int position in bounds.allPositionsWithin)
            {
                if (deployTilemap.HasTile(position))
                {
                    if (CanPlace(position))
                    {
                        Vector3 worldPos = deployTilemap.GetCellCenterWorld(position);
                        GameObject obj = Instantiate(unitPrefab, worldPos, Quaternion.identity);
                        obj.GetComponent<Ally>().InitStat(data.playerIndex);
                        BattleManager.Instance.deploy_Player_List.Add(obj);
                        gameObject.SetActive(false);
                    
                    if (BattleManager.Instance.dialogue.isTutorial && BattleManager.Instance.tutorial.isDeploy_Tutorial)
                    {
                        BattleManager.Instance.tutorial.EndTutorial(14);
                    }

                        return; // �� ���� �ϳ��� ���ָ� ��ġ�ϵ��� �����մϴ�.
                    }
                }
            }
        }

        private bool CanPlace(Vector3Int position)
        {
            Collider2D[] colliders = Physics2D.OverlapPointAll(deployTilemap.GetCellCenterWorld(position));

            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Player"))
                {
                    return false; // �̹� �ش� ��ġ�� ������ ������ ��ġ�� �� �����ϴ�.
                }
            }

            return true;
        }
}