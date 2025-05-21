using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGroupStatusManager : MonoBehaviour
{
    [field: SerializeField]
    [SerializedDictionary("Target Status", "UI Elements")]
    public SerializedDictionary<IngameStatus, UIGroupManager> UiDict { get; private set; }

    private IngameStatus ingameStatus = IngameStatus.Planet;

    public UIGroupManager UIGroupManager
    {
        get
        {
            if (UiDict.ContainsKey(ingameStatus))
            {
                return UiDict[ingameStatus];
            }
            return null;
        }
    }

    public void SetUIStatus(IngameStatus status)
    {
        if (ingameStatus == status)
        {
            return;
        }

        if (UiDict.ContainsKey(ingameStatus))
        {
            UiDict[ingameStatus].gameObject.SetActive(false);
        }

        if (UiDict.ContainsKey(status))
        {
            UiDict[status].gameObject.SetActive(true);
        }

        ingameStatus = status;
    }
}
