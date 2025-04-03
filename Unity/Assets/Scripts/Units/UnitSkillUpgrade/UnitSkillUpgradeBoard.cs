using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UnitSkillUpgradeBoard : MonoBehaviour
{
    private UnitSkillUpgradeManager manager;

    private UnitSkillUpgradeElement element;

    [SerializeField]
    private Image currentImage;
    [SerializeField]
    private TextMeshProUGUI currentText;
    [SerializeField]
    private Image nextImage;
    [SerializeField]
    private TextMeshProUGUI nextText;


    private int id;
    private void Start()
    {
    }
    public void ShowFirstOpened()
    {
        var data = DataTableManager.SkillUpgradeTable.GetData(1);
        // 처음 스킬 업그레이드 버튼 클릭했을시에 탱커 -> 노말 기준으로 뜨게 여기서 설정해야됌

    }
    public void ShowBoard()
    {

    }

    public void SetInfo(int id)
    {
        var data = DataTableManager.SkillUpgradeTable.GetData(id);
    }

    public void SetBoardText(int id)
    {
        var data = DataTableManager.SkillUpgradeTable.GetData(id);

        currentText.text = $"";
        
    }

    public void SetImage()
    {

    }
}
