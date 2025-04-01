using UnityEngine;
using UnityEngine.UIElements;

public class BossAttack : MonoBehaviour
{
    public Enemy EnemyScript;
    public GameObject EnemyBullet1;
    public GameObject MuzzleLeft;
    public GameObject MuzzleRight;
    public float Cooltime1;
    public float Cooltime2;
    private float _angle = 0;
    private float _anglePivot = 1f;

    private float _nextFire=0;


    public static BossAttack Instance = null;

    private void Awake()
    {
        Instance = this;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EnemyScript = this.GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        UI_Game.Instance.HealthSlider.value = EnemyScript.NowHealth;

        if (EnemyScript.NowHealth > 0.7f * EnemyScript.enemyData.MaxHealth)
        {
            if (Time.time < _nextFire)
            {
                return;
            }

            GameObject[] bullet = new GameObject[36];
            for (int i = 0; i < 24; i++)
            {
                bullet[i] = Instantiate(EnemyBullet1); //프리팹의 인스턴스화, 게임오브젝트를 복제해 씬에 새로 넣는다.
                bullet[i].transform.position = transform.position; // 총알 위치 설정
                bullet[i].transform.rotation = Quaternion.Euler(0, 0, i * 15);
            }

            _nextFire = Time.time + Cooltime1;
        }
        else if (EnemyScript.NowHealth > 0.3f * EnemyScript.enemyData.MaxHealth) {

            if (Time.time < _nextFire)
            {
                return;
            }

            GameObject[] bullet = new GameObject[2];

            bullet[0] = Instantiate(EnemyBullet1); //프리팹의 인스턴스화, 게임오브젝트를 복제해 씬에 새로 넣는다.
            bullet[0].transform.position = MuzzleLeft.transform.position; // 총알 위치 설정
            bullet[1] = Instantiate(EnemyBullet1); //프리팹의 인스턴스화, 게임오브젝트를 복제해 씬에 새로 넣는다.
            bullet[1].transform.position = MuzzleRight.transform.position; // 총알 위치 설정

            bullet[0].transform.rotation = Quaternion.Euler(0, 0, _angle - 5);
            bullet[1].transform.rotation = Quaternion.Euler(0, 0, _angle + 5);

            if (_angle == 10)
                _anglePivot = -1f;
            if (_angle == -10)
                _anglePivot = 1f;

            _angle += _anglePivot;

            _nextFire = Time.time + Cooltime2;

        }
        else
        {
            if (Time.time < _nextFire)
            {
                return;
            }

            GameObject[] bullet = new GameObject[2];

            bullet[0] = Instantiate(EnemyBullet1); //프리팹의 인스턴스화, 게임오브젝트를 복제해 씬에 새로 넣는다.
            bullet[0].transform.position = MuzzleLeft.transform.position; // 총알 위치 설정
            bullet[1] = Instantiate(EnemyBullet1); //프리팹의 인스턴스화, 게임오브젝트를 복제해 씬에 새로 넣는다.
            bullet[1].transform.position = MuzzleRight.transform.position; // 총알 위치 설정

            bullet[0].transform.rotation = Quaternion.Euler(0, 0, _angle + 20);
            bullet[1].transform.rotation = Quaternion.Euler(0, 0, _angle - 20);

            if (_angle == 10)
                _anglePivot = -1f;
            if (_angle == -10)
                _anglePivot = 1f;

            _angle += _anglePivot;

            _nextFire = Time.time + Cooltime2;
        }
    }


}
