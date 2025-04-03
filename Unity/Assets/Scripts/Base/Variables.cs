using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Variables
{
    public static Languages currentLanguage = Languages.Korean;
    public static int planetNumber = 1;
    public static int stageNumber = 1;
    public static int maxPlanetNumber = 1;
    public static int maxStageNumber = 1;
    public static int selectedDungeonType = 1;
    public static int selectedDungeonStage = 1;

    public static bool isDungeonEnd = false;
    public static StageMode stageMode = StageMode.Ascend;
    public static DungeonMode dungeonMode = DungeonMode.End;
}
