using UnityEngine;

[System.Serializable]
public class PlayerData //: MonoBehaviour
{
    public int Score = 0;
    public int KillCount = 0;
    public int BoomCount = 0;
    public int BossKillCount = 0;

    public PlayerData(int score, int killcount, int boomcount, int bosskillcount)
    {
        this.Score = score;
        this.KillCount = killcount;
        this.BoomCount = boomcount;
        this.BossKillCount = bosskillcount;
    }
}
