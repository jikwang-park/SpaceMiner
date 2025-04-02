using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGroupManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] tabs;
    [SerializeField]
    private GameObject[] popups;

    private int currentTab = 0;

    [field: SerializeField]
    public StageSelectWindow StageSelectWindow { get; private set; }

    //TODO: ������ ����� �г��� �ν����Ϳ��� OnClick �̺�Ʈ�� ���� �� �г��ε��� �Ҵ�
    public void SetPanelActive(int index)
    {
        if (tabs.Length <= index)
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
        if (tabs.Length <= index)
        {
            return;
        }

        popups[index].gameObject.SetActive(true);
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
