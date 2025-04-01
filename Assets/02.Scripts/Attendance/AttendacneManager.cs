using System;
using System.Collections.Generic;
using UnityEngine;


public class AttendanceSaveData 
{
    public DateTime LastLoginDateTime;
    public int AttendanceCount;
    public List<bool> IsRewarded = new List<bool>();
}


public class AttendanceManager : MonoBehaviour
{
    // ������(�Ѹ�<�̱���>): �߰�, ����, ��ȸ, ����

    public static AttendanceManager Instance;

    private AttendanceSaveData _saveData;  
    private DateTime _lastLoginSaved => _saveData.LastLoginDateTime;
    private int _attendanceCountSaved => _saveData.AttendanceCount;
    private List<bool> _isRewardedSaved => _saveData.IsRewarded;


    // ��ȹ�ڰ� ������ �⼮ �����͵�
    [SerializeField]
    private List<AttendanceDataSO> _soDatas;

    // (�⼮ �����ͷκ��� �������) '�⼮' ��ü��
    private List<Attendance> _attendances; // = null

    // �⼮ ���� ������
    private DateTime _lastLoginDateTime;  // ���������� �α����� ��¥
    private int _attendanceCount;                   // ������� �⼮ Ƚ��

    // ������ �ٲ𶧸��� ȣ��Ǵ� �ݹ�
    public Action OnDataChanged;

    // [��ȸ]
    public List<Attendance> Attendances => _attendances;



    private void Awake()
    {
        Instance = this;
        Load();
        // [�߰�]
        // ������ ���۵� �� �⼮ ��ü���� �������Ѵ�. (��ȹ�ڰ� ������ ��ŭ)
        _attendances = new List<Attendance>(_soDatas.Count); // �뷮(capacity)�� �����ؼ� �̸� �ʿ��� ��ŭ �޾Ƶ���
        foreach (AttendanceDataSO data in _soDatas)
        {
            Attendance attendance = new Attendance(data, false);
            attendance.SetRewarded(_isRewardedSaved[data.Day - 1]);
            _attendances.Add(attendance);
        }

        _attendanceCount = _attendanceCountSaved;
        _lastLoginDateTime = _lastLoginSaved;

        OnDataChanged?.Invoke();

        AttendanceCheck();
    }

    private void AttendanceCheck()
    {
        DateTime today = DateTime.Today;
        if (today > _lastLoginDateTime) // ������ ���������� �α����� ��¥���� ũ�ٸ�(�Ϸ� �̻� �����ٸ�) 
        {
            // ������ ����
            _lastLoginDateTime = today;
            _attendanceCount += 1;
        }
    }

    public int GetAttendanceCount()
    {
        return _attendanceCount;
    }




    // [���� �ޱ�]
    // ���¿°����.�����ּ���(�⼮)
    public bool TryGetReward(Attendance attendance)
    {
        // ���� 1. �̹� ������ �޾Ҵٸ� ����
        if (attendance.IsRewarded)
        {
            return false;
        }

        // ���� 2. ���� �׸�ŭ �⼮�� �ߴ°�?
        if (_attendanceCount < attendance.Data.Day)
        {
            return false;
        }

        CurrencyManager.Instance.Add(attendance.Data.RewardCurrencyType, attendance.Data.RewardAmount);
        attendance.SetRewarded(true);

        OnDataChanged?.Invoke();
        Save();
        return true;
    }
    private const string SAVE_KEY = "Attendance";

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
            _saveData = JsonUtility.FromJson<AttendanceSaveData>(jsonData);
        }
        else
        {
            _saveData = new AttendanceSaveData();
        }
    }


}