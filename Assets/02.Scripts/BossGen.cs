using UnityEngine;

public class BossGen : MonoBehaviour
{
    public static BossGen Instance = null;
    public GameObject Boss;
    public bool isBoss = false;
    public GameObject[] EnemyGen;
    private float time = 0;
    public GameObject newEnemy;
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (PlayerScoring.Instance.playerData.BossKillCount == 100)
        {
            if (time == 0)
            {
                time = Time.time;
                UI_Game.Instance.Warning();
            }
                BossSpawn();
        }
        if(isBoss == false)
        {
            foreach (GameObject gen in EnemyGen)
            {
                gen.GetComponent<EnemyGenerate>().isSpawning = true;
            }
            UI_Game.Instance.HealthSlider.gameObject.SetActive(false);
            UI_Game.Instance.BackSlider.gameObject.SetActive(false);
            UI_Game.Instance.SliderText.gameObject.SetActive(false);
        }
    }
    public void BossSpawn()
    {

        foreach (GameObject gen in EnemyGen) {

            gen.GetComponent<EnemyGenerate>().isSpawning = false;
        }
        isBoss = true;

        if (Time.time > time + 5)
        {
            PlayerScoring.Instance.playerData.BossKillCount = 0;
            newEnemy = Instantiate(Boss);

            newEnemy.transform.position = this.transform.position; // 총알 위치 설정

            UI_Game.Instance.BeginSlider();
            time = 0;
        }
    }

}
