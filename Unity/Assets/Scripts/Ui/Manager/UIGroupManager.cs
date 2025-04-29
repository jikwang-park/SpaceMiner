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

    //TODO: ������ ����� �г��� �ν����Ϳ��� OnClick �̺�Ʈ�� ���� �� �г��ε��� �Ҵ�
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

    //TODO: �˾� ������� ����� �г��� �ν����Ϳ��� OnClick �̺�Ʈ�� ���� �� �г��ε��� �Ҵ�
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
