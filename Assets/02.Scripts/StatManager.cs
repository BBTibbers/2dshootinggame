using System;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    // ������: �߰�, ����, ��ȸ, ����, 
    // ������, ��ȸ
    public static StatManager Instance;

    // ������
    public List<StatDataSO> StatDataList;


    private List<Stat> _stats = new List<Stat>();
    public List<Stat> Stats => _stats;

    // ��������Ʈ: �Լ��� �����ϴ� ����
    public delegate void OnDataChanged();
    // 
    public OnDataChanged OnDataChangedCallback = null;



    private void Awake()
    {
        Instance = this;

        for (int i = 0; i < (int)StatType.Count; ++i)
        {
            _stats.Add(new Stat((StatType)i, 1, StatDataList[i]));
        }
    }


    public bool TryLevelUp(StatType statType)
    {
        // ��! �����Ͱ� ��ȭ�Ҷ����� �ʰ� ����� �Լ��� ȣ�����ٰ�!
        OnDataChangedCallback?.Invoke();

        return _stats[(int)statType].TryUpgrade();
    }
}