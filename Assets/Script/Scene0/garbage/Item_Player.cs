using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item_Player : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image imgCharctor;
    [Header("ĳ���� �̸�")]
    [SerializeField] private TMP_InputField field_InputPlayerName;
    [SerializeField] private TextMeshProUGUI textPlayerName;
    public string playerName;

    [Header("ĳ���� ����")]
    [SerializeField] private TextMeshProUGUI textPlayerJob;// �ν�����â�� ����� ���� �̸��� ���Ӿ��� ǥ���ϱ����� ����
    [SerializeField] private string playerJob;// �ν�����â�� ����� �����̸��� �������� ������ �� ��.

    private void Awake()
    {
        field_InputPlayerName.onValueChanged.AddListener(OnInputValueChanged);
        textPlayerJob.text = playerJob;// ���� ���۵Ǹ� ������ �ν����Ϳ� ����� �����̸��� ǥ�õ�
        SelectPanelAlpha(0.1f);//OnUnselect();
    }

    private void OnInputValueChanged(string field_InputPlayerName) {
        playerName = field_InputPlayerName;
    }
    public string GetName() => playerName;

    public string GetJob() => textPlayerJob.text;//public string GetName() => textPlayerJob.text;
    public void SelectPanelAlpha(float alpha)
    {
        Color currentColor = imgCharctor.color;
        currentColor.a = alpha;
        imgCharctor.color = currentColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PanelSelectPlayer.single.OnFocusePlayerItem(this);
    }
}
