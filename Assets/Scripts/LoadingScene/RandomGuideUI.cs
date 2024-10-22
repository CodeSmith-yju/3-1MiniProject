using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RandomGuideUI : MonoBehaviour
{
    public Sprite[] spRandomGuids;
    string[] strGuideDesc = {
        "����Ű�ε� �̵��� �����մϴ�.",
        "��� �����Ϸ��� ������ �����ؾ��մϴ�.",
        "������ ��ǰ���� �������� ��ȯ�Ҷ����� ���ŵ˴ϴ�.",
        "���¹̳� �ܷ��� �����ϼ���.",
        "��ȭ�� �����Ҽ��� �����ϴ�.",
        "�̿��� �Ϻ� ����� Ʃ�丮���� �Ϸ��ؾ� �����ϴ�.",
    };

    public TextMeshProUGUI txtGuideDesc;

    public Sprite SetGuidImages()
    {
        // Random.Range�� 0���� numbers.Length - 1������ �ε����� ��ȯ
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
