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
        ActorCast = 0x020A, // updated 5.1
        ActorControl = 0x018C, // updated 5.1 (ActorControl142?)
        ActorControlSelf = 0x0097, // updated 5.1 (ActorControl143?)
        ActorControlTarget = 0x0383, // updated 5.1 (ActorControl144?)
        ActorGauge = 0x0377, // updated 5.1
        ActorSetPos = 0x028B, // updated 5.1
        AddStatusEffect = 0x0328, // updated 5.1
        AoeEffect16 = 0x039D, // updated 5.1 (actioneffect16)
        AoeEffect24 = 0x00E4, // updated 5.1 (actioneffect24)
        AoeEffect32 = 0x0327, // updated 5.1 (actioneffect32)
        AoeEffect8 = 0x011B, // updated 5.1 (actioneffect8)
        BossStatusEffectList = 0x037F, // updated 5.1
        CFNotify = 0x025E, // updated 5.1
        CFPreferredRole = 0x00E6, // updated 5.1
        CurrencyCrystalInfo = 0x02FF, // updated 5.1
        Effect = 0x00D6, // updated 5.1 (actioneffect1)
        EventFinish = 0x02B8, // updated 5.1
        EventPlay = 0x03D8, // updated 5.1
        EventPlay4 = 0x00DB, // updated 5.1
        EventStart = 0x032D, // updated 5.1
        Examine = 0x0213, // updated 5.1
        ExamineSearchInfo = 0x0130, // updated 5.1
        InitZone = 0x031E, // updated 5.1
        InventoryTransaction = 0x0162, // updated 5.1
        //InventoryTransactionFinish = ?
        ItemInfo = 0x017C, // updated 5.1
        MarketBoardItemListing = 0x009C, // updated 5.1
        MarketBoardItemListingCount = 0x026E, // updated 5.1
        MarketBoardItemListingHistory = 0x027A, // updated 5.1
        MarketBoardSearchResult = 0x0081, // updated 5.1
        MarketTaxRates = 0x03C6, // updated 5.1
        NpcSpawn = 0x01FC, // updated 5.1
        PlayerSetup = 0x0382, // updated 5.1
        PlayerSpawn = 0x010A, // updated 5.1
        PlayerStats = 0x03E7, // updated 5.1
        Playtime = 0x0205, // updated 5.1
        RetainerInformation = 0x3DC, // updated 5.1
        SomeDirectorUnk4 = 0x00FB, // updated 5.1
        StatusEffectList = 0x01CE, // updated 5.1
        UpdateClassInfo = 0x03D0, // updated 5.1
        UpdateHpMpTp = 0x0296, // updated 5.1
        UpdateInventorySlot = 0x0204, // updated 5.1
        UpdateSearchInfo = 0x029C, // updated 5.1
        UseMooch = 0x02E8, // updated 5.1
    };
    /**
    * Client IPC Zone Type Codes.
    */
    enum ClientZoneIpcTypeKR : ushort
    {
        ChatHandler = 0x024E, // updated 5.1
        ClientTrigger = 0x031C, // updated 5.1
        InventoryModifyHandler = 0x0273, // updated 5.1
        SetSearchInfoHandler = 0x0242, // updated 5.1
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
