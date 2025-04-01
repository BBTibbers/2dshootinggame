using UnityEngine;
using System.Collections;

public class Fever : MonoBehaviour
{

    private bool _activated = false;
    public GameObject Barrier;
    public GameObject[] Backgrounds;
    private Collider2D _collider;
    private GameObject[] _enemys;
    public GameObject[] Generators;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Barrier.SetActive(false);
        _collider = GetComponent<Collider2D>();
    }


    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha6) && !_activated)
        {
            _activated = true;
            Barrier.SetActive(true);
            _collider.enabled = false;
            GetComponents<PlayerFire>()[0].SetAutoAttack(false);
            GetComponents<PlayerFire>()[1].SetAutoAttack(false);
            StartCoroutine(BackgroundSpeedControll());
        }


    }
    private IEnumerator BackgroundSpeedControll()
    {
        foreach (GameObject background in Backgrounds)
            background.GetComponent<Background>().ScrollSpeed *= 5;
        foreach (GameObject gen in Generators)
            gen.GetComponent<EnemyGenerate>().Cooltime /= 5;
        float duration = 3f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;  
            _enemys = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in _enemys)
            {
                enemy.GetComponent<Enemy>().setSpeed(enemy.GetComponent<Enemy>().enemyData.OriginSpeed * 5f);
                enemy.GetComponent<Enemy>().CURVE = enemy.GetComponent<Enemy>().OriginCURVE*5;
            }
            yield return null;
        }
        foreach (GameObject background in Backgrounds)
            background.GetComponent<Background>().ScrollSpeed /= 5;
        foreach (GameObject gen in Generators)
            gen.GetComponent<EnemyGenerate>().Cooltime *= 5;
        _enemys = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in _enemys)
        {
            enemy.GetComponent<Enemy>().setSpeed(enemy.GetComponent<Enemy>().enemyData.OriginSpeed);
            enemy.GetComponent<Enemy>().CURVE = enemy.GetComponent<Enemy>().OriginCURVE / 5;
        }
        _activated = false;
        Barrier.SetActive(false);
        _collider.enabled = true;

    }


}
