using UnityEngine;
using UnityEngine.XR;

public class Item : MonoBehaviour
{
    // Update is called once per frame

    GameObject Player;
    public float Speed;
    private float _minSpeed = 0f;
    private Vector2 _lookAt = new Vector2(0, 0);
    private Vector2 _direction = new Vector2(0, 0);
    private bool _isTracking = false;
    private float _time = 0;
    private float _vanishTime = 0;
    private const float CURVE = 0.01f;
    private const float TRACKING_RANGE = 5f;
    private const float MOVE_BURFF = 1f;
    private const float ATTACK_BURFF = 5f / 6f;
    private const int HEAL_BURFF = 1;

    public GameObject VFXItem;

    public enum ItemType
    {
        MoveBurff,
        AttakcBuff,
        HealthBuff,
        Magnet
    }
    public ItemType itemType;

    private void Start()
    {
        Player = GameObject.FindWithTag("Player");
        _vanishTime = Time.time;
    }
    void Update()
    {
        if(Time.time - _vanishTime > 5)
        {
            Destroy(this.gameObject);
        }
        Tracking();
        if (_isTracking)
        {
            Follow();
        }
    }

    void Tracking()
    {
        if(Vector2.Distance(Player.transform.position, transform.position)<TRACKING_RANGE)
        {
            _isTracking = true;
        }
    }
    private void Follow()
    {
        _lookAt = Player.transform.position - transform.position;
        _lookAt.Normalize();
        _direction = _direction * (1 - CURVE) + _lookAt * (CURVE);
        float nowSpeed = Speed*3 / Vector2.Distance(Player.transform.position, transform.position);
        if (_minSpeed < nowSpeed) { _minSpeed = nowSpeed; }
        else { nowSpeed = _minSpeed; }

        if ((Vector2.Distance(Player.transform.position,transform.position)<1f))
        {
            transform.position = Player.transform.position;
            return;
        }
        transform.Translate(_direction * nowSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        _time = Time.time; 
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if(Time.time-_time > 1f)
        {
            if (itemType == ItemType.MoveBurff)
            {
                StatManager.Instance.Stats[(int)StatType.MoveSpeed].Value += MOVE_BURFF;

            }
            else if (itemType == ItemType.AttakcBuff)
            {
                Player.GetComponents<PlayerFire>()[0].Cooltime *= ATTACK_BURFF;
                Player.GetComponents<PlayerFire>()[1].Cooltime *= ATTACK_BURFF;
            }
            else if (itemType == ItemType.HealthBuff)
            {
                StatManager.Instance.Stats[(int)StatType.Health].Value += HEAL_BURFF;
            }
            else if (itemType == ItemType.Magnet)
            {
                PullAll();
            }
            if (VFXItem != null)
            {
                GameObject vfx = Instantiate(VFXItem);
                vfx.transform.position = transform.position;
            }
            Destroy(this.gameObject);
        }
    }

    private void PullAll()
    {
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
        foreach (GameObject item in items)
        {
            item.GetComponent<Item>()._isTracking = true;
            item.GetComponent<Item>().Speed = 15f;
        }
    }
}
