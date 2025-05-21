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
    [SerializeField]
    private GachaInteractableUI gachaInteractableUI;
    [SerializeField]
    private Image backgroundImage;
    [SerializeField]
    private GameObject skipRequestPanel;

    private const string prefabFormat = "Prefabs/UI/SoldierInfoImage";
    private WaitForSeconds waitSecondsToNextResult = new WaitForSeconds(0.05f);
    private Coroutine coDisplayResult;
    private bool skipRequested;

    private List<GameObject> gachaResults = new List<GameObject>();
    private void Awake()
    {
        closeButton.onClick.AddListener(() => gameObject.SetActive(false));
    }
    public void SetResult(List<SoldierTable.Data> datas, int gachaId)
    {
        skipRequestPanel.gameObject.SetActive(true);
        skipRequested = false;
        if (coDisplayResult != null)
        {
            StopCoroutine(coDisplayResult);
        }

        foreach (var obj in gachaResults)
        {
            obj.SetActive(false);
        }
        backgroundImage.sprite = gachaInteractableUI.GetBackgroundSprite(gachaId);
        coDisplayResult = StartCoroutine(DisplayResult(datas));
        gachaPurchaseUI.Initialize(DataTableManager.GachaTable.GetData(gachaId));
    }
    private IEnumerator DisplayResult(List<SoldierTable.Data> datas)
    {
        int i = 0;
        for (i = 0; i < datas.Count; i++)
        {
            if (skipRequested)
            {
                break;
            }
            SoundManager.Instance.PlaySFX("GachaSFX");
            InstantiateGachaResult(datas[i], i);
            yield return waitSecondsToNextResult;
        }

        for (; i < datas.Count; i++)
        {
            InstantiateGachaResult(datas[i], i);
        }
        skipRequested = false;
        skipRequestPanel.gameObject.SetActive(false);
        coDisplayResult = null;
    }
    private void InstantiateGachaResult(SoldierTable.Data data, int index)
    {
        GameObject obj;

        if (index < gachaResults.Count)
        {
            obj = gachaResults[index];
            obj.SetActive(true);
            InitializeSoldierInfoImage(obj, data);
        }
        else
        {
            Addressables.InstantiateAsync(prefabFormat, contentParent).Completed += (AsyncOperationHandle<GameObject> handle) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    var newObj = handle.Result;
                    newObj.transform.SetParent(contentParent, false);
                    gachaResults.Add(newObj);
                    InitializeSoldierInfoImage(newObj, data);
                }
                else
                {
                    Debug.LogError("Failed to instantiate prefab with key: " + prefabFormat);
                }
            };
        }
    }
    private void InitializeSoldierInfoImage(GameObject obj, SoldierTable.Data data)
    {
        var soldierInfoImage = obj.GetComponent<SoldierInfoImage>();
        if (soldierInfoImage != null)
        {
            soldierInfoImage.Initialize(data.Grade, data.Level, "", gradeSprites[(int)data.Grade - 1], data.SpriteID);
        }
    }
    public void OnClickSkip()
    {
        if(!skipRequested)
        {
            skipRequested = true;
            skipRequestPanel.gameObject.SetActive(false);
        }
    }
}
