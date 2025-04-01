using System;
using System.Collections.Generic;
using UnityEngine;

public enum CurrencyType
{
    Gold,
    Diamond,

    Count
}

public class CurrencySaveData
{
    public List<int> Values = new List<int>(new int[(int)CurrencyType.Count]);

    // ToDo: 저장 시간 추가
}

// 재화 관리자
public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

    private CurrencySaveData _saveData;
    private List<int> _values => _saveData.Values;

    // 델리게이트: 함수를 참조하는 변수
    public delegate void OnDataChanged();
    // 
    public OnDataChanged OnDataChangedCallback = null;

    private void Awake()
    {
        Instance = this;

        Load();
    }

    private void Start()
    {
        UI_Game.Instance.RefreshGold(Gold);
    }

    public int Gold => _values[(int)CurrencyType.Gold];
    public int Diamond => _values[(int)CurrencyType.Diamond];

    // 재화량
    public int Get(CurrencyType currencyType)
    {
        return _values[(int)currencyType];
    }

    // 재화 추가
    public void Add(CurrencyType currencyType, int amount)
    {
        _values[(int)currencyType] += amount;

        UI_Game.Instance.RefreshGold(Gold);
        //Debug.Log(_values[(int)currencyType]);
        OnDataChangedCallback?.Invoke();
        Save();
    }

    // 재화 가지고 있니?
    public bool Have(CurrencyType currencyType, int amount)
    {
        return _values[(int)currencyType] >= amount;
    }

    // 재화 소모
    public bool TryConsume(CurrencyType currencyType, int amount)
    {
        if (!Have(currencyType, amount))
        {
            return false;
        }

        _values[(int)currencyType] -= amount;

        UI_Game.Instance.RefreshGold(Gold);
        OnDataChangedCallback?.Invoke();
        Save();

        return true;
    }

    private const string SAVE_KEY = "Currency";

    private void Save()
    {
        string jsonData = JsonUtility.ToJson(_saveData);
        PlayerPrefs.SetString(SAVE_KEY, jsonData);
    }

    private void Load()
    {
        if (PlayerPrefs.HasKey(SAVE_KEY))
        {
            string jsonData = PlayerPrefs.GetString(SAVE_KEY);
            _saveData = JsonUtility.FromJson<CurrencySaveData>(jsonData);
        }
        else
        {
            _saveData = new CurrencySaveData();
        }
    }
}