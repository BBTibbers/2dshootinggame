using UnityEngine;
using static Bullet;
using static UnityEngine.GraphicsBuffer;
using System.Collections;


public class Enemy : MonoBehaviour
{
    public EnemyDataSO enemyData;

    private GameObject _player;
    private float _speed;
    public int NowHealth;
    private Vector2 _direction = new Vector2(0,0);
    private Vector2 _lookAt= new Vector2(0,0);
    private float _time;
    public float CURVE = 5f;
    public float OriginCURVE = 5f;
    public Animator MyAnimator;
    public GameObject Semi;
    public GameObject[] Item;

    private const float SEMI_SPAWN_RANGE = 0.5f;

    public GameObject ExplosionVFXPrefab;
    public enum EnemyType
    {
        Normal,
        Semi,
        Target,
        Follow,
        Horizon,
        None,
        Boss,
        Bullet
    }

    public enum EnemyHorizontal
    {
        Left,
        Right
        ,None
    }
    public EnemyHorizontal enemyHorizontal;

    public void Initialize()
    {

    }
    private void Awake()
    {
        MyAnimator = GetComponent<Animator>();
    }
    private void Start()
    {
        FindPlayer();
        _speed = enemyData.OriginSpeed;
        NowHealth = enemyData.MaxHealth;
    }

    public void setSpeed(float speed)
    {
        _speed = speed;
    }

    private void FindPlayer()
    {
            if (_player == null)
            {
                _player = GameObject.FindWithTag("Player"); // 씬에서 Player 자동으로 찾기
                if (_player == null)
                {
                    Debug.LogError("씬에서 'Player' 오브젝트를 찾을 수 없습니다!");
                }
            }
            if (enemyData.EnemyType == EnemyType.Semi)
            {
                float x = Mathf.Cos(Random.Range(0f, 360f));
                float y = Mathf.Sin(Random.Range(0f, 360f));
                _direction = new Vector2(x, y);
                _direction.Normalize();
            }
            _time = Time.time;
        
    }
    void Update()
    {
        if (enemyData.EnemyType == EnemyType.Normal || enemyData.EnemyType == EnemyType.Bullet)
        {
            Move();
        }
        else if (enemyData.EnemyType == EnemyType.Semi || enemyData.EnemyType == EnemyType.Follow)
        {
            MoveFollow();
        }
        else if (enemyData.EnemyType == EnemyType.Target)
        {
            MoveTarget();
        }
        else if (enemyData.EnemyType == EnemyType.Horizon)
        {
            MoveHorizon();
        }
        else if (enemyData.EnemyType == EnemyType.Boss)
        {
            BossMove();
        }
    }

    private void Move()
    {
        Vector2 direction = Vector2.down;
        transform.Translate(direction * _speed * Time.deltaTime);
    }

    private void BossMove()
    {
        Vector3 bossLocate;

        float x = Random.Range(-1f, 1f);
        float y = 3f;
        bossLocate = new Vector3(x, y,0);
        _lookAt = bossLocate - transform.position;
        _lookAt.Normalize();
        if (_direction.x == 0 && _direction.y == 0)
        {
            _direction = _lookAt;
        }

        _direction = _direction * (1 - CURVE * Time.deltaTime) + _lookAt * (CURVE * Time.deltaTime);
        //_direction.Normalize();

        //float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg; // 라디안을 각도로 변환
        //transform.rotation = Quaternion.Euler(0, 0, angle + 90); // 2D는 Z축 회전
        transform.Translate(_direction * _speed * Time.deltaTime);
    }

    private void MoveHorizon()
    {
        Vector2 direction = Vector2.left;
        if(enemyHorizontal == EnemyHorizontal.Left)
        {
            direction = Vector2.left;
        }
        else if (enemyHorizontal == EnemyHorizontal.Right)
        {
            direction = Vector2.right;
        }
        transform.Translate(direction * _speed * Time.deltaTime);
    }
    private void MoveFollow()
    {
        _lookAt = _player.transform.position - transform.position;
        _lookAt.Normalize();
        if(_direction.x == 0 && _direction.y == 0)
        {
            _direction = _lookAt;
        }

        _direction = _direction * (1 - CURVE*Time.deltaTime) + _lookAt * (CURVE*Time.deltaTime);
        _direction.Normalize();

        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg; // 라디안을 각도로 변환
            transform.rotation = Quaternion.Euler(0, 0, angle+90); // 2D는 Z축 회전
        transform.Translate(Vector2.down * _speed * Time.deltaTime);
    }
    private void MoveTarget()
    {
        if (_lookAt.x == 0 && _lookAt.y == 0)
        {
            _lookAt = _player.transform.position - transform.position;
            _lookAt.Normalize();
            float angle = Mathf.Atan2(_lookAt.y, _lookAt.x) * Mathf.Rad2Deg; // 라디안을 각도로 변환
            transform.rotation = Quaternion.Euler(0, 0, angle+90); // 2D는 Z축 회전
        }
        transform.Translate(Vector2.down * _speed * Time.deltaTime);
    }

    public void PlayAnimation()
    {
        if(MyAnimator)
            MyAnimator.SetTrigger("Hit"); 
        //MyAnimator.ResetTrigger("Hit");
    }

    public void onDeath()
    {
       if (enemyData.EnemyType == EnemyType.Boss)
        {
            GameObject vfx = Instantiate(ExplosionVFXPrefab);
            vfx.transform.position = transform.position;

            Destroy(vfx, 5f);
            BossGen.Instance.isBoss = false;
        }
        else 
        {
            VFX vfx;
            if (enemyData.EnemyType == EnemyType.Semi)
                vfx = VFXPool.Instance.Create(VFX.VFXType.Semi,this.transform.position);
            else if(enemyData.EnemyType == EnemyType.Normal|| enemyData.EnemyType == EnemyType.Follow|| enemyData.EnemyType == EnemyType.Target)
                vfx = VFXPool.Instance.Create(VFX.VFXType.Normal, this.transform.position);

        }


        if (UnityEngine.Random.Range(0, 5) == 0)
        {
            CurrencyManager.Instance.Add(CurrencyType.Diamond, 10);
        }
        else
        {
            CurrencyManager.Instance.Add(CurrencyType.Gold, 10);
        }

        PlayerScoring.Instance.AddScore(this.enemyData.EnemyType);

    }



    public void takeDamage(Damage damage)
    {

        PlayAnimation();
        
        NowHealth -= damage.damage;

        if (enemyData.EnemyType == EnemyType.Boss) {
            UI_Game.Instance.SliderControll();
        }
        
        Vector3 direction = new Vector3(SEMI_SPAWN_RANGE, 0, 0);
        if (NowHealth <= 0)
        {
           onDeath();
            if (enemyData.EnemyType != EnemyType.Semi&&enemyData.EnemyType !=EnemyType.Boss&&enemyData.EnemyType !=EnemyType.Bullet)
            {

                EnemyPool.Instance.Create(EnemyType.Semi, transform.position + direction);
                EnemyPool.Instance.Create(EnemyType.Semi, transform.position);
                EnemyPool.Instance.Create(EnemyType.Semi, transform.position - direction);

                if (Random.value > 0.3f)
                {
                    float rand = Random.value;
                    if (rand < (1f / 4f))
                    {
                        Instantiate(Item[0], transform.position, Quaternion.identity);
                    }
                    else if (rand < (1f / 2f))
                    {
                        Instantiate(Item[1], transform.position, Quaternion.identity);
                    }
                    else if( rand <3f/4f)
                    {
                        Instantiate(Item[2], transform.position, Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(Item[3], transform.position, Quaternion.identity);
                    }
                }
            }
            gameObject.SetActive(false);
        }
    }
}
