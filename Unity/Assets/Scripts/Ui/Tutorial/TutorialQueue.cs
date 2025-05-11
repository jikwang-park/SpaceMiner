using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialQueue : MonoBehaviour
{
    [SerializeField]
    private RectTransform sideMenuOpenButton;

    [SerializeField]
    private GameObject maximizedSidebar;

    private Queue<(RectTransform rect, bool isSideBar)> queue = new Queue<(RectTransform rect, bool isSideBar)>();

    [field: SerializeField]
    public TutorialUIBlocker TutorialUIBlocker { get; private set; }

    [SerializeField]
    private TutorialPage tutorialPage;

    private void Update()
    {
        if (tutorialPage.gameObject.activeInHierarchy)
        {
            return;
        }
        if (queue.Count > 0)
        {
            if (queue.Peek().isSideBar)
            {
                if (!maximizedSidebar.activeInHierarchy && !TutorialUIBlocker.isShowing)
                {
                    TutorialUIBlocker.SetBlock(sideMenuOpenButton);
                    TutorialUIBlocker.ShowArrow(TutorialUIBlocker.Position.Down);
                }
                if (maximizedSidebar.activeInHierarchy && TutorialUIBlocker.isShowing)
                {
                    TutorialUIBlocker.Close();
                    TutorialUIBlocker.SetBlock(queue.Dequeue().rect);
                    TutorialUIBlocker.ShowArrow(TutorialUIBlocker.Position.Down);
                }
            }
            else if (!TutorialUIBlocker.isShowing)
            {
                TutorialUIBlocker.SetBlock(queue.Dequeue().rect);
                TutorialUIBlocker.ShowArrow(TutorialUIBlocker.Position.Up);
            }
        }
    }

    public void EnqueueTutorial(RectTransform rectTransform, bool isSideBar)
    {
        queue.Enqueue((rectTransform, isSideBar));
    }
}
