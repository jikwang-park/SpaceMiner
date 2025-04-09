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


    private void Awake()
    {
        InIt();
    }


    private void InIt()
    {
        var data = SaveLoadManager.Data.buildingData.buildingLevels;

        for (int i = (int)BuildingTable.BuildingType.IdleTime; i <= (int)BuildingTable.BuildingType.Mining; ++i)
        {
            var stats = Instantiate(element, parentTransform);
            stats.Init(DataTableManager.BuildingTable.GetDatas((BuildingTable.BuildingType)i));
            stats.SetData((BuildingTable.BuildingType)i, data[(BuildingTable.BuildingType)i]);
            //시퀀스에 따라서 순서 처리 해줘야됌
        }
    }
}
