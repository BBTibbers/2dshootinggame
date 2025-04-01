using UnityEngine;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using DG.Tweening;
using System.Collections;
using UnityEngine.UI;

public class UI_Game : MonoBehaviour
{

    public static UI_Game Instance = null;

    private void Awake()
    {
        Instance = this;
    }

    public List<GameObject> Booms;
    public Image FillImage;
    public TextMeshProUGUI KillText;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI WarningText;
    public TextMeshProUGUI SliderText;
    public TextMeshProUGUI GoldText;
    public Slider HealthSlider;
    public Slider BackSlider;
    public GameObject Player;
    private PlayerData _playerData;
    private int pauseHealth; 
    private Coroutine sliderCoroutine;
    private int currentHealth;



    void Start()
    {
        _playerData = Player.GetComponent<PlayerScoring>().playerData;
        WarningText.enabled = false;
        HealthSlider.gameObject.SetActive(false);
        BackSlider.gameObject.SetActive(false);
        SliderText.gameObject.SetActive(false);
        KillRefresh(Enemy.EnemyType.None);
    }

    public void KillRefresh(Enemy.EnemyType enemyType)
    {

        _playerData = Player.GetComponent<PlayerScoring>().playerData;

        KillText.text = $"Kills : {_playerData.KillCount}";
        for (int i = 0;  i< 3; i++)
        {
            Booms[i].SetActive(i < _playerData.BoomCount);

        }

        if(enemyType != Enemy.EnemyType.Semi&&enemyType != Enemy.EnemyType.None)
        {
            Scaling();
        }


        ScoreText.text = $"{_playerData.Score}";
    }

    private void Scaling()
    {
        DG.Tweening.Sequence seq = DOTween.Sequence();
        seq.Append(ScoreText.transform.DOScale(new Vector2(1.4f, 1.4f), 0.25f).SetEase(Ease.OutBack));
        seq.Append(ScoreText.transform.DOScale(new Vector2(1.0f, 1.0f), 0.25f).SetEase(Ease.OutBack));
    }

    private void SliderScaling()
    {
        DG.Tweening.Sequence seq = DOTween.Sequence();
        seq.Append(SliderText.transform.DOScale(new Vector2(1.2f, 1.2f), 0.25f).SetEase(Ease.OutBack));
        seq.Append(SliderText.transform.DOScale(new Vector2(1.0f, 1.0f), 0.25f).SetEase(Ease.OutBack));
    }

    public void Warning()
    {
        StartCoroutine(BlinkWarningText());
    }
    private IEnumerator BlinkWarningText()
    {
        float duration = 5f; // 총 깜박이는 시간
        float elapsedTime = 0f;
        float visibleTime = 0.8f; // 텍스트가 보이는 시간
        float hiddenTime = 0.2f;  // 텍스트가 사라지는 시간

        while (elapsedTime < duration)
        {
            WarningText.enabled = true; // 텍스트 표시
            yield return new WaitForSeconds(visibleTime);
            elapsedTime += visibleTime;

            WarningText.enabled = false; // 텍스트 숨김
            yield return new WaitForSeconds(hiddenTime);
            elapsedTime += hiddenTime;
        }

        WarningText.enabled = false; // 최종적으로 숨김 상태 유지
    }

    public void BeginSlider()
    {
        int maxHP = BossGen.Instance.newEnemy.GetComponent<Enemy>().enemyData.MaxHealth;
        HealthSlider.gameObject.SetActive(true);
        BackSlider.gameObject.SetActive(true);
        HealthSlider.maxValue = maxHP;
        HealthSlider.value = maxHP;
        BackSlider.maxValue = maxHP;
        BackSlider.value = maxHP;
        currentHealth = maxHP;
        SliderText.gameObject.SetActive(true);
    }
    public void SliderControll()
    {
        if (sliderCoroutine != null)
        {
            StopCoroutine(sliderCoroutine); // 기존 코루틴 중단
        }
        sliderCoroutine = StartCoroutine(SetSlider());
    }
    private IEnumerator SetSlider()
    {
        float duration = 1f; // 애니메이션이 걸리는 시간
        //float hiddenTime = 0.1f;


            pauseHealth = currentHealth;

           // yield return new WaitForSeconds(hiddenTime);

            float elapsed = 0f;
            SliderScaling();
        while (elapsed < duration)
            {
                float now = HealthSlider.value;
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                currentHealth  = (int)Mathf.Lerp(pauseHealth, now, t);
                BackSlider.value = Mathf.Lerp(pauseHealth, now, t);
            float healthPercent = HealthSlider.value / HealthSlider.maxValue;
            FillImage.color = Color.Lerp(Color.red, Color.yellow, healthPercent);

            SliderText.text = $"{HealthSlider.value} / {HealthSlider.maxValue}";
            yield return null;
        }
        sliderCoroutine = null; // 완료되면 null 처리


    }
    public void RefreshGold(int gold)
    {
        GoldText.text = gold.ToString("N0");
    }

}
