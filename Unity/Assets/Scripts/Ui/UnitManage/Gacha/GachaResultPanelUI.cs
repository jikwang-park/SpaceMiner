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
    private void Awake()
    {
        closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        UpdateGridCellSize();
    }
    public void SetResult(List<SoldierTable.Data> datas, int gachaId)
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
                        soldierInfoImage.Initialize("1", "", gradeSprites[(int)data.Rating - 1]);
                    }
                }
                else
                {
                    Debug.LogError("Failed to instantiate prefab with key: " + prefabFormat);
                }
            };
        }
        gachaPurchaseUI.Initialize(DataTableManager.GachaTable.GetData(gachaId));
    }

    public void UpdateGridCellSize()
    {
        GridLayoutGroup gridLayout = contentParent.GetComponent<GridLayoutGroup>();
        if (gridLayout == null)
        {
            return;
        }
        RectTransform rectTransform = contentParent.GetComponent<RectTransform>();
        float totalWidth = rectTransform.rect.width;
        int columns = gridLayout.constraintCount;
        int leftPadding = gridLayout.padding.left;
        int rightPadding = gridLayout.padding.right;
        float spacingX = gridLayout.spacing.x;

        float availableWidth = totalWidth - leftPadding - rightPadding - spacingX * (columns - 1);
        float cellSize = availableWidth / columns;
        gridLayout.cellSize = new Vector2(cellSize, cellSize);
    }
}
