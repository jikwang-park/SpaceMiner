using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingBoardUI : MonoBehaviour
{
    [SerializeField]
    private List<RankingElement> rankingElements = new List<RankingElement>();

    public void Initialize(List<LeaderBoardEntry> list, RankingType type)
    {
        foreach(var element in rankingElements)
        {
            element.gameObject.SetActive(false);
        }

        int count = Mathf.Min(list.Count, rankingElements.Count);

        for(int i = 0; i < count; i++)
        {
            var element = rankingElements[i];
            element.gameObject.SetActive(true);
            element.SetInfo(i + 1, list[i], type);
        }
    }
    public void UpdateElement(int rank)
    {
        // rankingElements[rank].UpdateBackground(true);
    }
}
