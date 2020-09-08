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
        ActorCast = 0x02B1, // updated 5.2
        ActorControl = 0x029C, // updated 5.2
        ActorControlSelf = 0x00AB, // updated 5.2
        ActorControlTarget = 0x00AD, // updated 5.2
        ActorGauge = 0x014F, // updated 5.2
        ActorSetPos = 0x0083, // updated 5.2
        AddStatusEffect = 0x0164, // updated 5.2
        AoeEffect16 = 0x0210, // updated 5.2
        AoeEffect24 = 0x024F, // updated 5.2
        AoeEffect32 = 0x00F2, // updated 5.2
        AoeEffect8 = 0x00B4, // updated 5.2
        CFNotify = 0x00FE, // updated 5.2
        CFPreferredRole = 0x0324, // updated 5.2
        CurrencyCrystalInfo = 0x0247, // updated 5.2
        Effect = 0x0073, // updated 5.2
        EventFinish = 0x0171, // updated 5.2
        EventPlay = 0x0113, // updated 5.2
        EventPlay4 = 0x03DA, // updated 5.2
        EventStart = 0x0068, // updated 5.2
        Examine = 0x008E, // updated 5.2
        ExamineSearchInfo = 0x0146, // updated 5.2
        InitZone = 0x039D, // updated 5.2
        InventoryTransaction = 0x02FE, // updated 5.2
        InventoryTransactionFinish = 0x0121, // updated 5.2
        ItemInfo = 0x01D8, // updated 5.2
        MarketBoardItemListing = 0x0190, // updated 5.2
        MarketBoardItemListingCount = 0x02A7, // updated 5.2
        MarketBoardItemListingHistory = 0x0362, // updated 5.2
        MarketBoardSearchResult = 0x0187, // updated 5.2
        MarketTaxRates = 0x02D7, // updated 5.2
        NpcSpawn = 0x030E, // updated 5.2
        PlayerSetup = 0x02E7, // updated 5.2
        PlayerSpawn = 0x03CC, // updated 5.2
        PlayerStats = 0x0088, // updated 5.2
        Playtime = 0x008D, // updated 5.2
        RetainerInformation = 0x01D2, // updated 5.2
        SomeDirectorUnk4 = 0x02B2, // updated 5.2
        StatusEffectList = 0x0166, // updated 5.2
        UpdateClassInfo = 0x0158, // updated 5.2
        UpdateHpMpTp = 0x02A2, // updated 5.2
        UpdateInventorySlot = 0x0349, // updated 5.2
        UpdateSearchInfo = 0x027E, // updated 5.2
    };
    /**
    * Client IPC Zone Type Codes.
    */
    enum ClientZoneIpcTypeKR : ushort
    {
        ChatHandler = 0x00DC, // updated 5.2
        ClientTrigger = 0x02DC, // updated 5.2
        InventoryModifyHandler = 0x03A3, // updated 5.2
        SetSearchInfoHandler = 0x02E3, // updated 5.2
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
