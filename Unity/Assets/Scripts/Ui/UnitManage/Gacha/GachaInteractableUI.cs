using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class GachaInteractableUI : MonoBehaviour
{
    [SerializeField]
    private GachaDescribeUI gachaDescribeUI;
    [SerializeField]
    private Transform contentParent;
    private const string prefabFormat = "Prefabs/UI/SelectGachaButton";
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        var gachas = DataTableManager.GachaTable.GetDict();

        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }
        var totalCount = gachas.Count;
        var completedCount = 0;
        foreach(var gacha in gachas)
        {
            Addressables.InstantiateAsync(prefabFormat, contentParent).Completed += (AsyncOperationHandle<GameObject> handle) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    GameObject elementObj = handle.Result;
                    SelectGachaButton selectGachaButton = elementObj.GetComponent<SelectGachaButton>();
                    selectGachaButton.Initialize(gacha.Value);
                    selectGachaButton.parent = this;
                }

                completedCount++;
                if(totalCount == completedCount)
                {
                    OnClickSelectGachaButton("1");
                }
            };
        }
    }

    public void OnClickSelectGachaButton(string gachaId)
    {
        gachaDescribeUI.Initialize(DataTableManager.GachaTable.GetData(gachaId));
    }
}
