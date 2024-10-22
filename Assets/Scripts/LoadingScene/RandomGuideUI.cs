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
        "상점의 물품들은 던전에서 귀환할때마다 갱신됩니다.",
        "스태미나 잔량에 주의하세요.",
        "강화는 많이할수록 좋습니다.",
        "이외의 일부 기능은 튜토리얼을 완료해야 열립니다.",
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
