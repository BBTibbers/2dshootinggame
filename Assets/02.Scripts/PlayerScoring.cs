using UnityEngine;
using System.IO;
using static Enemy;

public class PlayerScoring : MonoBehaviour
{
    public PlayerData playerData; 
    private string filePath;
    public static PlayerScoring Instance = null;

    private void Awake()
    {
        Instance = this;
        playerData = new PlayerData(0, 0, 0,0);
        filePath = "C:\\Users\\skku26\\2dshootinggame\\PlayerData.json";
        playerData = LoadPlayerData();

    }
    public void AddScore(Enemy.EnemyType enemyType)
    {
        if (enemyType == Enemy.EnemyType.Bullet) return;
        if (enemyType == Enemy.EnemyType.Semi)
        {
            playerData.Score += 1;
           
        }
        else if (enemyType == Enemy.EnemyType.Target)
        {
            playerData.Score += 20;
        }
        else if (enemyType == Enemy.EnemyType.Follow)
        {
            playerData.Score += 40;
        }
        else if (enemyType == Enemy.EnemyType.Normal)
        {
            playerData.Score += 10;
        }else if (enemyType == Enemy.EnemyType.Boss)
        {
            playerData.Score += 10000;
        }
        if (enemyType != Enemy.EnemyType.Semi && playerData.KillCount < 60)
            playerData.KillCount++;
        if (enemyType != Enemy.EnemyType.Semi && playerData.BossKillCount < 100)
            playerData.BossKillCount++;
        playerData.BoomCount = playerData.KillCount / 20;
        //Debug.Log(playerData.BossKillCount);


        UI_Game.Instance.KillRefresh(enemyType);

        SavePlayerData(playerData); // 데이터 저장

    }

    public void SavePlayerData(PlayerData data)
    {
        string json = JsonUtility.ToJson(data, true); // ✅ JSON 변환
        string encryptedJson = AESUtil.Encrypt(json); // ✅ AES 암호화
        File.WriteAllText(filePath, encryptedJson); 
       // Debug.Log("파일 저장 완료: " + filePath);
    }
    public PlayerData LoadPlayerData()
    {
        if (File.Exists(filePath)) // ✅ 파일이 존재하는지 확인
        {
            string encryptedJson = File.ReadAllText(filePath); // ✅ 암호화된 JSON 불러오기
            string decryptedJson = AESUtil.Decrypt(encryptedJson); // ✅ AES 복호화
            PlayerData data = JsonUtility.FromJson<PlayerData>(decryptedJson); // ✅ JSON → 객체 변환
         //   Debug.Log("🔓 복호화된 JSON: " + decryptedJson);
         //   Debug.Log("불러온 JSON: " + decryptedJson);
            return data;
        }
        else
        {
            Debug.LogWarning("저장된 데이터가 없습니다! 기본값을 반환합니다.");
            return new PlayerData(0, 0, 0,0); // 기본값 반환
        }
    }
}
