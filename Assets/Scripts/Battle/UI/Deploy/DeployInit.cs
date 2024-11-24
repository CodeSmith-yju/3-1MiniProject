using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DeployInit : MonoBehaviour
{
    private Tilemap tilemap; // Ÿ�ϸ�
    public GameObject spritePrefab; // ��ġ�� ��������Ʈ ������
    public GameObject deloy_obj; // Ŭ���� ���� �� �� ��ü

    public GameObject buff_Prefab;
    public Transform buff_Deploy_Tf;

    private int butt_Count = 4;

    public List<GameObject> highlight = new List<GameObject>();
    List<GameObject> buff = new List<GameObject>();

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
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
                if (tilemap.HasTile(cellPos))
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

        foreach (GameObject high in highlight)
        {
            if (high != null && high.activeSelf) 
                high.SetActive(false);
        }
    }

    private void Update()
    {
        if (BattleManager.Instance.buff_On)
        {
            SelectBuffTile();
            BattleManager.Instance.buff_On = false;
        }

        if (BattleManager.Instance._curphase == BattleManager.BattlePhase.Battle)
        {
            if (buff_Deploy_Tf.gameObject.activeSelf)
                buff_Deploy_Tf.gameObject.SetActive(false);
        }
    }


    private void SelectBuffTile()
    {
        List<GameObject> availableTiles = new List<GameObject>(highlight); // ���� ������ Ÿ�� ���纻

        for (int i = 0; i < Random.Range(1, butt_Count); i++)
        {
            if (availableTiles.Count == 0)
            {
                Debug.LogWarning("���� ������ Ÿ���� �����մϴ�.");
                break; // ���� ������ Ÿ���� ������ ����
            }

            // �������� Ÿ�� ����
            int randomIndex = Random.Range(0, availableTiles.Count);
            GameObject selectedTile = availableTiles[randomIndex];

            // ���õ� Ÿ���� ���� ��Ͽ� �߰��ϰ� ���纻���� ����
            buff.Add(selectedTile);
            availableTiles.RemoveAt(randomIndex);

            int randomBuff = Random.Range(0, butt_Count);

            // ���� ����
            CreateBuff(selectedTile, randomBuff);
        }
    }

    private void CreateBuff(GameObject buffTile, int buff_Index)
    {
        Vector3 pos = buffTile.transform.position;
        GameObject buff_Obj = Instantiate(buff_Prefab, pos, Quaternion.identity, buff_Deploy_Tf);

        buff_Obj.GetComponent<BuffInit>().Init(buff_Index);

    }
}
