using System;
public enum PacketType : int
{
    StartGame,
    PlayerPosition,
    Hello,
}


public class NetworkPacket
{
    public const int MAX_SIZE = 256;
    public int size;
    public PacketType type;

    public virtual byte[] ToByteArray()
    {
        byte[] data = new byte[MAX_SIZE];
        int offset = 0;

        BitConverter.GetBytes((int)type).CopyTo(data, offset); //the first 4 bytes will be the packet type
        offset += sizeof(int);

        BitConverter.GetBytes(size).CopyTo(data, offset); //the next 4 bytes will be the packet size
        offset += sizeof(int);

        return data;
    }

    public virtual void FromByteArray(byte[] data)
    {
        int offset = 0;
        type = (PacketType)BitConverter.ToInt32(data, offset); //the first 4 bytes will be the packet type
        offset += sizeof(int);

        size = BitConverter.ToInt32(data, offset); //the next 4 bytes will be the packet size
        offset += sizeof(int);
    }

    public static NetworkPacket ParsePacket(byte[] data)
    {
        NetworkPacket packet;
        PacketType type = (PacketType)BitConverter.ToInt32(data, 0); //the first 4 bytes will be the packet type

        switch (type)
        {
            case PacketType.StartGame:
                {
                    packet = new StartGamePacket();
                    return packet;
                }
            case PacketType.PlayerPosition:
                {
                    packet = new PlayerPositionPacket();
                    packet.FromByteArray(data);
                    return packet;
                }
        }
        return null;
    }
}

