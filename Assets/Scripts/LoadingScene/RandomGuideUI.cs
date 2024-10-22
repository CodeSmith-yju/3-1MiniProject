using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RandomGuideUI : MonoBehaviour
{
    public Sprite[] spRandomGuids;
    string[] strGuideDesc = {"�⺻ Ű ������ �ȳ��ص帳�ϴ�.",
        "��� ���� ����� �ȳ��ص帳�ϴ�.",
        "���� �̿� ����� �ȳ��ص帳�ϴ�.",
        "���¹̳� ���� ����� �ȳ��ص帳�ϴ�.",
        "��� ��ȭ ����� �ȳ��ص帳�ϴ�.",
    };

    public TextMeshProUGUI txtGuideDesc;

    public Sprite SetGuidImages()
    {
        // Random.Range�� 0���� numbers.Length - 1������ �ε����� ��ȯ
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
