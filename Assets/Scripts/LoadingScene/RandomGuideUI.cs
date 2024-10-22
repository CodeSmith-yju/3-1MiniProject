using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGuideUI : MonoBehaviour
{
    public Sprite[] spRandomGuids;

    public Sprite SetGuidImages()
    {
        // Random.Range는 0부터 numbers.Length - 1까지의 인덱스를 반환
        int randomIndex = Random.Range(0, spRandomGuids.Length);
        return spRandomGuids[randomIndex];
    }

}
