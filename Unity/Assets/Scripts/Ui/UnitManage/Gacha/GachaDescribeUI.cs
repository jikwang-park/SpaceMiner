using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GachaDescribeUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI describeText;

    public void Initialize(GachaTable.Data data)
    {
        describeText.text = DataTableManager.StringTable.GetData(data.explainStringID); //250331 HKY 데이터형 변경
    }
}
