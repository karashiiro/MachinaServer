namespace Sapphire.Common.Packets
{
    ////////////////////////////////////////////////////////////////////////////////
    /// Lobby Connection IPC Codes
    /**
    * Server IPC Lobby Type Codes.
    */
    enum ServerLobbyIpcTypeKR : ushort
    {
        LobbyError = 0x0002,
        LobbyServiceAccountList = 0x000C,
        LobbyCharList = 0x000D,
        LobbyCharCreate = 0x000E,
        LobbyEnterWorld = 0x000F,
        LobbyServerList = 0x0015,
        LobbyRetainerList = 0x0017,
    };
    /**
    * Client IPC Lobby Type Codes.
    */
    enum ClientLobbyIpcTypeKR : ushort
    {
        ReqCharList = 0x0003,
        ReqEnterWorld = 0x0004,
        ClientVersionInfo = 0x0005,
        ReqCharDelete = 0x000A,
        ReqCharCreate = 0x000B,
    };
    ////////////////////////////////////////////////////////////////////////////////
    /// Zone Connection IPC Codes
    /**
    * Server IPC Zone Type Codes.
    */
    enum ServerZoneIpcTypeKR : ushort
    {
        ActorCast = 0x022D, // updated 5.15
        ActorControl = 0x034B, // updated 5.15 (ActorControl142)
        ActorControlSelf = 0x030B, // updated 5.15 (ActorControl143)
        ActorControlTarget = 0x009B, // updated 5.15 (ActorControl144)
        ActorGauge = 0x035E, // updated 5.15
        ActorSetPos = 0x00DF, // updated 5.15
        AddStatusEffect = 0x00B1, // updated 5.15
        AoeEffect16 = 0x011F, // updated 5.15 (ActionEffect16)
        AoeEffect24 = 0x0134, // updated 5.15 (ActionEffect24)
        AoeEffect32 = 0x0244, // updated 5.15 (ActionEffect32)
        AoeEffect8 = 0x00E2, // updated 5.15 (ActionEffect8)
        //BossStatusEffectList
        CFNotify = 0x00A3, // updated 5.15
        CFPreferredRole = 0x0092, // updated 5.15
        CurrencyCrystalInfo = 0x0127, // updated 5.15
        Effect = 0x02E8, // updated 5.15 (ActionEffect)
        EventFinish = 0x0254, // updated 5.15
        EventPlay = 0x01F5, // updated 5.15
        EventPlay4 = 0x020D, // updated 5.15
        EventStart = 0x03B9, // updated 5.15
        Examine = 0x015C, // updated 5.15
        ExamineSearchInfo = 0x02B5, // updated 5.15
        InitZone = 0x01AE, // updated 5.15
        InventoryTransaction = 0x014F, // updated 5.15
        //InventoryTransactionFinish
        ItemInfo = 0x0356, // updated 5.15
        MarketBoardItemListing = 0x0223, // updated 5.15  (MarketBoardOfferings)
        MarketBoardItemListingCount = 0x013E, // updated 5.15 (MarketBoardItemRequestStart)
        MarketBoardItemListingHistory = 0x0183, // updated 5.15 (MarketBoardHistory)
        MarketBoardSearchResult = 0x025D, // updated 5.15
        MarketTaxRates = 0x0230, // updated 5.15
        NpcSpawn = 0x0100, // updated 5.15
        PlayerSetup = 0x01D6, // updated 5.15
        PlayerSpawn = 0x0110, // updated 5.15
        PlayerStats = 0x01A9, // updated 5.15
        Playtime = 0x02BF, // updated 5.15
        RetainerInformation = 0x008C, // updated 5.15
        SomeDirectorUnk4 = 0x024A, // updated 5.15
        StatusEffectList = 0x0118, // updated 5.15
        UpdateClassInfo = 0x0262, // updated 5.15
        UpdateHpMpTp = 0x03D3, // updated 5.15
        UpdateInventorySlot = 0x01CF, // updated 5.15
        UpdateSearchInfo = 0x0317, // updated 5.15
        UseMooch = 0x0082, // updated 5.15
    };
    /**
    * Client IPC Zone Type Codes.
    */
    enum ClientZoneIpcTypeKR : ushort
    {
        ChatHandler = 0x03CD, // updated 5.15
        ClientTrigger = 0x011E, // updated 5.15
        InventoryModifyHandler = 0x026F, // updated 5.15
        SetSearchInfoHandler = 0x0307, // updated 5.15
    };
    ////////////////////////////////////////////////////////////////////////////////
    /// Chat Connection IPC Codes
    /**
    * Server IPC Chat Type Codes.
    */
    enum ServerChatIpcTypeKR : ushort
    {
        //    Tell = 0x0064, // updated for sb
        TellErrNotFound = 0x0066,
        FreeCompanyEvent = 0x012C, // added 5.0
    };
    /**
    * Client IPC Chat Type Codes.
    */
    enum ClientChatIpcTypeKR : ushort
    {
        TellReq = 0x0064,
    };
}
