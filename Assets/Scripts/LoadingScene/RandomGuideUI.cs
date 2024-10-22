using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RandomGuideUI : MonoBehaviour
{
    public Sprite[] spRandomGuids;
    string[] strGuideDesc = {"기본 키 사용법을 안내해드립니다.",
        "장비 개조 방법을 안내해드립니다.",
        "상점 이용 방법을 안내해드립니다.",
        "스태미나 관리 방법을 안내해드립니다.",
        "장비 강화 방법을 안내해드립니다.",
    };

    public TextMeshProUGUI txtGuideDesc;

    public Sprite SetGuidImages()
    {
        // Random.Range는 0부터 numbers.Length - 1까지의 인덱스를 반환
        int randomIndex = Random.Range(0, spRandomGuids.Length);
        if (GameMgr.single.firstStart)
        {
            GameMgr.single.firstStart = false;
            randomIndex = 0;
        }
        txtGuideDesc.text = strGuideDesc[randomIndex];
        return spRandomGuids[randomIndex];
    }
}
