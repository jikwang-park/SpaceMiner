using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class IdleRewardPopup : MonoBehaviour
{
    [SerializeField]
    private Transform contentParent;

    private string prefabFormat = "Prefabs/UI/IdleReward";
    public void DisplayIdleReward(List<KeyValuePair<int, BigNumber>> rewards)
    {
        foreach(var reward in rewards)
        {
            Addressables.InstantiateAsync(prefabFormat, contentParent).Completed += (AsyncOperationHandle<GameObject> handle) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    GameObject elementObj = handle.Result;
                    IdleRewardElement idleRewardElement = elementObj.GetComponent<IdleRewardElement>();
                    if(idleRewardElement != null)
                    {
                        idleRewardElement.Initialize(reward.Key, reward.Value);
                    }
                }
            };
        }
    }
    private void OnDisable()
    {
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }
    }
}
