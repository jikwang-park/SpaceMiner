using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class BuildingPanel : MonoBehaviour
{
    [SerializeField]
    private BuildingDataElement element;
    [SerializeField]
    private Transform parentTransform;
    [SerializeField]
    private const string formatPath = "UI/BuildingElement.prefab";

    [SerializeField]
    private BuildingTable.BuildingType type;


    private void Start()
    {
        InIt();
    }


    private void InIt()
    {
        var data = SaveLoadManager.Data.buildingData.buildingLevels;

        for (int i = (int)BuildingTable.BuildingType.IdleTime; i <= (int)BuildingTable.BuildingType.Mining; ++i)
        {
            var currentType = (BuildingTable.BuildingType)i;
            Addressables.InstantiateAsync(formatPath, parentTransform).Completed += (AsyncOperationHandle<GameObject> handle) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    GameObject element = handle.Result;
                    BuildingDataElement buildingElement = element.GetComponent<BuildingDataElement>();
                    if (buildingElement != null)
                    {
                        buildingElement.Init(DataTableManager.BuildingTable.GetDatas(currentType));
                        buildingElement.SetData(currentType, data[currentType]);
                    }
                }
                else
                {
                    Debug.LogError("Failed key Address" + formatPath);
                }
            };
           
        }
    }
}
