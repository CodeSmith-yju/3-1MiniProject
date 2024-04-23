using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DeloyInit : MonoBehaviour
{
    private Tilemap tilemap; // Ÿ�ϸ�
    public GameObject spritePrefab; // ��ġ�� ��������Ʈ ������
    public GameObject deloy_obj; // Ŭ���� ���� �� �� ��ü
    public GameObject view_Obj; // ��ġ�� ������ ���ߴ� ��������Ʈ ������
    public List<GameObject> highlight = new List<GameObject>();
    Tilemap wall;

    private void OnEnable()
    {
        tilemap = GetComponent<Tilemap>();
        wall = GameObject.FindGameObjectWithTag("Wall").GetComponent<Tilemap>();
        PlaceSpritesInTilemap();
    }

    private void PlaceSpritesInTilemap()
    {
        // Ÿ�ϸ��� �� ũ�� ��������
        Vector3 cellSize = tilemap.cellSize;

        // Ÿ�ϸ��� ��� �� �ݺ�
        BoundsInt bounds = tilemap.cellBounds;
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int cellPos = new Vector3Int(x, y, 0);

                // ���� ���� Ÿ���� �ִ��� Ȯ��
                if (tilemap.HasTile(cellPos) && !wall.HasTile(cellPos))
                {
                    // ���� ���� ���� ��ǥ ���
                    Vector3 cellWorldPos = tilemap.GetCellCenterWorld(cellPos);

                    // ��������Ʈ�� ���� �߽ɿ� ��ġ
                    GameObject sprite = Instantiate(spritePrefab, cellWorldPos, Quaternion.identity);

                    // ��ġ�� ��������Ʈ�� ũ�⸦ ���� ũ�⿡ �°� ����
                    sprite.transform.localScale = new Vector3(cellSize.x, cellSize.y, 1f);

                    sprite.transform.SetParent(deloy_obj.transform);
                    highlight.Add(sprite);
                }
            }
        }
    }
}
