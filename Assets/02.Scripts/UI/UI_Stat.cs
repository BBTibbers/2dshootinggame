using System.Collections.Generic;
using UnityEngine;

public class UI_Stat : MonoBehaviour
{
    public List<UI_StatButton> UI_StatButtons;

    private void Start()
    {
        // 1. 게임 시작되면 스탯 매니저로부터 스탯을 가져와 하위 버튼 UI들을 초기화 한다.
        for (int i = 0; i < (int)StatType.Count; ++i)
        {
            UI_StatButtons[i]._stat = StatManager.Instance.Stats[i];
        }

        // 2. 스탯 매니저에게 데이터가 변화할 때마다 새로고침 함수를 호출해달라고 등록한다.
        // 옵저버 - 유튜브 구독 패턴
        StatManager.Instance.OnDataChangedCallback += Refresh;

        // ToDo: 하면 좋다.
         CurrencyManager.Instance.OnDataChangedCallback += Refresh;

        PlayerMove.Instance.OnDataChangedCallback += Refresh;

        // 3. 새로고침
        Refresh();
    }


    public void Refresh()
    {
        // 하위 버튼 UI들을 새로고침한다.
        for (int i = 0; i < (int)StatType.Count; ++i)
        {
            UI_StatButtons[i].Refresh();
        }
    }


}