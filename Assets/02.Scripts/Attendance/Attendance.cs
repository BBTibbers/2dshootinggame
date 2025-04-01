using UnityEngine;

public class Attendance
{
    // �⼮ ������
    public readonly AttendanceDataSO Data;

    // ���� �ޱ� ����
    private bool _rewarded;
    public bool IsRewarded => _rewarded;

    // ������(�����Ϳ� ���� ������ �޾ƿ´�.)
    public Attendance(AttendanceDataSO data, bool rewarded)
    {
        Data = data;
        _rewarded = rewarded;
    }


    public void SetRewarded(bool rewarded)
    {

        _rewarded = rewarded;
    }
}