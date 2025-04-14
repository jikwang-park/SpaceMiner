using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class GachaResultPanelUI : MonoBehaviour
{
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private Transform contentParent;
    [SerializeField]
    private List<Sprite> gradeSprites;
    [SerializeField]
    private GachaPurchaseUI gachaPurchaseUI;

    private const string prefabFormat = "Prefabs/UI/SoldierInfoImage";
    private WaitForSeconds waitSecondsToNextResult = new WaitForSeconds(0.05f);
    private Coroutine coDisplayResult;
    private void Awake()
    {
        closeButton.onClick.AddListener(() => gameObject.SetActive(false));
    }
    public void SetResult(List<SoldierTable.Data> datas, int gachaId)
    {
        if(coDisplayResult != null)
        {
            StopCoroutine(coDisplayResult);
        }
        coDisplayResult = StartCoroutine(DisplayResult(datas));
        gachaPurchaseUI.Initialize(DataTableManager.GachaTable.GetData(gachaId));
    }
    private IEnumerator DisplayResult(List<SoldierTable.Data> datas)
    {
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var data in datas)
        {
            var soldierData = data;

            Addressables.InstantiateAsync(prefabFormat, contentParent).Completed += (AsyncOperationHandle<GameObject> handle) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    GameObject elementObj = handle.Result;
                    SoldierInfoImage soldierInfoImage = elementObj.GetComponent<SoldierInfoImage>();
                    if (soldierInfoImage != null)
                    {
                        soldierInfoImage.Initialize(data.Level.ToString(), "", gradeSprites[(int)data.Grade - 1]);
                    }
                }
                else
                {
                    Debug.LogError("Failed to instantiate prefab with key: " + prefabFormat);
                }
            };
            yield return waitSecondsToNextResult;
        }
    }
}
