using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;





public class UnitStatsUpgrade : MonoBehaviour
{
    public UnitStatsUpgradeElement statsUpgradeElements;

    private const string prefabFormat = "Prefabs/UI/Stats";
    

    [SerializeField]
    private Transform parentTransform;

    [SerializeField]
    private List<Sprite> statsSprite;

    [SerializeField]
    private Toggle x1Toggle;
    [SerializeField]
    private Toggle x10Toggle;
    [SerializeField]
    private Toggle x100Toggle;

    public int currentMultiplier { get; private set; } = 1;
    private List<UnitStatsUpgradeElement> allElements = new List<UnitStatsUpgradeElement>();

    private void OnEnable()
    {
        x1Toggle.isOn = false;
        x10Toggle.isOn = false;
        x100Toggle.isOn = false;
        x1Toggle.isOn = true;
    }

    private void Awake()
    {

        x1Toggle.onValueChanged.AddListener((isOn) => { if (isOn) SetMultiplier(1); });
        x10Toggle.onValueChanged.AddListener((isOn) => { if (isOn) SetMultiplier(10); });
        x100Toggle.onValueChanged.AddListener((isOn) => { if (isOn) SetMultiplier(100); });
        Init();
    }

    private void SetMultiplier(int multiplier)
    {
        currentMultiplier = multiplier;
        foreach(var element in allElements)
        {
            element.SetMultiplier(multiplier);
        }
    }
    private void Init()
    {

        var data = SaveLoadManager.Data.unitStatUpgradeData.upgradeLevels;
        for (int i = (int)UnitUpgradeTable.UpgradeType.AttackPoint; i <= (int)UnitUpgradeTable.UpgradeType.CriticalDamages; i++)
        {
            var currentType = (UnitUpgradeTable.UpgradeType)i;
            Addressables.InstantiateAsync(prefabFormat, parentTransform).Completed += (AsyncOperationHandle<GameObject> handle) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {

                    GameObject element = handle.Result;
                    UnitStatsUpgradeElement statsElement = element.GetComponent<UnitStatsUpgradeElement>();
                    if (statsElement != null)
                    {
                        statsElement.SetData(data[currentType], DataTableManager.UnitUpgradeTable.GetData(currentType));
                        statsElement.SetInitString(currentType);
                        statsElement.SetImage(currentType, statsSprite);
                        statsElement.SetMultiplier(currentMultiplier);
                        allElements.Add(statsElement);
                    }

                }
                else
                {
                    Debug.LogError("Failed Key address " + prefabFormat);
                }
            };
        };
      
    }
}
