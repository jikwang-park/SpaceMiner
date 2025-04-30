using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonExterminateResult : MonoBehaviour
{
    private const string prefabAddress = "Assets/Addressables/Prefabs/UI/Stage/Dungeon/DungeonClearRewardIcon.prefab";

    [SerializeField]
    private Transform iconParent;

    private List<DungeonClearRewardIcon> rewardIcons = new List<DungeonClearRewardIcon>();

    private StageManager stageManager;

    private void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
    }

    private void OnDisable()
    {
        for (int i = 0; i < rewardIcons.Count; ++i)
        {
            rewardIcons[i].Release();
        }
        rewardIcons.Clear();
    }

    public void Open(SortedList<int, BigNumber> rewards)
    {
        gameObject.SetActive(true);

        foreach (var reward in rewards)
        {
            var itemIconGo = stageManager.StageUiManager.ObjectPoolManager.Get(prefabAddress);
            itemIconGo.transform.SetParent(iconParent);
            var rewardIcon = itemIconGo.GetComponent<DungeonClearRewardIcon>();
            rewardIcons.Add(rewardIcon);
            rewardIcon.SetItem(reward.Key, reward.Value);
        }
    }

    public void Open(int itemID, BigNumber count)
    {
        gameObject.SetActive(true);

        var itemIconGo = stageManager.StageUiManager.ObjectPoolManager.Get(prefabAddress);
        itemIconGo.transform.SetParent(iconParent);
        var rewardIcon = itemIconGo.GetComponent<DungeonClearRewardIcon>();
        rewardIcons.Add(rewardIcon);
        rewardIcon.SetItem(itemID, count);
    }
}
