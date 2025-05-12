using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> sprites = new List<Sprite>();
    [SerializeField]
    private AddressableImage image;
    [SerializeField]
    private TextMeshProUGUI scoreText;

    private readonly int combatPowerIconId = 10301;
    private readonly int dungeonDamageIconId = 133005;
    public void SetBoard(RankingType type, string score)
    {
        if(type == RankingType.Stage)
        {
            var stageData = DataTableManager.StageTable.GetData(int.Parse(score));
            var spriteId = DataTableManager.PlanetTable.GetData(stageData.Planet).SpriteID;
            scoreText.text = $"{stageData.Planet}-{stageData.Stage}";
            image.SetSprite(spriteId);
            return;
        }
        else
        {
            if(type == RankingType.CombatPower)
            {
                image.SetSprite(combatPowerIconId);
            }
            else
            {
                image.SetSprite(dungeonDamageIconId);
            }
            scoreText.text = score;
            return;
        }
    }
}
