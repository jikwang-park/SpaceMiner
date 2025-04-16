using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GachaDescribeUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI describeText;
    private AddressableImage addressableImage;
    private void Awake()
    {
        addressableImage = GetComponent<AddressableImage>();
    }
    public void Initialize(GachaTable.Data data)
    {

        describeText.text = DataTableManager.StringTable.GetData(data.DetailStringID); //250331 HKY 데이터형 변경
    }
}
