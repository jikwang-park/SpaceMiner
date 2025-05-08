using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;

public static class DataTableManager
{
    private static readonly Dictionary<string, DataTable> tables = new Dictionary<string, DataTable>();

    public static Dictionary<string, DataTable> Tables => tables;

    static DataTableManager()
    {
        LoadTables();
    }

    private static void LoadTables()
    {
#if UNITY_EDITOR
        foreach (var id in DataTableIds.stringTables)
        {
            var table = new StringTable();
            table.Load(id);
            tables.Add(id, table);
        }
#else
        var table = new StringTable();
        var stringTableId = DataTableIds.stringTables[(int)Variables.currentLanguage];
        table.Load(stringTableId);
        tables.Add(stringTableId, table);
#endif

        var corpsTable = new CorpsTable();
        corpsTable.Load(DataTableIds.corpsTable);
        tables.Add(DataTableIds.corpsTable, corpsTable);

        var stageTable = new StageTable();
        stageTable.Load(DataTableIds.stageTable);
        tables.Add(DataTableIds.stageTable, stageTable);

        var waveTable = new WaveTable();
        waveTable.Load(DataTableIds.waveTable);
        tables.Add(DataTableIds.waveTable, waveTable);

        var dungeonTable = new DungeonTable();
        dungeonTable.Load(DataTableIds.dungeonTable);
        tables.Add(DataTableIds.dungeonTable, dungeonTable);



        var soldierTable = new SoldierTable();
        soldierTable.Load(DataTableIds.soldierTable);
        tables.Add(DataTableIds.soldierTable, soldierTable);

        var tankerSkillTable = new TankerSkillTable();
        tankerSkillTable.Load(DataTableIds.tankerSkillTable);
        tables.Add(DataTableIds.tankerSkillTable, tankerSkillTable);

        var healerSkillTable = new HealerSkillTable();
        healerSkillTable.Load(DataTableIds.healerSkillTable);
        tables.Add(DataTableIds.healerSkillTable, healerSkillTable);

        var dealerSkillTable = new DealerSkillTable();
        dealerSkillTable.Load(DataTableIds.dealerSkillTable);
        tables.Add(DataTableIds.dealerSkillTable, dealerSkillTable);

        var unitUpgradeTable = new UnitUpgradeTable();
        unitUpgradeTable.Load(DataTableIds.unitUpgradeTable);
        tables.Add(DataTableIds.unitUpgradeTable, unitUpgradeTable);

        var skillUpgradeTable = new SkillUpgradeTable();
        skillUpgradeTable.Load(DataTableIds.skillUpgradeTable);
        tables.Add(DataTableIds.skillUpgradeTable, skillUpgradeTable);

        var buffTable = new BuffTable();
        buffTable.Load(DataTableIds.buffTable);
        tables.Add(DataTableIds.buffTable, buffTable);


        var monsterTable = new MonsterTable();
        monsterTable.Load(DataTableIds.monsterTable);
        tables.Add(DataTableIds.monsterTable, monsterTable);

        var monsterSkillTable = new MonsterSkillTable();
        monsterSkillTable.Load(DataTableIds.monsterSkillTable);
        tables.Add(DataTableIds.monsterSkillTable, monsterSkillTable);

        var monsterRewardTable = new MonsterRewardTable();
        monsterRewardTable.Load(DataTableIds.monsterRewardTable);
        tables.Add(DataTableIds.monsterRewardTable, monsterRewardTable);



        var gachaTable = new GachaTable();
        gachaTable.Load(DataTableIds.gachaTable);
        tables.Add(DataTableIds.gachaTable, gachaTable);

        var gachaGradeTable = new GachaGradeTable();
        gachaGradeTable.Load(DataTableIds.gachaGradeTable);
        tables.Add(DataTableIds.gachaGradeTable, gachaGradeTable);

        var gachaSoldierTable = new GachaSoldierTable();
        gachaSoldierTable.Load(DataTableIds.gachaSoldierTable);
        tables.Add(DataTableIds.gachaSoldierTable, gachaSoldierTable);


        var itemTable = new ItemTable();
        itemTable.Load(DataTableIds.itemTable);
        tables.Add(DataTableIds.itemTable, itemTable);

        var shopTable = new ShopTable();
        shopTable.Load(DataTableIds.shopTable);
        tables.Add(DataTableIds.shopTable, shopTable);



        var defaultDataTable = new DefaultDataTable();
        defaultDataTable.Load(DataTableIds.defaultDataTable);
        tables.Add(DataTableIds.defaultDataTable, defaultDataTable);

        var addressTable = new AddressTable();
        addressTable.Load(DataTableIds.addressTable);
        tables.Add(DataTableIds.addressTable, addressTable);



        var guideQuestTable = new GuideQuestTable();
        guideQuestTable.Load(DataTableIds.guideQuestTable);
        tables.Add(DataTableIds.guideQuestTable, guideQuestTable);


        var robotTable = new RobotTable();
        robotTable.Load(DataTableIds.robotTable);
        tables.Add(DataTableIds.robotTable, robotTable);

        var robotMergeTable = new RobotMergeTable();
        robotMergeTable.Load(DataTableIds.robotMergeTable);
        tables.Add(DataTableIds.robotMergeTable, robotMergeTable);


        var buildingTable = new BuildingTable();
        buildingTable.Load(DataTableIds.buildingTable);
        tables.Add(DataTableIds.buildingTable, buildingTable);


        var planetTable = new PlanetTable();
        planetTable.Load(DataTableIds.planetTable);
        tables.Add(DataTableIds.planetTable, planetTable);

        var miningBattleTable = new MiningBattleTable();
        miningBattleTable.Load(DataTableIds.miningBattleTable);
        tables.Add(DataTableIds.miningBattleTable, miningBattleTable);

        var miningBattleSpawnTable = new MiningBattleSpawnTable();
        miningBattleSpawnTable.Load(DataTableIds.miningBattleSpawnTable);
        tables.Add(DataTableIds.miningBattleSpawnTable, miningBattleSpawnTable);


        var damageDungeonRewardTable = new DamageDungeonRewardTable();
        damageDungeonRewardTable.Load(DataTableIds.damageDungeonRewardTable);
        tables.Add(DataTableIds.damageDungeonRewardTable, damageDungeonRewardTable);


        var attendanceTable = new AttendanceTable();
        attendanceTable.Load(DataTableIds.attendanceTable);
        tables.Add(DataTableIds.attendanceTable, attendanceTable);

        var attendanceRewardTable = new AttendanceRewardTable();
        attendanceRewardTable.Load(DataTableIds.attendanceRewardTable);
        tables.Add(DataTableIds.attendanceRewardTable, attendanceRewardTable);


        var tutorialTable = new TutorialTable();
        tutorialTable.Load(DataTableIds.tutorialTable);
        tables.Add(DataTableIds.tutorialTable, tutorialTable);
    }


    public static T GetTable<T>(string tableId) where T : DataTable
    {
        bool isContainsKey = tables.ContainsKey(tableId);
        if (!isContainsKey)
        {
            Debug.Assert(isContainsKey, "Table Not Exists");
            return null;
        }
        return tables[tableId] as T;
    }

    public static StringTable StringTable
        => GetTable<StringTable>(DataTableIds.stringTables[(int)Variables.currentLanguage]);

    public static CorpsTable CorpsTable
        => GetTable<CorpsTable>(DataTableIds.corpsTable);

    public static StageTable StageTable
        => GetTable<StageTable>(DataTableIds.stageTable);

    public static WaveTable WaveTable
        => GetTable<WaveTable>(DataTableIds.waveTable);

    public static SoldierTable SoldierTable
        => GetTable<SoldierTable>(DataTableIds.soldierTable);

    public static TankerSkillTable TankerSkillTable
         => GetTable<TankerSkillTable>(DataTableIds.tankerSkillTable);

    public static HealerSkillTable HealerSkillTable
        => GetTable<HealerSkillTable>(DataTableIds.healerSkillTable);

    public static DealerSkillTable DealerSkillTable
        => GetTable<DealerSkillTable>(DataTableIds.dealerSkillTable);

    public static MonsterTable MonsterTable
        => GetTable<MonsterTable>(DataTableIds.monsterTable);

    public static MonsterSkillTable MonsterSkillTable
        => GetTable<MonsterSkillTable>(DataTableIds.monsterSkillTable);

    public static MonsterRewardTable MonsterRewardTable
        => GetTable<MonsterRewardTable>(DataTableIds.monsterRewardTable);

    public static GachaTable GachaTable
        => GetTable<GachaTable>(DataTableIds.gachaTable);

    public static GachaGradeTable GachaGradeTable
        => GetTable<GachaGradeTable>(DataTableIds.gachaGradeTable);

    public static GachaSoldierTable GachaSoldierTable
        => GetTable<GachaSoldierTable>(DataTableIds.gachaSoldierTable);

    public static ItemTable ItemTable
        => GetTable<ItemTable>(DataTableIds.itemTable);

    public static UnitUpgradeTable UnitUpgradeTable
        => GetTable<UnitUpgradeTable>(DataTableIds.unitUpgradeTable);

    public static ShopTable ShopTable
        => GetTable<ShopTable>(DataTableIds.shopTable);

    public static DungeonTable DungeonTable
        => GetTable<DungeonTable>(DataTableIds.dungeonTable);

    public static SkillUpgradeTable SkillUpgradeTable
        => GetTable<SkillUpgradeTable>(DataTableIds.skillUpgradeTable);

    public static DefaultDataTable DefaultDataTable
        => GetTable<DefaultDataTable>(DataTableIds.defaultDataTable);

    public static GuideQuestTable GuideQuestTable
        => GetTable<GuideQuestTable>(DataTableIds.guideQuestTable);

    public static RobotTable RobotTable
        => GetTable<RobotTable>(DataTableIds.robotTable);

    public static RobotMergeTable RobotMergeTable
        => GetTable<RobotMergeTable>(DataTableIds.robotMergeTable);

    public static BuildingTable BuildingTable
        => GetTable<BuildingTable>(DataTableIds.buildingTable);

    public static PlanetTable PlanetTable
        => GetTable<PlanetTable>(DataTableIds.planetTable);

    public static AddressTable AddressTable
        => GetTable<AddressTable>(DataTableIds.addressTable);

    public static BuffTable BuffTable
        => GetTable<BuffTable>(DataTableIds.buffTable);

    public static DamageDungeonRewardTable DamageDungeonRewardTable
        => GetTable<DamageDungeonRewardTable>(DataTableIds.damageDungeonRewardTable);

    public static AttendanceTable AttendanceTable
        => GetTable<AttendanceTable>(DataTableIds.attendanceTable);

    public static AttendanceRewardTable AttendanceRewardTable
        => GetTable<AttendanceRewardTable>(DataTableIds.attendanceRewardTable);

    public static MiningBattleTable MiningBattleTable
        => GetTable<MiningBattleTable>(DataTableIds.miningBattleTable);

    public static MiningBattleSpawnTable MiningBattleSpawnTable
        => GetTable<MiningBattleSpawnTable>(DataTableIds.miningBattleSpawnTable);

    public static TutorialTable TutorialTable
        => GetTable<TutorialTable>(DataTableIds.tutorialTable);
}
