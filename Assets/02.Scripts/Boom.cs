using UnityEngine;

public class Boom : MonoBehaviour
{
    public GameObject BoomVFX;
    private float _time = 0;
    public GameObject Player;
    private PlayerData _playerData;
    private bool _activated = false;

    private Collider2D _collider;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    public GameObject MainCamera;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>(); // Collider 가져오기
        _spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer 가져오기
        _animator = GetComponent<Animator>(); // Animator 가져오기
        
    }

    private void Start()
    {
        // GameObject는 활성화된 상태에서, Collider, SpriteRenderer, Animator만 비활성화
        SetComponentsActive(false);

        _playerData = Player.GetComponent<PlayerScoring>().playerData;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5) && !_activated)
        {
            if (_playerData.KillCount < 20)
            {
                return;
            }

            _time = Time.time;
            _playerData.KillCount -= 20;

            _playerData.BoomCount = _playerData.KillCount / 20;
            _activated = true;
            UI_Game.Instance.KillRefresh(Enemy.EnemyType.None);
            if (BossGen.Instance.isBoss)
            {
                BossAttack.Instance.EnemyScript.takeDamage(new Damage(Damage.DamageType.Boom,500));
            }
            // Collider, SpriteRenderer, Animator 활성화
            SetComponentsActive(true);

        }

        if(_activated)
        {
            MainCamera.GetComponent<CameraShaking>().Shake(true);
        }

        if (_activated && Time.time - _time > 2)
        {
            _activated = false;

            // Collider, SpriteRenderer, Animator 비활성화
            SetComponentsActive(false);
        }
    }

    private void SetComponentsActive(bool isActive)
    {
        if (_collider != null)
            _collider.enabled = isActive;

        if (_spriteRenderer != null)
            _spriteRenderer.enabled = isActive;

        if (_animator != null)
            _animator.enabled = isActive;
    }

    private void Kill()
    {
        if (_activated)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                Damage damage = new Damage(Damage.DamageType.Boom,1000);
                enemy.GetComponent<Enemy>().takeDamage(damage);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if(other.GetComponent<Enemy>().enemyData.EnemyType!=Enemy.EnemyType.Boss)
               other.GetComponent<Enemy>().takeDamage(new Damage(Damage.DamageType.Boom, 1000));
        }
    }
}
