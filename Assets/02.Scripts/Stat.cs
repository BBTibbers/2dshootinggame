public enum StatType
{
    Damage,
    Health,
    MoveSpeed,


    Count
}
// 스탯을 추상화 => 우리가 만드는 목적에 맞게 '속성'과 '기능'을 분리함으로써 간소화 하는 작업
// 속성:
//   - (Enum.StatType) 스탯 이름(ID)
//   - (int)           레벨
//   - (float)         현재 수치
//   - (int)           비용
//   - (float)         업그레이드 비용 증가값
//   - (float)         업그레이드 수치 증가값

// 기능:
//   - 업그레이드: public bool TryUpgrade()
//   - 레벨의 의한 수치 계산: private void Calculate()


public class Stat
{
    public StatType StatType;
    public int Level;

    private StatDataSO _data;

    public float Value;
    public int Cost;


    public Stat(StatType statType, int level, StatDataSO data)
    {
        StatType = statType;

        Level = level;

        _data = data;


        Calculate();
    }


    public bool TryUpgrade()
    {
        // 1. 돈이 충분한가?
        if (!CurrencyManager.Instance.TryConsume(CurrencyType.Gold, Cost))
        {
            return false;
        }

        // 2. 돈이 충분하다면 레벨업!
        Level += 1;

        // 3. 레벨업에 따른 수치/비용 재계산
        Calculate();

        return true;
    }

    private void Calculate()
    {
        Value = _data.DefaultValue + Level * _data.UpgradeAddValue;
        Cost = (int)(_data.DefaultCost + Level * _data.UpgradeAddCost);
    }
}