using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class GachaInteractableUI : MonoBehaviour
{
    [SerializeField]
    private GachaResultPanelUI gachaResultPanel;
    [SerializeField]
    private GachaDescribeUI gachaDescribeUI;
    [SerializeField]
    private GachaPurchaseUI gachaPurchaseUI;
    [SerializeField]
    private Transform contentParent;
    [SerializeField]
    private List<Sprite> backgroundSprites = new List<Sprite>();

    private Dictionary<int, Sprite> gachaIdSpriteMapping = new Dictionary<int, Sprite>();
    private const string prefabFormat = "Prefabs/UI/SelectGachaButton";
    // Start is called before the first frame update
    void Awake()
    {
        Initialize();
    }
    private void OnDisable()
    {
        gachaResultPanel.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        if (GachaManager.useTicket)
        {
            GachaManager.ToggleUseTicket();
        }
    }
    public void Initialize()
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
                    gachaIdSpriteMapping.Add(gacha.Value.ID, backgroundSprites[completedCount]);
                }

                completedCount++;
                if(totalCount == completedCount)
                {
                    OnClickSelectGachaButton(gachaIdSpriteMapping.First().Key);
                }
            };
        }
    }

    public void OnClickSelectGachaButton(int gachaId)
    {
        gachaPurchaseUI.Initialize(DataTableManager.GachaTable.GetData(gachaId));
        gachaDescribeUI.Initialize(DataTableManager.GachaTable.GetData(gachaId), gachaIdSpriteMapping[gachaId]); //250331 HKY 데이터형 변경
    }
    public Sprite GetBackgroundSprite(int gachaId)
    {
        return gachaIdSpriteMapping[gachaId];
    }
}
