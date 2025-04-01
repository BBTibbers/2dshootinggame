using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using Coffee.UIExtensions;
using System.Collections;

public class UI_AttendanceButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public GameObject AttendancePanel;

    public int Day = 1;
    private RectTransform _rectTransform;
    private RectTransform _pannelTransform;
    public float EndScale = 1.1f;
    public float StartScale = 1f;
    public float Duration = 0.2f;

    public TextMeshProUGUI DayText;
    public TextMeshProUGUI AmountText;
    public Image IconImage;
    public Sprite GoldSprite;
    public Sprite DiamonSprite;
    private bool _isShinyPlaying = false;

    public void Start()
    {
        Attendance attendance = AttendanceManager.Instance.Attendances[Day-1];
        
        _rectTransform  = GetComponent<RectTransform>();
        _pannelTransform = AttendancePanel.GetComponent<RectTransform>();
        _pannelTransform.localScale = new Vector3(1f, 0f, 1f);
        if (_rectTransform == null)
        {
            Debug.Log("NULL");
            }

        DayText.text = $"Day {Day}";

        AmountText.text = attendance.Data.RewardAmount.ToString();


        if(attendance.Data.RewardCurrencyType == CurrencyType.Gold)
        {
            IconImage.sprite = GoldSprite;
        }
        else
        {
            IconImage.sprite = DiamonSprite;
        }
        Debug.Log(attendance.Data.Day);
        Debug.Log(attendance.IsRewarded);
        if (attendance.Data.Day > AttendanceManager.Instance.GetAttendanceCount()||attendance.IsRewarded==true)
        {
            GetComponent<Button>().interactable = false;
        }
        else
        {
            GetComponent<Button>().interactable = true;
        }

    }

    public void OnClickAttendancePopUp()
    {
        Debug.Log("PopUp");
        _pannelTransform.DOScale(new Vector3(1f,1f,1f), Duration).SetUpdate(true).SetEase(Ease.InOutQuad);
    }

    public void OnClickClose()
    {

        _pannelTransform.DOScale(new Vector3 (1f,0f,1f), Duration).SetUpdate(true).SetEase(Ease.InOutQuad);

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _rectTransform.DOScale(new Vector3(EndScale,EndScale,1f),Duration).SetUpdate(true);
    }

    private void OnDisable()
    {
        //_rectTransform.localScale = new Vector3 (StartScale, StartScale, 1f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _rectTransform.DOScale(new Vector3(StartScale, StartScale, 1f), Duration).SetUpdate(true);
    }

    public void OnClickCheckOut()
    {
        StartCoroutine(PlayShinyEffect());
    }
    private IEnumerator PlayShinyEffect()
    {
        _isShinyPlaying = true;

        var shiny = GetComponent<ShinyEffectForUGUI>();
        if (shiny != null)
        {
            shiny.Play(1f); // 1초 동안 재생한다고 가정
        }

        yield return new WaitForSeconds(3f); // 재생 시간만큼 기다림
        _isShinyPlaying = false;
    }

}
