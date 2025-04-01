using UnityEngine;
using static System.Math;

public class PlayerMove : MonoBehaviour
{

    // MonoBehaviour : 여러가지 이벤트 함수를 자동으로 호출하는 기능
    // Compoenent : 게임 오브젝트에 추가할 수 있는 여러가지 기능

    //목표 : 키보드 입력에 따라 플레이어를 이동시키기.

    private float Speed => StatManager.Instance.Stats[(int)StatType.MoveSpeed].Value;
    private int Health => (int)StatManager.Instance.Stats[(int)StatType.Health].Value;
    private float _nowSpeed;
    private float _h;
    private float _v;
    private bool _isE;
    private bool _isQ;
    private bool _autoMove = false;
    private const float LEFT_WALL = -2f;
    private const float RIGHT_WALL = 2f;
    private const float TOP_WALL = 1f;
    private const float BOTTOM_WALL = -5f;
    private const float SPEED_RANGE = 3f;
    private const float FOLLOW_DISTANCE = 2f;
    private const float WARNING_DISTANCE = 2f;
    private Vector2 _direction = new Vector2(0,0);
    public GameObject MainCamera;

    public Animator MyAnimaitor;

    public delegate void OnDataChanged();
    // 
    public OnDataChanged OnDataChangedCallback = null;

    public static PlayerMove Instance;

    public FloatingJoystick Joystick;

    //start보다 먼저 호출
    private void Awake()
    {
        Instance = this;
        MyAnimaitor = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _autoMove = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _autoMove = false;
        }

        SpeedCheck();
        CheckBorder();;
        Move();
        PlayAnimation(_direction);
    }

    private void CheckBorder()
    {
        if (transform.position.x < LEFT_WALL)
        {
            transform.position = new Vector3(RIGHT_WALL, transform.position.y, 0);
        }
        if (transform.position.x > RIGHT_WALL)
        {
            transform.position = new Vector3(LEFT_WALL, transform.position.y, 0);
        }
        if (transform.position.y < BOTTOM_WALL)
        {
            transform.position = new Vector3(transform.position.x, TOP_WALL, 0);
        }
        if (transform.position.y > TOP_WALL)
        {
            transform.position = new Vector3(transform.position.x, BOTTOM_WALL, 0);
        }
    }

    private void Move()
    {
        _direction = new Vector2(_h, _v); // 방향 벡터 생성
        _direction.Normalize();

        transform.Translate(_direction * _nowSpeed * Time.deltaTime); // 방향 벡터에 속도를 곱해 이동
    }

    private void SpeedCheck()
    {
        if (_autoMove == false)
        {
            _h = Input.GetAxisRaw("Horizontal"); // 수평키 누르기에 따라 -1f ~ 1f 값 반환
            _v = Input.GetAxisRaw("Vertical"); // 수직 키 누르기에 따라 -1f ~ 1f 값 반환
#if PLATFORM_ANDROID
            _h = Joystick.Horizontal;
            _v = Joystick.Vertical;

#endif

            _isE = Input.GetKey(KeyCode.E); // E키 누르면 true 반환
            _isQ = Input.GetKey(KeyCode.Q); // Q키 누르면 true 반환

            if (_isE)
            {
                _nowSpeed = Speed  + SPEED_RANGE;
            }
            else
            if (_isQ)
            {
                _nowSpeed = Speed - SPEED_RANGE;
            }
            else
            {
                _nowSpeed = Speed;
            }
        }else if (_autoMove == true)
        {
            GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");

            GameObject closestEnemy = null;
            float closestDistance = Mathf.Infinity;

            if(enemys.Length == 0)
            {
                _h = 0;
                _v = 0;
                return;
            }

            foreach (GameObject enemy in enemys)
            {

                if (Vector2.Distance(transform.position, enemy.transform.position) < closestDistance)
                {
                    closestEnemy = enemy;
                    closestDistance = Vector2.Distance(transform.position, enemy.transform.position);
                }
                if (closestDistance >= FOLLOW_DISTANCE)
                {
                    _v = 0;

                    _h = (transform.position.x - closestEnemy.transform.position.x < 0) ? 1 : -1;
                }
                else if(closestDistance < WARNING_DISTANCE)
                {
                    _h = (transform.position.x - closestEnemy.transform.position.x > 0) ? 1 : -1;
                    _v = -1;
                }else
                {
                    _h = 0;
                    _v = 0;
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other) // stay, exit도 있음

    {
        if (other.CompareTag("Enemy"))
        {
            var stat = StatManager.Instance.Stats[(int)StatType.Health];
            stat.Value -= 1;

            OnDataChangedCallback?.Invoke();
            if (stat.Value <= 0)
            {
                Destroy(gameObject);
            }

            other.gameObject.SetActive(false);
            MainCamera.GetComponent<CameraShaking>().Shake(true);
        }
    }

    private void PlayAnimation(Vector2 direction)
    {/*
        if(direction.x == 0)
        {
            Myanimaitor.Play("Player_Idle");
        }
        else if (direction.x > 0)
        {
            Myanimaitor.Play("Player_Right");
        }
        else if (direction.x < 0)
        {
            Myanimaitor.Play("Player_Left");
        }*/
        MyAnimaitor.SetInteger("x", direction.x < 0 ? -1 : direction.x > 0 ? 1 : 0);
    }
}
