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
        // DB에서 랜덤 아이템을 불러옴
        item = DBConnector.LoadRandomItemFromDB();

        // 아이템 정보 출력 (디버그 용도)
        if (item != null)
        {
            Debug.Log($"아이템 로드 성공: {item.itemName}, 코드: {item.itemCode}, 가격: {item.itemPrice}");

            img_icon.sprite = item.itemImage;
            type_icon.sprite = item.typeIcon;

            name_text.text = item.itemName;
            title_text.text = item.itemTitle;
            desc_text.text = item.itemDesc;
            // 이미지 출력 등 원하는 처리 추가 가능
        }
        else
        {
            Debug.LogError("아이템 로드 실패");
        }
    }

    // DB에서 랜덤 아이템을 가져오는 함수
    
}
