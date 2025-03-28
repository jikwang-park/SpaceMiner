public enum Languages
{
    Korean,
    English,
    Japanese,
}

public class DataTableIds
{
    public static readonly string[] stringTables =
    {
        "StringTableKr",
        //"StringTableEn",
        //"StringTableJp",
    };
    
    public const string corpsTable = "CorpsTable";
    public const string stageTable = "StageTable";
    public const string waveTable = "WaveTable";

    public const string soldierTable = "SoldierTable";
    public const string tankerSkillTable = "TankerSkillTable";
    public const string healerSkillTable = "HealerSkillTable";
    public const string dealerSkillTable = "DealerSkillTable";

    public const string monsterTable = "MonsterTable";
    public const string monsterSkillTable = "MonsterSkillTable";
    public const string monsterRewardTable = "MonsterRewardTable";

    public const string gachaTable = "GachaTable";
    public const string gachaGradeTable = "GachaGradeTable";
    public const string gachaSoldierTable = "GachaSoldierTable";

    public const string itemTable = "ItemTable";
}

public enum StageMode
{
    Ascend,
    Repeat,
}