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
        ActorCast = 0x0187, // updated 5.0
        ActorControl = 0x00DE, // updated 5.1
        ActorControlSelf = 0x0143,
        ActorControlTarget = 0x0144,
        ActorFreeSpawn = 0x0191,
        ActorMove = 0x0182, // updated 5.0
        ActorOwner = 0x0192, // updated 5.0
        ActorSetPos = 0x0184, // updated 5.0
        AoeEffect16 = 0x0162, // updated 5.0
        AoeEffect24 = 0x0163, // updated 5.0
        AoeEffect32 = 0x0164, // updated 5.0
        AoeEffect8 = 0x0161, // updated 5.0
        BlackList = 0x0123, // updated 5.0
        CFDutyInfo = 0x007A,
        CFMemberStatus = 0x0079,
        CFNotifyPop = 0x0265, // updated 5.1
        CFNotify = 0x0078,
        CFPlayerInNeed = 0x007F,
        CFPreferredRole = 0x026E, // updated 5.1
        CharaNameReq = 0x0198, // updated 5.0
        Chat = 0x0104, // updated 5.0
        ChatBanned = 0x006B,
        ContainerInfo = 0x01A2, // updated 5.0
        CountdownCancel = 0x011F, // updated 5.0
        CountdownInitiate = 0x011E, // updated 5.0
        CurrencyCrystalInfo = 0x0148, // updated 5.1
        DailyQuestRepeatFlags = 0x0260, // updated 5.0
        DailyQuests = 0x025E, // updated 5.0
        DirectorPlayScene = 0x01B9, // updated 5.0
        DirectorPopUp = 0x0200, // updated 5.0 - display dialogue pop-ups in duties and FATEs, for example, Teraflare's countdown
        DirectorVars = 0x01F5, // updated 5.0
        Discovery = 0x0212, // updated 5.0
        Effect = 0x015E, // updated 5.0
        EffectResult = 0x0141,
        EorzeaTimeOffset = 0x0214, // updated 5.0
        EquipDisplayFlags = 0x0220, // updated 5.0
        EurekaStatusEffectList = 0x015C, // updated 5.0
        EventFinish = 0x007A, // updated 5.1
        EventLinkshell = 0x1169,
        EventOpenGilShop = 0x01BC, // updated 5.0
        EventPlay = 0x0384, // updated 5.1
        EventStart = 0x0134, // updated 5.1
        EventPlay4 = 0x021D, // updated 5.1
        Examine = 0x039F, // updated 5.1
        ExamineSearchInfo = 0x0218, // updated 5.1
        FreeCompanyUpdateShortMessage = 0x0157, // added 5.0
        GCAffiliation = 0x016F, // updated 5.0
        HateList = 0x018A, // updated 5.0
        HateRank = 0x0189, // updated 5.0
        HousingEstateGreeting = 0x023B, // updated 5.0
        HousingIndoorInitialize = 0x0237, // updated 5.0
        HousingInternalObjectSpawn = 0x241, // updated 5.0
        HousingLandFlags = 0x023D, // updated 5.0
        HousingObjectInitialize = 0x0240, // updated 5.0
        HousingObjectMove = 0x0244, // updated 5.0
        HousingShowEstateGuestAccess = 0x023E, // updated 5.0
        HousingUpdateLandFlagsSlot = 0x023C, // updated 5.0
        HousingWardInfo = 0x0243, // updated 5.0
        HuntingLogEntry = 0x01B3, // updated 5.0
        Init = 0x0066,
        InitSearchInfo = 0x0111, // updated 5.0
        InitZone = 0x015B, // updated 5.1
        InventoryActionAck = 0x01A7, // updated 5.0
        InventoryTransaction = 0x01A4, // updated 5.0
        InventoryTransactionFinish = 0x01A3, // updated 5.0
        ItemInfo = 0x030B, // updated 5.1
        LandInfoSign = 0x0239, // updated 5.0
        LandPriceUpdate = 0x0238, // updated 5.0
        LandRename = 0x023A, // updated 5.0
        LandSetInitialize = 0x0234, // updated 5.0
        LandUpdate = 0x0235, // updated 5.0
        LinkshellList = 0x012A, // updated 5.0
        LogMessage = 0x00D0,
        Logout = 0x0077, // updated 5.0
        MSQTrackerComplete = 0x01D6, // updated 5.0
        MailDeleteRequest = 0x012B, // updated 5.0
        MailLetterNotification = 0x013A, // updated 5.0
        MarketBoardItemListing = 0x0348, // updated 5.1
        MarketBoardItemListingCount = 0x02EF, // updated 5.1
        MarketBoardItemListingHistory = 0x00A9, // updated 5.1
        MarketBoardSearchResult = 0x0122, // updated 5.1
        MarketTaxRates = 0x006E, // updated 5.1
        ModelEquip = 0x0196, // updated 5.0
        Mount = 0x01F3, // updated 5.0
        NpcSpawn2 = 0x0181, // ( Bigger statuseffectlist? ) updated 5.0
        NpcSpawn = 0x007E, // updated 5.1
        ObjectDespawn = 0x018C, // updated 5.0
        ObjectSpawn = 0x018B, // updated 5.0
        PartyList = 0x0188, // updated 5.0
        PersistantEffect = 0x0165, // updated 5.0
        Ping = 0x0065,
        PlayerAddedToBlacklist = 0x0120, // updated 5.0
        PlayerClassInfo = 0x0194, // updated 5.0
        PlayerRemovedFromBlacklist = 0x0121, // updated 5.0
        PlayerSetup = 0x014F, // updated 5.1
        PlayerSpawn = 0x0147, // updated 5.1
        PlayerStateFlags = 0x0193, // updated 5.0
        PlayerStats = 0x0258, // updated 5.1
        PlayerTitleList = 0x0211, // updated 5.0
        Playtime = 0x02AE, // updated 5.1
        PrepareZoning = 0x02A4, // updated 5.0
        QuestActiveList = 0x01D2, // updated 5.0
        QuestCompleteList = 0x01D4, // updated 5.0
        QuestFinish = 0x01D5, // updated 5.0
        QuestMessage = 0x01DE, // updated 5.0
        QuestTracker = 0x01E3, // updated 5.0
        QuestUpdate = 0x01D3, // updated 5.0
        ReqMoogleMailLetter = 0x0139, // updated 5.0
        ReqMoogleMailList = 0x0138, // updated 5.0
        ServerNotice = 0x0116, // updated 5.0
        ServerNoticeShort = 0x0115, // updated 5.0
        SetOnlineStatus = 0x0117, // updated 5.0
        SilentSetClassJob = 0x018E, // updated 5.0 - seems to be the case, not sure if it's actually used for anything
        SocialList = 0x010D, // updated 5.0
        SocialRequestError = 0x00AD,
        SomeDirectorUnk4 = 0x0248, // updated 5.1
        StatusEffectList = 0x015B, // updated 5.0
        UpdateClassInfo = 0x02B6, // updated 5.1
        UpdateHpMpTp = 0x03D4, // updated 5.1
        UpdateInventorySlot = 0x01AD, // updated 5.1
        UpdateRetainerItemSalePrice = 0x019F, // updated 5.0
        UpdateSearchInfo = 0x039D, // updated 5.1
        UseMooch = 0x0293, // updated 5.1
        WeatherChange = 0x0210, // updated 5.0
        YardObjectSpawn = 0x0236, // updated 5.0
    };
    /**
    * Client IPC Zone Type Codes.
    */
    enum ClientZoneIpcTypeCN : ushort
    {
        AoESkillHandler = 0x140, // updated 5.0
        BlackListHandler = 0x00F2, // updated 5.0
        BuildPresetHandler = 0x014F, // updated 5.0
        CFCommenceHandler = 0x006F,
        CFRegisterDuty = 0x0071,
        CFRegisterRoulette = 0x0072,
        CancelLogout = 0x0075, // updated 5.0
        ChatHandler = 0x019F, // updated 5.1
        ClientTrigger = 0x0315, // updated 5.1
        DiscoveryHandler = 0x013B, // updated 5.0
        EmoteEventHandler = 0x0152, // updated 5.0
        EnterTeriEventHandler = 0x0155, // updated 5.0
        FinishLoadingHandler = 0x0069, // unchanged 5.0
        FreeCompanyUpdateShortMessageHandler = 0x0123, // added 5.0
        GMCommand1 = 0x013E, // updated 5.0
        GMCommand2 = 0x013F, // updated 5.0
        HousingUpdateHouseGreeting = 0x0178, // updated 5.0
        HousingUpdateObjectPosition = 0x0179, // updated 5.0
        InitHandler = 0x0066, // unchanged 5.0
        InventoryModifyHandler = 0x0148, // updated 5.0
        LandRenameHandler = 0x0177, // updated 5.0
        LinkshellListHandler = 0x00FA, // updated 5.0
        LogoutHandler = 0x0074, // unchanged 5.0
        OutOfRangeEventHandler = 0x0154, // updated 5.0
        PingHandler = 0x0065, // unchanged 5.0
        PlayTimeHandler = 0x0073, // unchanged 5.0
        PlayerSearchHandler = 0x00F4, // updated 5.0
        ReqEquipDisplayFlagsChange = 0x0175, // updated 5.0
        ReqExamineSearchCommentHandler = 0x00E7, // updated 5.0
        ReqPlaceHousingItem = 0x014B, // updated 5.0
        ReqRemovePlayerFromBlacklist = 0x00F1, // updated 5.0
        ReqSearchInfoHandler = 0x00E6, // updated 5.0
        ReturnEventHandler = 0x015A, // updated 5.0?
        SetSearchInfoHandler = 0x032A, // updated 5.1
        SetSharedEstateSettings = 0x017B, // updated 5.0
        ShopEventHandler = 0x0156, // updated 5.0
        SkillHandler = 0x013D, // updated 5.0
        SocialListHandler = 0x00E1, // updated 5.0
        TalkEventHandler = 0x0151, // updated 5.0
        TradeReturnEventHandler = 0x015B, // updated 5.0?
        UpdatePositionHandler = 0x0141, // updated 5.0
        UpdatePositionInstance = 0x0180, // updated 5.0
        WithinRangeEventHandler = 0x0153, // updated 5.0
        ZoneLineHandler = 0x0139, // updated 5.0
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
