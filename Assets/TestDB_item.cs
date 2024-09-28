using MySql.Data.MySqlClient;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestDB_item : MonoBehaviour
{
    public Item item;
    public Image img_icon;
    public Image type_icon;
    public TextMeshProUGUI name_text;
    public TextMeshProUGUI title_text;
    public TextMeshProUGUI desc_text;

    void Start()
    {
        // DB���� ���� �������� �ҷ���
        item = DBConnector.LoadRandomItemFromDB();

        // ������ ���� ��� (����� �뵵)
        if (item != null)
        {
            Debug.Log($"������ �ε� ����: {item.itemName}, �ڵ�: {item.itemCode}, ����: {item.itemPrice}");

            img_icon.sprite = item.itemImage;
            type_icon.sprite = item.typeIcon;

            name_text.text = item.itemName;
            title_text.text = item.itemTitle;
            desc_text.text = item.itemDesc;
            // �̹��� ��� �� ���ϴ� ó�� �߰� ����
        }
        else
        {
            Debug.LogError("������ �ε� ����");
        }
    }

    // DB���� ���� �������� �������� �Լ�
    
}
