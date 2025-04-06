using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGroupStatusManager : MonoBehaviour
{
    [SerializeField]
    [SerializedDictionary("Target Status", "UI Elements")]
    private SerializedDictionary<IngameStatus, UIGroupManager> uiDict;

    private IngameStatus ingameStatus = IngameStatus.Planet;

    public UIGroupManager UIGroupManager
    {
        get
        {
            if (uiDict.ContainsKey(ingameStatus))
            {
                return uiDict[ingameStatus];
            }
            return null;
        }
    }

    public void SetStatus(IngameStatus status)
    {
        if (ingameStatus == status)
        {
            return;
        }

        if (uiDict.ContainsKey(ingameStatus))
        {
            uiDict[ingameStatus].gameObject.SetActive(false);
        }

        if (uiDict.ContainsKey(status))
        {
            uiDict[status].gameObject.SetActive(true);
        }

        ingameStatus = status;
    }
}
