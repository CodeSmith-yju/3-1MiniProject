using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RandomGuideUI : MonoBehaviour
{
    public Sprite[] spRandomGuids;
    string[] strGuideDesc = {
        "방향키로도 이동이 가능합니다.",
        "장비를 개조하려면 장착을 해제해야합니다.",
        "상점 이용 방법을 안내해드립니다.",
        "스태미나 잔량에 주의하세요.",
        "장비 강화 방법을 안내해드립니다.",
        "일부 기능은 튜토리얼을 완료해야 열립니다.",
    };

    public TextMeshProUGUI txtGuideDesc;

    public Sprite SetGuidImages()
    {
        // Random.Range는 0부터 numbers.Length - 1까지의 인덱스를 반환
        int randomIndex = Random.Range(0, spRandomGuids.Length);
        if (GameMgr.single.firstStart)
        {
            GameMgr.single.firstStart = false;
            randomIndex = 5;
        }
        txtGuideDesc.text = strGuideDesc[randomIndex];
        return spRandomGuids[randomIndex];
    }
}
