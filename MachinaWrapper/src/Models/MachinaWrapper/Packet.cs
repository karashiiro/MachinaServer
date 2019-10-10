namespace MachinaWrapper.Common
{
    public struct IpcPacket
    {
        public Packet Metadata;
        public string Type;
        public ushort ServerId;
        public uint Timestamp;
        public string ActorControlCategory;
        public string ClientTriggerCategory;

        public IpcPacket(Packet metadata) : this()
        {
            Metadata = metadata;
        }
    }

    public struct Packet
    {
        public string ConnectionRoute;
        public string ConnectionType;
        public long Epoch;
        public byte[] Data;
        public uint PacketSize;
        public ushort SegmentType;

        public Packet(string connectionRoute, string connectionType, long epoch, byte[] data) : this()
        {
            ConnectionRoute = connectionRoute;
            ConnectionType = connectionType;
            Epoch = epoch;
            Data = data;
        }
    }
}
