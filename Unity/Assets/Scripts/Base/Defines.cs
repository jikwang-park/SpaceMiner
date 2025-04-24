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

    public const string dungeonTable = "DungeonTable";
    public const string damageDungeonRewardTable = "DamageDungeonRewardTable";

    public const string soldierTable = "SoldierTable";
    public const string tankerSkillTable = "TankerSkillTable";
    public const string healerSkillTable = "HealerSkillTable";
    public const string dealerSkillTable = "DealerSkillTable";
    public const string unitUpgradeTable = "UnitUpgradeTable";
    public const string skillUpgradeTable = "SkillUpgradeTable";
    public const string buffTable = "BuffTable";

    public const string monsterTable = "MonsterTable";
    public const string monsterSkillTable = "MonsterSkillTable";
    public const string monsterRewardTable = "MonsterRewardTable";

    public const string gachaTable = "GachaTable";
    public const string gachaGradeTable = "GachaGradeTable";
    public const string gachaSoldierTable = "GachaSoldierTable";

    public const string itemTable = "ItemTable";
    public const string shopTable = "ShopTable";

    public const string defaultDataTable = "DefaultDataTable";
    public const string addressTable = "AddressTable";

    public const string guideQuestTable = "GuideQuestTable";

    public const string attendanceTable = "AttendanceTable";
    public const string attendanceRewardTable = "AttendanceRewardTable";

    public const string robotTable = "RobotTable";
    public const string robotMergeTable = "RobotMergeTable";

    public const string buildingTable = "BuildingTable";

    public const string planetTable = "PlanetTable";
}

public enum StageMode
{
    Ascend,
    Repeat,
}

public enum DungeonMode
{
    Ascend,
    End,
}

public enum Currency
{
    Gold = 1001,
    Annotaion,
    Cobalt,
    Tungsten,
    Titanium,
    Spinel,
}

public enum Grade
{
    None,
    Normal = 1,
    Rare,
    Epic,
    Legend,
}

public enum IngameStatus
{
    Planet,
    Dungeon,
    Mine,
}

public enum TargetPriority
{
    /// <summary>
    /// ÅÊÄ¿-µô·¯-Èú·¯
    /// </summary>
    FrontOrder = 1,
    /// <summary>
    /// Èú·¯-µô·¯-ÅÊÄ¿
    /// </summary>
    BackOrder = 2,
}

public enum SlotType
{
    Equip,
    Inventory,
}

public enum UnitTypes
{
    Tanker = 1,
    Dealer,
    Healer,
}