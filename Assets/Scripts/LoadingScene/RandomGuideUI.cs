using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGuideUI : MonoBehaviour
{
    public Sprite[] spRandomGuids;

    public Sprite SetGuidImages()
    {
        // Random.Range�� 0���� numbers.Length - 1������ �ε����� ��ȯ
        int randomIndex = Random.Range(0, spRandomGuids.Length);
        return spRandomGuids[randomIndex];
    }

}
