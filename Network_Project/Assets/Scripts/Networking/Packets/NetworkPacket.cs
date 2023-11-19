using System;
public enum PacketType : int
{
    PlayerPosition
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
        offset += 4;

        BitConverter.GetBytes(size).CopyTo(data, offset); //the next 4 bytes will be the packet size
        offset += 4;

        return data;
    }

    public virtual void FromByteArray(byte[] data)
    {
        int offset = 0;
        type = (PacketType)BitConverter.ToInt32(data, offset); //the first 4 bytes will be the packet type
        offset += 4;

        size = BitConverter.ToInt32(data, offset); //the next 4 bytes will be the packet size
        offset += 4;
    }
}
