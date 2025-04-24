using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Variables
{
    public const float PlanetTime = 60f;

    public static Languages currentLanguage = Languages.Korean;

    public static int currentDungeonType = 1;
    public static int currentDungeonStage = 1;

    public static StageMode stageMode = StageMode.Ascend;
    public static DungeonMode dungeonMode = DungeonMode.End;

    public static int planetMiningID = 1;

    public static readonly BigNumber DefenceBase = 200;

    public static bool isAutoSkillMode = true;
    public static float healerSkillHPRatio = 0.5f;

}
