using System;

public class PlayerPositionPacket : NetworkPacket
{
    public float x;
    public float y;
    public float z;

    public PlayerPositionPacket()
    {
        type = PacketType.PlayerPosition;
    }
    public PlayerPositionPacket(float _x, float _y, float _z)
    {
        type = PacketType.PlayerPosition;

        x = _x;
        y = _y;
        z = _z;
    }

    public override byte[] ToByteArray()
    {
        byte[] data = base.ToByteArray();

        int offset = 8;

        BitConverter.GetBytes(x).CopyTo(data, offset); //the next 4 bytes will be the player x
        offset += 4;

        BitConverter.GetBytes(y).CopyTo(data, offset); //the next 4 bytes will be the player y
        offset += 4;

        BitConverter.GetBytes(z).CopyTo(data, offset); //the next 4 bytes will be the player z
        offset += 4;

        return data;
    }

    public override void FromByteArray(byte[] data)
    {
        base.FromByteArray(data);

        int offset = 0;

        x = BitConverter.ToInt32(data, offset); //the next 4 bytes will be the player x
        offset += 4;

        y = BitConverter.ToInt32(data, offset); //the next 4 bytes will be the player y
        offset += 4;

        z = BitConverter.ToInt32(data, offset); //the next 4 bytes will be the player z
    }
}

//I drive - Ryan Gosling (me)