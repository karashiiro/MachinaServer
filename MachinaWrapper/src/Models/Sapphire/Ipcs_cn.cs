// Generated by https://github.com/zhyupe/ffxiv-opcode-worker

namespace Sapphire.Common.Packets
{
    enum ServerZoneIpcTypeCN : ushort
    {
        ActorCast = 0x0146,
        ActorControl = 0x0229,
        ActorControlSelf = 0x025E,
        ActorGauge = 0x036A,
        ActorMove = 0x0202,
        ActorSetPos = 0x0158,
        AddStatusEffect = 0x02D0,
        AoeEffect16 = 0x021F,
        AoeEffect24 = 0x00BB,
        AoeEffect32 = 0x029F,
        AoeEffect8 = 0x034A,
        BossStatusEffectList = 0x0165,
        CFNotify = 0x034F,
        CFPreferredRole = 0x007A,
        Chat = 0x01f3,
        ContainerInfo = 0x02eb,
        CurrencyCrystalInfo = 0x0375,
        DirectorStart = 0x02de,
        Effect = 0x035E,
        EventFinish = 0x0103,
        EventPlay = 0x0350,
        EventPlay4 = 0x02A2,
        EventStart = 0x00F2,
        Examine = 0x0174,
        ExamineSearchInfo = 0x0201,
        GroupMessage = 0x0065,
        InitZone = 0x013C,
        InventoryActionAck = 0x009a,
        InventoryTransaction = 0x011E,
        InventoryTransactionFinish = 0x01f0,
        ItemInfo = 0x02F0,
        MarketBoardItemListing = 0x0175,
        MarketBoardItemListingCount = 0x0070,
        MarketBoardItemListingHistory = 0x031D,
        MarketBoardSearchResult = 0x00EF,
        MarketTaxRates = 0x01F5,
        NpcSpawn = 0x0211,
        NpcSpawn2 = 0x03c9,
        PlayerSetup = 0x02DD,
        PlayerSpawn = 0x03A0,
        PlayerStats = 0x00A7,
        Playtime = 0x0152,
        RetainerInformation = 0x0179,
        SomeDirectorUnk4 = 0x0228,
        StatusEffectList = 0x00F0,
        UpdateClassInfo = 0x01B8,
        UpdateHpMpTp = 0x0237,
        UpdateInventorySlot = 0x03c8,
        UpdateSearchInfo = 0x014F,
    };

    enum ClientZoneIpcTypeCN : ushort
    {
        ChatHandler = 0x0132,
        ClientTrigger = 0x0204,
        InventoryModifyHandler = 0x0066,
        SetSearchInfoHandler = 0x00D0,
    };

    enum ServerChatIpcTypeCN : ushort
    {

    };

    enum ClientChatIpcTypeCN : ushort
    {

    };
}
