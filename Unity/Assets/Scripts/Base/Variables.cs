using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Variables
{
    public static Languages currentLanguage = Languages.Korean;

    public static int currentDungeonType = 1;
    public static int currentDungeonStage = 1;

    public static bool isDungeonEnd = false;
    public static StageMode stageMode = StageMode.Ascend;
    public static DungeonMode dungeonMode = DungeonMode.End;
}
