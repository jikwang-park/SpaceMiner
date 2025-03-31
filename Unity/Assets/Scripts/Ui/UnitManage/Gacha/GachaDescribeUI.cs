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
        describeText.text = data.explainStringID.ToString(); //250331 HKY �������� ����
    }
}
