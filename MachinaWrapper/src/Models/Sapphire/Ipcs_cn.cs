namespace Sapphire.Common.Packets
{
    ////////////////////////////////////////////////////////////////////////////////
    /// Lobby Connection IPC Codes
    /**
    * Server IPC Lobby Type Codes.
    */
    enum ServerLobbyIpcTypeCN : ushort
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
    enum ClientLobbyIpcTypeCN : ushort
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
    enum ServerZoneIpcTypeCN : ushort
    {
        ActorCast = 0x008C, // updated 5.18
        ActorControl = 0x03CC, // updated 5.18
        ActorControlSelf = 0x0398, // updated 5.18
        // ActorControlTarget
        ActorGauge = 0x03DB, // updated 5.18
        ActorSetPos = 0x0067, // updated 5.18
        AddStatusEffect = 0x0251, // updated 5.18
        AoeEffect16 = 0x0245, // updated 5.18
        AoeEffect24 = 0x01E5, // updated 5.18
        AoeEffect32 = 0x01B2, // updated 5.18
        AoeEffect8 = 0x0219, // updated 5.18
        BossStatusEffectList = 0x023b, // updated 5.18
        CFNotify = 0x021C, // updated 5.18
        CFPreferredRole = 0x0256, // updated 5.18
        Chat = 0x02AE, // updated 5.18
        ContainerInfo = 0x026F, // updated 5.18
        CurrencyCrystalInfo = 0x0217, // updated 5.18
        Effect = 0x00D5, // updated 5.18
        EventFinish = 0x0320, // updated 5.18
        EventPlay = 0x01DB, // updated 5.18
        EventPlay4 = 0x02C1, // updated 5.18
        EventStart = 0x00CF, // updated 5.18
        Examine = 0x038D, // updated 5.18
        ExamineSearchInfo = 0x022A, // updated 5.18
        InitZone = 0x0264, // updated 5.18
        InventoryTransaction = 0x028B, // updated 5.18
        ItemInfo = 0x0387, // updated 5.18
        MarketBoardItemListing = 0x02D7, // updated 5.18
        MarketBoardItemListingCount = 0x0231, // updated 5.18
        MarketBoardItemListingHistory = 0x00DF, // updated 5.18
        MarketBoardSearchResult = 0x01C5, // updated 5.18
        MarketTaxRates = 0x019F, // updated 5.18
        NpcSpawn = 0x0280, // updated 5.18
        NpcSpawn2 = 0x02e0, // updated 5.18
        PlayerSetup = 0x02C6, // updated 5.18
        PlayerSpawn = 0x0116, // updated 5.18
        PlayerStats = 0x0346, // updated 5.18
        Playtime = 0x03BD, // updated 5.18
        RetainerInformation = 0x00B5, // updated 5.18
        SomeDirectorUnk4 = 0x03E7, // updated 5.18
        StatusEffectList = 0x021F, // updated 5.18
        UpdateClassInfo = 0x0096, // updated 5.18
        UpdateHpMpTp = 0x01F6, // updated 5.18
        UpdateInventorySlot = 0x01A7, // updated 5.18
        UpdateSearchInfo = 0x00B2, // updated 5.18
    };
    /**
    * Client IPC Zone Type Codes.
    */
    enum ClientZoneIpcTypeCN : ushort
    {
        ChatHandler = 0x03A8, // updated 5.18
        ClientTrigger = 0x0233, // updated 5.18
        InventoryModifyHandler = 0x0291, // updated 5.18
        SetSearchInfoHandler = 0x02BF, // updated 5.18
    };
    ////////////////////////////////////////////////////////////////////////////////
    /// Chat Connection IPC Codes
    /**
    * Server IPC Chat Type Codes.
    */
    enum ServerChatIpcTypeCN : ushort
    {
        //    Tell = 0x0064, // updated for sb
        TellErrNotFound = 0x0066,
        FreeCompanyEvent = 0x012C, // added 5.0
    };
    /**
    * Client IPC Chat Type Codes.
    */
    enum ClientChatIpcTypeCN : ushort
    {
        TellReq = 0x0064,
    };
}
