namespace Sapphire.Common.Packets
{

    ////////////////////////////////////////////////////////////////////////////////
    /// Lobby Connection IPC Codes
    /**
    * Server IPC Lobby Type Codes.
    */
    enum ServerLobbyIpcType : ushort
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
    enum ClientLobbyIpcType : ushort
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
    enum ServerZoneIpcType : ushort
    {

        // static opcode ( the ones that rarely, if ever, change )
        Ping = 0x0065,
        Init = 0x0066,

        ActorFreeSpawn = 0x0097, // updated 5.1
        InitZone = 0x019B, // updated 5.1

        EffectResult = 0x0153, // updated 5.1
        ActorControl = 0x0264, // updated 5.1
        ActorControlSelf = 0x0164, // updated 5.1
        ActorControlTarget = 0x016C, // updated 5.1

        /*!
         * @brief Used when resting
         */
        UpdateHpMpTp = 0x032D, // updated 5.1

        ///////////////////////////////////////////////////

        ChatBanned = 0x006B,
        Playtime = 0x01DB, // updated 5.1
        Logout = 0x011D, // updated 5.1
        CFNotify = 0x0078,
        CFMemberStatus = 0x0079,
        CFDutyInfo = 0x007A,
        CFPlayerInNeed = 0x007F,

        SocialRequestError = 0x00AD,

        //CFRegistered = 0x00B8, // updated 4.1
        //SocialRequestResponse = 0x00BB, // updated 4.1
        //CancelAllianceForming = 0x00C6, // updated 4.2

        LogMessage = 0x00D0,

        Chat = 0x02DC, // updated 5.1

        //WorldVisitList = 0x00FE, // added 4.5

        SocialList = 0x007A, // updated 5.1

        ExamineSearchInfo = 0x03D7, // updated 5.1
        UpdateSearchInfo = 0x0358, // updated 5.1
        InitSearchInfo = 0x03A1, // updated 5.1
        //ExamineSearchComment = 0x0102, // updated 4.1

        //ServerNoticeShort = 0x0115, // updated 5.0
        //ServerNotice = 0x0116, // updated 5.0
        SetOnlineStatus = 0x015E, // updated 5.1

        CountdownInitiate = 0x01EE, // updated 5.1
        CountdownCancel = 0x03C6, // updated 5.1

        PlayerAddedToBlacklist = 0x033F, // updated 5.1
        PlayerRemovedFromBlacklist = 0x0385, // updated 5.1
        BlackList = 0x031F, // updated 5.1

        //LinkshellList = 0x012A, // updated 5.0

        //MailDeleteRequest = 0x012B, // updated 5.0

        // 12D - 137 - constant gap between 4.5x -> 5.0
        //ReqMoogleMailList = 0x0138, // updated 5.0
        //ReqMoogleMailLetter = 0x0139, // updated 5.0
        //MailLetterNotification = 0x013A, // updated 5.0

        //MarketBoardItemListingCount = 0x013B, // updated 5.0
        MarketBoardItemListing = 0x036A, // updated 5.1
        MarketBoardItemListingHistory = 0x0194, // updated 5.1
        //MarketBoardSearchResult = 0x0139, // updated 4.5

        //CharaFreeCompanyTag = 0x013B, // updated 4.5
        //FreeCompanyBoardMsg = 0x013C, // updated 4.5
        //FreeCompanyInfo = 0x013D, // updated 4.5
        //ExamineFreeCompanyInfo = 0x013E, // updated 4.5

        //FreeCompanyUpdateShortMessage = 0x0157, // added 5.0

        StatusEffectList = 0x023A, // updated 5.1
        EurekaStatusEffectList = 0x0398, // updated 5.1
        BossStatusEffectList = 0x00E6, // added 5.1
        Effect = 0x00A7, // updated 5.1
        AoeEffect8 = 0x00A9, // updated 5.1
        AoeEffect16 = 0x015F, // updated 5.1
        AoeEffect24 = 0x0292, // updated 5.1
        AoeEffect32 = 0x0268, // updated 5.1
        //PersistantEffect = 0x0165, // updated 5.0

        //GCAffiliation = 0x016F, // updated 5.0

        PlayerSpawn = 0x0386, // updated 5.1
        NpcSpawn = 0x010A, // updated 5.1
        NpcSpawn2 = 0x0115, // ( Bigger statuseffectlist? ) updated 5.1
        ActorMove = 0x01BC, // updated 5.1

        ActorSetPos = 0x0311, // updated 5.1

        ActorCast = 0x012C, // updated 5.1
        //SomeCustomiseChangePacketProbably = 0x0187, // added 5.0
        PartyList = 0x0231, // updated 5.1
        HateRank = 0x0354, // updated 5.1
        HateList = 0x00C7, // updated 5.1
        ObjectSpawn = 0x0156, // updated 5.1
        ObjectDespawn = 0x00A3, // updated 5.1
        UpdateClassInfo = 0x0258, // updated 5.1
        //SilentSetClassJob = 0x018E, // updated 5.0 - seems to be the case, not sure if it's actually used for anything
        PlayerSetup = 0x0110, // updated 5.1
        PlayerStats = 0x00CF, // updated 5.1
        ActorOwner = 0x01A5, // updated 5.1
        PlayerStateFlags = 0x019F, // updated 5.1
        PlayerClassInfo = 0x02D4, // updated 5.1

        ModelEquip = 0x025E, // updated 5.1
        Examine = 0x00EA, // updated 5.1
        //CharaNameReq = 0x0198, // updated 5.0

        // nb: see #565 on github
        //UpdateRetainerItemSalePrice = 0x019F, // updated 5.0

        SetLevelSync = 0x1186, // not updated for 4.4, not sure what it is anymore

        ItemInfo = 0x031E, // updated 5.1
        ContainerInfo = 0x0145, // updated 5.1
        InventoryTransactionFinish = 0x01AB, // updated 5.1
        InventoryTransaction = 0x023E, // updated 5.1
        CurrencyCrystalInfo = 0x0246, // updated 5.1

        InventoryActionAck = 0x0084, // updated 5.1
        UpdateInventorySlot = 0x0072, // updated 5.1
    };

    /**
    * Client IPC Zone Type Codes.
    */
    enum ClientZoneIpcType : ushort
    {

        PingHandler = 0x0065, // unchanged 5.0
        InitHandler = 0x03D2, // updated 5.1

        FinishLoadingHandler = 0x0069, // unchanged 5.0

        CFCommenceHandler = 0x006F,


        CFRegisterDuty = 0x0071,
        CFRegisterRoulette = 0x0072,
        PlayTimeHandler = 0x0073, // unchanged 5.0
        LogoutHandler = 0x0074, // unchanged 5.0
    };

    ////////////////////////////////////////////////////////////////////////////////
    /// Chat Connection IPC Codes
    /**
    * Server IPC Chat Type Codes.
    */
    enum ServerChatIpcType : ushort
    {
        Tell = 0x0064, // updated for sb
        TellErrNotFound = 0x0066,

        //FreeCompanyEvent = 0x012C, // added 5.0
    };

    /**
    * Client IPC Chat Type Codes.
    */
    enum ClientChatIpcType : ushort
    {
        TellReq = 0x0064,
    };


}