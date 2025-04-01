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

    // ToDo: ���� �ð� �߰�
}

// ��ȭ ������
public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

    private CurrencySaveData _saveData;
    private List<int> _values => _saveData.Values;

    // ��������Ʈ: �Լ��� �����ϴ� ����
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

    // ��ȭ��
    public int Get(CurrencyType currencyType)
    {
        return _values[(int)currencyType];
    }

    // ��ȭ �߰�
    public void Add(CurrencyType currencyType, int amount)
    {
        _values[(int)currencyType] += amount;

        UI_Game.Instance.RefreshGold(Gold);
        //Debug.Log(_values[(int)currencyType]);
        OnDataChangedCallback?.Invoke();
        Save();
    }

    // ��ȭ ������ �ִ�?
    public bool Have(CurrencyType currencyType, int amount)
    {
        return _values[(int)currencyType] >= amount;
    }

    // ��ȭ �Ҹ�
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