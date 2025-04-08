using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [SerializeField]
    private BuildingDataElement element;
    [SerializeField]
    private Transform parentTransform;

    [SerializeField]
    private BuildingTable.BuildingType type;

    public Dictionary<BuildingTable.BuildingType, Dictionary<int, int>> constructionDic;




    private void InIt()
    {
        for (int i = (int)BuildingTable.BuildingType.IdleTime; i <= (int)BuildingTable.BuildingType.Mining; ++i)
        {
            var stats = Instantiate(element, parentTransform);
            stats.Init(DataTableManager.BuildingTable.GetDatas((BuildingTable.BuildingType)i));
            //�������� ���� ���� ó�� ����߉�
        }
    }
}
