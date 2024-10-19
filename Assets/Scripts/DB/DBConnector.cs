using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using static UnityEditor.Progress;

public class DBConnector : MonoBehaviour
{
    [Header("DBConnect")]
    [SerializeField] private string IP = "127.0.0.1";
    [SerializeField] private int PORT = 3308;
    [SerializeField] private string ID = string.Empty;
    [SerializeField] private string PW = string.Empty;
    [SerializeField] private string DB_NAME = string.Empty;

    [Header("Data")]
    [SerializeField] private int uid = 0;

    private static DBConnector single;

    private static MySqlConnection _connection = null;
    private static MySqlConnection connection
    {
        get
        {
            if (_connection == null)
            {
                try
                {
                    //string formatSql = $"Server={single.IP}; Port={single.PORT}; Database={single.DB_NAME}; UserId={single.ID}; Password={single.PW}";
                    string formatSql = $"Server={single.IP}; Port={single.PORT}; Database={single.DB_NAME}; UserId={single.ID}; Password={single.PW}; Charset=utf8mb4";
                    _connection = new MySqlConnection(formatSql);
                }
                catch (MySqlException e)
                {
                    Debug.LogError(e);
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                }
            }

            return _connection;

        }
    }

    private void Awake()
    {
        single = this;
        DontDestroyOnLoad(single);
    }



    private static bool m_OnChange(string query)
    {
        bool result = false;
        try
        {
            MySqlCommand sqlCommand = new MySqlCommand();
            sqlCommand.Connection = connection;
            sqlCommand.CommandText = query;

            connection.Open();

            sqlCommand.ExecuteNonQuery();

            connection.Close();

            result = true;
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }

        connection.Close();
        return result;
    }

    private static DataSet m_OnLoad(string tableName, string query)
    {
        DataSet ds = null; ;
        try
        {
            connection.Open();   //DB 연결

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = query;

            MySqlDataAdapter sd = new MySqlDataAdapter(cmd);
            ds = new DataSet();
            sd.Fill(ds, tableName);
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }

        connection.Close();  //DB 연결 해제
        return ds;
    }

    /// <summary>
    /// 데이터 검색
    /// </summary>
    /// <param name="tableName">검색할 테이블</param>
    /// <param name="field">검색할 필드 (입력하지 않을 경우 전체 로드)</param>
    /// <param name="condition">조건</param>
    /// <returns></returns>
    public static XmlNodeList Select(string tableName, string field = "*", string condition = "")
    {
        DataSet dataSet = m_OnLoad(tableName, $"SELECT {field} FROM {tableName} {condition}");

        if (dataSet == null)
            return null;

        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(dataSet.GetXml());

        return xmlDocument.GetElementsByTagName(tableName);
    }

    /// <summary>
    /// 데이터 입력
    /// </summary>
    /// <param name="tableName">입력할 테이블</param>
    /// <param name="fieldName">입력할 필드 이름</param>
    /// <param name="value">입력할 값</param>
    /// <returns></returns>
    public static bool Insert(string tableName, string fieldName, string value)
    {
        return m_OnChange($"INSERT INTO {tableName} ({fieldName}) VALUES ('{value}')");
    }

    /// <summary>
    /// 데이터 입력 (순차적으로 전부 입력됨)
    /// </summary>
    /// <param name="tableName">입력할 테이블</param>
    /// <param name="values">입력할 값 (칼럼 순서대로 적용됨)</param>
    /// <returns></returns>
    public static bool Insert(string tableName, string[] values)
    {
        string strValues = string.Empty;

        foreach (string value in values)
        {
            if (strValues.Length > 0)
                strValues += ", ";
            strValues += $"'{value}'";
        }

        return m_OnChange($"INSERT INTO {tableName} VALUES ({strValues})");
    }

    /// <summary>
    /// 레코드 갱신
    /// </summary>
    /// <param name="tableName">입력할 테이블</param>
    /// <param name="fieldName">입력할 필드 이름</param>
    /// <param name="value">입력할 값</param>
    /// <param name="condition">조건</param>
    /// <returns></returns>
    public static bool Update(string tableName, string fieldName, string value, string condition)
    {
        return m_OnChange($"UPDATE {tableName} SET {fieldName}='{value}' WHERE {condition}");
    }

    /// <summary>
    /// 레코드 제거
    /// </summary>
    /// <param name="tableName">제거할 레코드가 포함된 테이블</param>
    /// <param name="condition">조건</param>
    /// <returns></returns>
    public static bool Delete(string tableName, string condition)
    {
        return m_OnChange($"DELETE FROM {tableName} WHERE {condition}");
    }

    /// <summary>
    /// userinfo 테이블에서 ID와 PW가 일치하는지 확인하는 메서드
    /// </summary>
    /// <param name="userID">사용자가 입력한 ID</param>
    /// <param name="userPW">사용자가 입력한 PW</param>
    /// <returns>로그인 성공 여부</returns>
    public static bool SelectUser(string userID, string userPW)
    {
        string query = $"SELECT * FROM userinfo WHERE id = '{userID}' AND pw = '{userPW}'";
        DataSet ds = m_OnLoad("userinfo", query);

        // DataSet에 데이터가 있다면 로그인 성공
        if (ds != null && ds.Tables["userinfo"].Rows.Count > 0)
        {
            return true;  // 로그인 성공
        }

        return false;  // 로그인 실패
    }
    public static bool InsertUser(string userID, string userPW)
    {
        // 먼저 동일한 ID가 있는지 확인 (중복 검사)
        DataSet ds = m_OnLoad("userinfo", $"SELECT * FROM userinfo WHERE id = '{userID}'");

        if (ds != null && ds.Tables["userinfo"].Rows.Count > 0)
        {
            Debug.LogError("이미 존재하는 ID입니다.");
            return false;  // 중복 ID가 있을 경우 false 반환
        }

        // 중복되지 않는다면 회원가입 진행
        string query = $"INSERT INTO userinfo (id, pw) VALUES ('{userID}', '{userPW}')";
        return m_OnChange(query);  // 삽입 성공 여부 반환
    }

    //
    // Unity -> DB
    // 아이템 정보를 DB에 저장하는 함수 (이미지 포함)
    /*public static bool InsertItemToDB(Item item)
    {
        // 아이템 이미지와 타입 아이콘을 바이트 배열로 변환
        byte[] itemImageBytes = ConvertSpriteToBytes(item.itemImage);
        byte[] typeIconBytes = ConvertSpriteToBytes(item.typeIcon);

        // SQL 쿼리
        string query = "INSERT INTO item (itemCode, itemName, itemType, itemTitle, itemDesc, itemPrice, itemPower, itemStack, modifyStack, itemImg, typeIcon) " +
                       "VALUES (@itemCode, @itemName, @itemType, @itemTitle, @itemDesc, @itemPrice, @itemPower, @itemStack, @modifyStack, @itemImg, @typeIcon)";

        MySqlCommand cmd = new MySqlCommand(query, connection);

        // 쿼리에 파라미터 추가
        cmd.Parameters.AddWithValue("@itemCode", item.itemCode);
        cmd.Parameters.AddWithValue("@itemName", item.itemName);
        cmd.Parameters.AddWithValue("@itemType", item.itemType.ToString());  // Enum을 문자열로 저장
        cmd.Parameters.AddWithValue("@itemTitle", item.itemTitle);
        cmd.Parameters.AddWithValue("@itemDesc", item.itemDesc);
        cmd.Parameters.AddWithValue("@itemPrice", item.itemPrice);
        cmd.Parameters.AddWithValue("@itemPower", item.itemPower);
        cmd.Parameters.AddWithValue("@itemStack", item.itemStack);
        cmd.Parameters.AddWithValue("@modifyStack", item.modifyStack);
        cmd.Parameters.AddWithValue("@itemImg", itemImageBytes);  // BLOB 데이터
        cmd.Parameters.AddWithValue("@typeIcon", typeIconBytes);  // BLOB 데이터

        try
        {
            connection.Open();
            cmd.ExecuteNonQuery();
            return true;  // 성공
        }
        catch (MySqlException ex)
        {
            Debug.LogError("DB Insert Error: " + ex.Message);
            return false;  // 실패
        }
        finally
        {
            connection.Close();
        }
    }*/
    public static bool InsertItemToDB(Item item)//최종수정본
    {
        byte[] itemImageBytes = ConvertSpriteToBytes(item.itemImage);
        byte[] typeIconBytes = ConvertSpriteToBytes(item.typeIcon);

        // 데이터 길이 로그로 출력하여 저장 전후 확인
        Debug.Log("Item Image Bytes Length (Before Saving): " + (itemImageBytes != null ? itemImageBytes.Length : 0));
        Debug.Log("Type Icon Bytes Length (Before Saving): " + (typeIconBytes != null ? typeIconBytes.Length : 0));

        string query = "INSERT INTO item (itemCode, itemName, itemType, itemTitle, itemDesc, itemPrice, itemPower, itemStack, modifyStack, itemImg, typeIcon) " +
                       "VALUES (@itemCode, @itemName, @itemType, @itemTitle, @itemDesc, @itemPrice, @itemPower, @itemStack, @modifyStack, @itemImg, @typeIcon)";

        MySqlCommand cmd = new MySqlCommand(query, connection);

        // 파라미터 추가
        cmd.Parameters.AddWithValue("@itemCode", item.itemCode);
        cmd.Parameters.AddWithValue("@itemName", item.itemName);
        cmd.Parameters.AddWithValue("@itemType", item.itemType.ToString());
        cmd.Parameters.AddWithValue("@itemTitle", item.itemTitle);
        cmd.Parameters.AddWithValue("@itemDesc", item.itemDesc);
        cmd.Parameters.AddWithValue("@itemPrice", item.itemPrice);
        cmd.Parameters.AddWithValue("@itemPower", item.itemPower);
        cmd.Parameters.AddWithValue("@itemStack", item.itemStack);
        cmd.Parameters.AddWithValue("@modifyStack", item.modifyStack);
        cmd.Parameters.AddWithValue("@itemImg", itemImageBytes);
        cmd.Parameters.AddWithValue("@typeIcon", typeIconBytes);

        try
        {
            connection.Open();
            cmd.ExecuteNonQuery();
            return true;
        }
        catch (MySqlException ex)
        {
            Debug.LogError("DB Insert Error: " + ex.Message);
            return false;
        }
        finally
        {
            connection.Close();
        }
    }

    // Sprite를 byte 배열로 변환하는 함수
    /*private static byte[] ConvertSpriteToBytes(Sprite sprite)
    {
        CheckTextureReadable(sprite);
        Texture2D texture = sprite.texture;
        return texture.EncodeToPNG();  // PNG 형식으로 변환
    }*/
    /*private static byte[] ConvertSpriteToBytes(Sprite sprite)
    {
        // 압축된 텍스처인지 확인
        if (GraphicsFormatUtility.IsCompressedFormat(sprite.texture.graphicsFormat))
        {
            Debug.LogWarning($"Texture '{sprite.name}' is in a compressed format and cannot be converted to bytes.");
            return null; // 압축된 텍스처를 건너뛰도록 처리
        }

        // 텍스처가 읽기 가능한지 확인
        CheckTextureReadable(sprite);

        // PNG 형식으로 텍스처를 변환
        Texture2D texture = sprite.texture;
        return texture.EncodeToPNG();  // PNG 형식으로 변환
    }*/
    private static byte[] ConvertSpriteToBytes(Sprite sprite)//최종 수정본
    {
        // 압축된 텍스처인지 확인
        if (GraphicsFormatUtility.IsCompressedFormat(sprite.texture.graphicsFormat))
        {
            Debug.LogWarning($"Texture '{sprite.name}' is in a compressed format and cannot be converted to bytes.");
            // 압축된 텍스처도 처리할 수 있도록 예외적인 경우라도 PNG로 변환을 시도
            Texture2D decompressedTexture = new Texture2D(sprite.texture.width, sprite.texture.height);
            Graphics.CopyTexture(sprite.texture, decompressedTexture);
            return decompressedTexture.EncodeToPNG();
        }

        // 텍스처가 읽기 가능한지 확인
        CheckTextureReadable(sprite);

        // PNG 형식으로 텍스처를 변환
        Texture2D texture = sprite.texture;
        return texture.EncodeToPNG();  // PNG 형식으로 변환
    }

    /*static void CheckTextureReadable(Sprite sprite)
    {
        if (!sprite.texture.isReadable)
        {
            Debug.LogError($"Texture '{sprite.name}' is not readable.");
        }
        else
        {
            Debug.Log($"Texture '{sprite.name}' is readable.");
        }
    }*/
    private static void CheckTextureReadable(Sprite sprite)//최종 수정본
    {
        if (!sprite.texture.isReadable)
        {
            Debug.LogError($"Texture '{sprite.name}' is not readable. Please enable Read/Write in the texture import settings.");
            throw new UnityException($"Texture '{sprite.name}' is not readable.");
        }
    }
    //
    // DB -> Unity
    // BLOB 데이터를 Sprite로 변환하는 함수
    /*public static Sprite ConvertBytesToSprite(byte[] imageBytes)
    {
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(imageBytes);  // 이미지 데이터를 텍스처로 로드

        // 텍스처를 Sprite로 변환
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }*/
    public static Sprite ConvertBytesToSprite(byte[] imageBytes)//최종 수정본
    {
        // 이미지 데이터가 유효한지 먼저 확인
        if (imageBytes == null || imageBytes.Length == 0)
        {
            Debug.LogError("Image byte data is invalid or empty.");
            return null;
        }

        // 바이트 배열에서 텍스처를 생성
        Texture2D texture = new Texture2D(2, 2);
        if (!texture.LoadImage(imageBytes))
        {
            Debug.LogError("Failed to load image from byte data.");
            return null;
        }

        // 텍스처를 Sprite로 변환
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    public static int SelectUserAndGetUID(string userID, string userPW)
    {
        int uid = 0;
        string query = $"SELECT uID FROM userinfo WHERE id = '{userID}' AND pw = '{userPW}'";
        DataSet ds = m_OnLoad("userinfo", query);

        // 데이터가 존재한다면 uID를 반환
        if (ds != null && ds.Tables["userinfo"].Rows.Count > 0)
        {
            uid = Convert.ToInt32(ds.Tables["userinfo"].Rows[0]["uID"]);
        }

        return uid;  // 일치하는 사용자가 없으면 0 반환
    }

    // DB에서 아이템 데이터를 읽는 함수
    /*public static Item LoadItemFromDB(int itemCode)
    {
        string query = $"SELECT * FROM item WHERE itemCode = {itemCode}";
        MySqlCommand cmd = new MySqlCommand(query, connection);

        connection.Open();
        MySqlDataReader reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            Item item = new Item();
            item.itemCode = reader.GetInt32("itemCode");
            item.itemName = reader.GetString("itemName");
            item.itemType = (Item.ItemType)Enum.Parse(typeof(Item.ItemType), reader.GetString("itemType"));
            item.itemTitle = reader.GetString("itemTitle");
            item.itemDesc = reader.GetString("itemDesc");
            item.itemPrice = reader.GetInt32("itemPrice");
            item.itemPower = reader.GetFloat("itemPower");
            item.itemStack = reader.GetInt32("itemStack");
            item.modifyStack = reader.GetInt32("modifyStack");

            // 이미지 데이터를 BLOB에서 가져와 Sprite로 변환
            byte[] itemImgBytes = (byte[])reader["itemImg"];
            item.itemImage = ConvertBytesToSprite(itemImgBytes);

            byte[] typeIconBytes = (byte[])reader["typeIcon"];
            item.typeIcon = ConvertBytesToSprite(typeIconBytes);

            connection.Close();
            return item;
        }

        connection.Close();
        return null;
    }*/
    public static Item LoadItemFromDB(int itemCode)//최종 수정본
    {
        string query = $"SELECT * FROM item WHERE itemCode = {itemCode}";
        MySqlCommand cmd = new MySqlCommand(query, connection);

        connection.Open();
        MySqlDataReader reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            Item item = new Item();
            item.itemCode = reader.GetInt32("itemCode");
            item.itemName = reader.GetString("itemName");
            item.itemType = (Item.ItemType)Enum.Parse(typeof(Item.ItemType), reader.GetString("itemType"));
            item.itemTitle = reader.GetString("itemTitle");
            item.itemDesc = reader.GetString("itemDesc");
            item.itemPrice = reader.GetInt32("itemPrice");
            item.itemPower = reader.GetFloat("itemPower");
            item.itemStack = reader.GetInt32("itemStack");
            item.modifyStack = reader.GetInt32("modifyStack");

            // 이미지 데이터를 BLOB에서 가져와 Sprite로 변환
            byte[] itemImgBytes = (byte[])reader["itemImg"];
            item.itemImage = ConvertBytesToSprite(itemImgBytes);

            byte[] typeIconBytes = (byte[])reader["typeIcon"];
            item.typeIcon = ConvertBytesToSprite(typeIconBytes);

            // 불러온 바이트 길이 로그로 확인
            Debug.Log("Item Image Bytes Length (After Loading): " + (itemImgBytes != null ? itemImgBytes.Length : 0));
            Debug.Log("Type Icon Bytes Length (After Loading): " + (typeIconBytes != null ? typeIconBytes.Length : 0));

            connection.Close();
            return item;
        }

        connection.Close();
        return null;
    }

    public static Item LoadRandomItemFromDB()
    {
        // SQL 쿼리: 아이템 테이블에서 랜덤하게 하나의 아이템 선택
        string query = "SELECT * FROM item ORDER BY RAND() LIMIT 1";

        MySqlCommand cmd = new MySqlCommand(query, DBConnector.connection);

        try
        {
            DBConnector.connection.Open();
            MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                // 새로운 아이템 생성 및 데이터 할당
                Item loadedItem = new Item
                {
                    itemCode = reader.GetInt32("itemCode"),
                    itemName = reader.GetString("itemName"),
                    itemType = (Item.ItemType)System.Enum.Parse(typeof(Item.ItemType), reader.GetString("itemType")),
                    itemTitle = reader.GetString("itemTitle"),
                    itemDesc = reader.GetString("itemDesc"),
                    itemPrice = reader.GetInt32("itemPrice"),
                    itemPower = reader.GetFloat("itemPower"),
                    itemStack = reader.GetInt32("itemStack"),
                    modifyStack = reader.GetInt32("modifyStack")
                };

                // 이미지 데이터 처리 (BLOB 데이터 -> Sprite 변환)
                byte[] itemImgBytes = (byte[])reader["itemImg"];
                loadedItem.itemImage = DBConnector.ConvertBytesToSprite(itemImgBytes);

                byte[] typeIconBytes = (byte[])reader["typeIcon"];
                loadedItem.typeIcon = DBConnector.ConvertBytesToSprite(typeIconBytes);

                return loadedItem;  // 로드 성공 시 아이템 반환
            }

            reader.Close();
        }
        catch (MySqlException ex)
        {
            Debug.LogError("DB 에러: " + ex.Message);
        }
        finally
        {
            DBConnector.connection.Close();
        }

        return null;  // 로드 실패 시 null 반환
    }
    public static bool LoadItemByCodeFromDB(int itemCode,ref Sprite _itemicon, ref Sprite _typeicon)
    {
        /*
                // SQL 쿼리: itemCode와 일치하는 아이템 선택
                string query = "SELECT * FROM item WHERE itemCode = @itemCode";

                MySqlCommand cmd = new MySqlCommand(query, DBConnector.connection);
                cmd.Parameters.AddWithValue("@itemCode", itemCode); // 매개변수 추가

                try
                {
                    DBConnector.connection.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        // 새로운 아이템 생성 및 데이터 할당
                        Item loadedItem = new Item
                        {
                            itemCode = reader.GetInt32("itemCode"),
                            itemName = reader.GetString("itemName"),
                            itemType = (Item.ItemType)System.Enum.Parse(typeof(Item.ItemType), reader.GetString("itemType")),
                            itemTitle = reader.GetString("itemTitle"),
                            itemDesc = reader.GetString("itemDesc"),
                            itemPrice = reader.GetInt32("itemPrice"),
                            itemPower = reader.GetFloat("itemPower"),
                            itemStack = reader.GetInt32("itemStack"),
                            modifyStack = reader.GetInt32("modifyStack")
                        };

                        // 이미지 데이터 처리 (BLOB 데이터 -> Sprite 변환)
                        byte[] itemImgBytes = (byte[])reader["itemImg"];
                        loadedItem.itemImage = DBConnector.ConvertBytesToSprite(itemImgBytes);

                        byte[] typeIconBytes = (byte[])reader["typeIcon"];
                        loadedItem.typeIcon = DBConnector.ConvertBytesToSprite(typeIconBytes);

                        _itemicon = loadedItem.itemImage;
                        _typeicon = loadedItem.typeIcon;

                        return true;  // 로드 성공 시 아이템 반환
                    }

                    reader.Close();
                }
                catch (MySqlException ex)
                {
                    Debug.LogError("DB 에러: " + ex.Message);
                }
                finally
                {
                    DBConnector.connection.Close();
                }

                return false;  // 로드 실패 시 null 반환
        */
        _itemicon = ItemResources.instance.itemSpriteRS[itemCode];
        _typeicon = ItemResources.instance.SetTpyeIcon(new Item().GenerateRandomItem(itemCode));
        return true;
    }


    /*public static void SaveToDB(SaveData saveData, int userID)
    {
        // SaveData를 JSON 문자열로 변환
        string saveJson = JsonUtility.ToJson(saveData);

        // SaveTable에 JSON 데이터를 삽입 또는 업데이트
        string query = $"INSERT INTO SaveTable (uID, saveData) VALUES (@userID, @saveData) " +
                       "ON DUPLICATE KEY UPDATE saveData = @saveData";

        MySqlCommand cmd = new MySqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@userID", userID);
        cmd.Parameters.AddWithValue("@saveData", saveJson);

        try
        {
            connection.Open();
            cmd.ExecuteNonQuery();
            Debug.Log("Save Success: Data saved to DB.");
        }
        catch (MySqlException ex)
        {
            Debug.LogError("DB Save Error: " + ex.Message);
        }
        finally
        {
            connection.Close();
        }
    }*/
    public static void SaveToDB(SaveData saveData, int userID)
    {
        if (userID <= 0) // 유효한 uID가 아닌 경우 예외 처리
        {
            Debug.LogError("Invalid UserID: " + userID);
            return;
        }
        // SaveData를 JSON 문자열로 변환
        string saveJson = JsonUtility.ToJson(saveData);

        // SaveTable에서 기존의 saveData 항목만 업데이트
        string query = $"UPDATE SaveTable SET saveData = @saveData WHERE uID = @userID";

        // 만약 해당 유저의 데이터가 없으면 INSERT로 데이터를 추가
        string insertQuery = $"INSERT INTO SaveTable (uID, saveData) VALUES (@userID, @saveData) " +
                             "ON DUPLICATE KEY UPDATE saveData = @saveData";

        MySqlCommand cmd = new MySqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@userID", userID);
        cmd.Parameters.AddWithValue("@saveData", saveJson);

        try
        {
            connection.Open();

            // 해당 유저의 데이터가 있는지 먼저 검사
            int rowsAffected = cmd.ExecuteNonQuery();

            // 업데이트된 행이 없다면(즉, 해당 유저 데이터가 없으면), INSERT로 추가
            if (rowsAffected == 0)
            {
                MySqlCommand insertCmd = new MySqlCommand(insertQuery, connection);
                insertCmd.Parameters.AddWithValue("@userID", userID);
                insertCmd.Parameters.AddWithValue("@saveData", saveJson);
                insertCmd.ExecuteNonQuery();
            }

            Debug.Log("Save Success: Data saved or updated in DB.");
        }
        catch (MySqlException ex)
        {
            Debug.LogError("DB Save Error: " + ex.Message);
        }
        finally
        {
            connection.Close();
        }
    }

    public static SaveData LoadFromDB(int userID)
    {
        string query = $"SELECT saveData FROM SaveTable WHERE uID = @userID";
        MySqlCommand cmd = new MySqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@userID", userID);

        try
        {
            connection.Open();
            MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                string saveJson = reader.GetString("saveData");
                SaveData saveData = JsonUtility.FromJson<SaveData>(saveJson);
                return saveData;
            }
            else
            {
                Debug.LogWarning("No save data found for userID: " + userID);
            }
        }
        catch (MySqlException ex)
        {
            Debug.LogError("DB Load Error: " + ex.Message);
        }
        finally
        {
            connection.Close();
        }

        return null; // 데이터를 찾지 못했을 때
    }
    public static bool SaveDataSearch(int uid)
    {
        string query = $"SELECT COUNT(*) FROM saveTable WHERE uID = {uid}";  // uid로 검색

        // DB 연결 및 쿼리 실행 (예: MySQL을 사용한다고 가정)
        MySqlCommand cmd = new MySqlCommand(query, DBConnector.connection);
        DBConnector.connection.Open();

        try
        {
            int count = Convert.ToInt32(cmd.ExecuteScalar());
            return count > 0;  // 해당 uid로 저장된 데이터가 있으면 true, 없으면 false
        }
        catch (Exception ex)
        {
            Debug.LogError("DB 에러: " + ex.Message);
            return false;
        }
        finally
        {
            DBConnector.connection.Close();
        }
    }
    public static int GetUID()
    {
        return DBConnector.single.uid;
    }
    public static void SetUID(int _uid)
    {
        single.uid = _uid;
    }
}

public static class DBConnecter_Expand
{
    public static T ParseXmlNode<T>(this XmlNode node, string fieldName)
    {
        return (T)System.Convert.ChangeType(node[fieldName].InnerText, typeof(T));
    }
}
