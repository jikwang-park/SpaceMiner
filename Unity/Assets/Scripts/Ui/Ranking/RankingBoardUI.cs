using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingBoardUI : MonoBehaviour
{
    [SerializeField]
    private List<RankingElement> rankingElements = new List<RankingElement>();

    public void Initialize(List<LeaderBoardEntry> list, RankingType type, int myRank = -1)
    {
        for (int i = 0; i < rankingElements.Count; i++)
        {
            var e = rankingElements[i];
            if (i < list.Count)
            {
                e.gameObject.SetActive(true);
                bool isMine = (i + 1) == myRank;
                e.SetInfo(i + 1, list[i], type, isMine);
            }
            else
            {
                e.gameObject.SetActive(false);
            }
        }
    }
    public void UpdateElement(int rank)
    {
        rankingElements[rank - 1].UpdateBackground(true);
    }
}
