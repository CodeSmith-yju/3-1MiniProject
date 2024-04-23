using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class UnitPlacement : MonoBehaviour
{
    public Tilemap deployTilemap;
    public GameObject unitPrefab;
    public Image unit_Icon;

    private void Start()
    {
        UnitPlacement[] unit = FindObjectsOfType<UnitPlacement>();
        deployTilemap = GameObject.FindGameObjectWithTag("Wait").GetComponent<Tilemap>();
    }

    public void InitList(GameObject unit, Sprite icon)
    {
        this.unitPrefab = unit;
        this.unit_Icon.sprite = icon;
    }



    public void DeployUnit()
        {
            BoundsInt bounds = deployTilemap.cellBounds;
            TileBase[] allTiles = deployTilemap.GetTilesBlock(bounds);

            foreach (Vector3Int position in bounds.allPositionsWithin)
            {
                if (deployTilemap.HasTile(position))
                {
                    if (CanPlace(position))
                    {
                        Vector3 worldPos = deployTilemap.GetCellCenterWorld(position);
                        GameObject obj = Instantiate(unitPrefab, worldPos, Quaternion.identity);
                        BattleManager.Instance.deloy_Player_List.Add(obj);
                        gameObject.SetActive(false);
                        
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