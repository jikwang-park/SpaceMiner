using UnityEngine;

public class UIGroupManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] tabs;
    [SerializeField]
    private GameObject[] popups;

    private int currentTab = 0;

    private void Start()
    {
        if (tabs.Length > 0)
        {
            tabs[currentTab].gameObject.SetActive(true);
        }
    }

    //TODO: 탭으로 사용할 패널을 인스펙터에서 OnClick 이벤트에 연결 후 패널인덱스 할당
    public void SetTabActive(int index)
    {
        if (index < 0 || index >= tabs.Length)
        {
            return;
        }
        if (currentTab == index)
        {
            return;
        }

        tabs[currentTab].gameObject.SetActive(false);
        tabs[index].gameObject.SetActive(true);
        currentTab = index;
    }

    //TODO: 팝업 윈도우로 사용할 패널을 인스펙터에서 OnClick 이벤트에 연결 후 패널인덱스 할당
    public void SetPopUpActive(int index)
    {
        if (index < 0 || index >= popups.Length)
        {
            return;
        }

        popups[index].gameObject.SetActive(true);
    }

    public void SetPopUpInactive(int index)
    {
        if (index < 0 || index >= popups.Length)
        {
            return;
        }

        popups[index].gameObject.SetActive(false);
    }

    public void SetTimeScale()
    {
        if (Time.timeScale <= 0f)
        {
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 0f;
        }
    }
}
