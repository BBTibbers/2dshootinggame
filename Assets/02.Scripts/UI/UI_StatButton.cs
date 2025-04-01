using TMPro;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using Coffee.UIExtensions;
using System.Collections;

public class UI_StatButton : MonoBehaviour
{
    public Stat _stat;

    public TextMeshProUGUI NameTextUI;
    public TextMeshProUGUI ValueTextUI;
    public TextMeshProUGUI CostTextUI;
    private Vector3 _originalScale;
    private bool _isShinyPlaying = false;
    public GameObject VFX;
    public GameObject Pivot;
    private bool _isAble = false;
    private void Awake()
    {
        _originalScale = transform.localScale;
    }

    public void Refresh()
    {
        NameTextUI.text = _stat.StatType.ToString();
        ValueTextUI.text = $"{_stat.Value}";
        CostTextUI.text = $"{_stat.Cost:N0}";


        if (CurrencyManager.Instance.Have(CurrencyType.Gold, _stat.Cost))
        {
            if (_isAble == false)
            {
                CostTextUI.color = Color.white;
                transform.DOScale(_originalScale * 1.1f, 0.2f).SetEase(Ease.OutBack);
                if (!_isShinyPlaying)
                {
                    StartCoroutine(PlayShinyEffect());
                }
                _isAble=true;
            }
        }
        else
        {
            CostTextUI.color = Color.red;
            transform.DOScale(_originalScale, 0.2f).SetEase(Ease.OutBack);
            _isAble = false;
        }
    }

    private IEnumerator PlayShinyEffect()
    {
        _isShinyPlaying = true;
        GameObject vfx = Instantiate(VFX);
        vfx.transform.position = Pivot.transform.position;

        var shiny = GetComponent<ShinyEffectForUGUI>();
        if (shiny != null)
        {
            shiny.Play(1f); // 1초 동안 재생한다고 가정
        }

        yield return new WaitForSeconds(3f); // 재생 시간만큼 기다림
        _isShinyPlaying = false;
    }


public void OnClickLevelUp()
    {
        if (StatManager.Instance.TryLevelUp(_stat.StatType))
        {
            Debug.Log($"{_stat.StatType} 레벨업!");
            // 업그레이드 성공 이펙트 실행            
        }
        else
        {
            Debug.Log($"돈이 부족합니다!");
            // 돈이 부족합니다 토스팝업 Show
        }
    }

}