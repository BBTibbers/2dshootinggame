using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class AttendanceSaveData 
{
    public string LastLoginDateTimeString;
    public int AttendanceCount;
    public List<bool> IsRewardedList = new List<bool>();

    public AttendanceSaveData(int attendanceCount, string lastLoginDateTimeString)
    {
        AttendanceCount = attendanceCount;
        LastLoginDateTimeString = lastLoginDateTimeString;
    }
}


public class AttendanceManager : MonoBehaviour
{
    // ������(�Ѹ�<�̱���>): �߰�, ����, ��ȸ, ����

    public static AttendanceManager Instance;

    private AttendanceSaveData _saveData;  


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
        // [�߰�]
        // ������ ���۵� �� �⼮ ��ü���� �������Ѵ�. (��ȹ�ڰ� ������ ��ŭ)
        _attendances = new List<Attendance>(_soDatas.Count); // �뷮(capacity)�� �����ؼ� �̸� �ʿ��� ��ŭ �޾Ƶ���

        Load();

        for (int i = 0; i < _soDatas.Count; i++)
        {
            _attendances.Add(new Attendance
                (_soDatas[i], _saveData.IsRewardedList[i]));
        }

        _attendanceCount = _saveData.AttendanceCount; 
        
        DateTime tempDate;
        if (DateTime.TryParse(_saveData.LastLoginDateTimeString, out tempDate))
        {
            _lastLoginDateTime = tempDate;
        }
        else
        {
            _lastLoginDateTime = DateTime.MinValue; // �Ľ� ���� �� �⺻�� ó��
        }


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
        for (int i = 0; i < _attendances.Count; i++)
        {
            _saveData.IsRewardedList[i] = _attendances[i].IsRewarded;
        }
        _saveData.LastLoginDateTimeString = _lastLoginDateTime.ToString("o");
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
            _saveData = new AttendanceSaveData(0, DateTime.Now.ToString("o"));
            _saveData.IsRewardedList = new List<bool>(Enumerable.Repeat(false, _soDatas.Count));
        }
    }


}