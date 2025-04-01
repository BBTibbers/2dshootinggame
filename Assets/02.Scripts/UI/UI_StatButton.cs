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
            shiny.Play(1f); // 1�� ���� ����Ѵٰ� ����
        }

        yield return new WaitForSeconds(3f); // ��� �ð���ŭ ��ٸ�
        _isShinyPlaying = false;
    }


public void OnClickLevelUp()
    {
        if (StatManager.Instance.TryLevelUp(_stat.StatType))
        {
            Debug.Log($"{_stat.StatType} ������!");
            // ���׷��̵� ���� ����Ʈ ����            
        }
        else
        {
            Debug.Log($"���� �����մϴ�!");
            // ���� �����մϴ� �佺�˾� Show
        }
    }

}