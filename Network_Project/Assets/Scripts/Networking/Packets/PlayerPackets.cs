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
        offset += sizeof(float);

        BitConverter.GetBytes(y).CopyTo(data, offset); //the next 4 bytes will be the player y
        offset += sizeof(float);

        BitConverter.GetBytes(z).CopyTo(data, offset); //the next 4 bytes will be the player z
        offset += sizeof(float);

        return data;
    }

    public override void FromByteArray(byte[] data)
    {
        base.FromByteArray(data);

        int offset = 8;

        x = BitConverter.ToSingle(data, offset); //the next 4 bytes will be the player x
        offset += sizeof (float);

        y = BitConverter.ToSingle(data, offset); //the next 4 bytes will be the player y
        offset += sizeof(float);

        z = BitConverter.ToSingle(data, offset); //the next 4 bytes will be the player z
    }
}

public enum SpawnType : int
{
    Turret,
    Enemy,
}

public class SpawnPacket : NetworkPacket
{
    public SpawnType spawn;
    public int tower_id;
    public SpawnPacket()
    {
        type = PacketType.Spawn;
    }

    public SpawnPacket(SpawnType _spawn, int _tower_id)
    {
        type = PacketType.Spawn;

        spawn = _spawn;
        tower_id = _tower_id;
    }

    public override byte[] ToByteArray()
    {
        byte[] data = base.ToByteArray();

        int offset = 8;

        BitConverter.GetBytes((int)spawn).CopyTo(data, offset); //the next 4 bytes will be the spawn type
        offset += sizeof(int);

        BitConverter.GetBytes(tower_id).CopyTo(data, offset); //the next 4 bytes will be the spawn x
        offset += sizeof(int);

        

        return data;
    }

    public override void FromByteArray(byte[] data)
    {
        base.FromByteArray(data);

        int offset = 8;

        spawn = (SpawnType)BitConverter.ToInt32(data, offset); //the next 4 bytes will be the player x
        offset += sizeof(int);

        tower_id = BitConverter.ToInt32(data, offset); //the next 4 bytes will be the player x
        offset += sizeof(int);

       
    }

}

public class StartGamePacket : NetworkPacket
{
    public StartGamePacket()
    {
        type = PacketType.StartGame;
    }   
}

 public class HelloPacket : NetworkPacket
    {
        public HelloPacket()
        {
            type = PacketType.Hello;
        }
    }

//I drive - Ryan Gosling (me)