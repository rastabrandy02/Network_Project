using System;
using UnityEngine;


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

public class PlayerMovementPacket : NetworkPacket
{
    public Vector2 direction;

    public PlayerMovementPacket()
    {
        type = PacketType.PlayerMovement;
    }

    public PlayerMovementPacket(Vector2 _direction)
    {
        type = PacketType.PlayerMovement;

        direction = _direction;
    }
}


    public enum SpawnType : int
{
    Turret,
    Enemy,
}

public class SpawnTowerPacket : NetworkPacket
{
    public SpawnType spawn;
    public int tower_id;
    public SpawnTowerPacket()
    {
        type = PacketType.SpawnTower;
    }

    public SpawnTowerPacket(SpawnType _spawn, int _tower_id)
    {
        type = PacketType.SpawnTower;

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

public class SpawnEnemyPacket : NetworkPacket
{    
    public SpawnEnemyPacket()
    {
        type = PacketType.SpawnEnemy;
    }
}

public class BaseDmgPacket : NetworkPacket
{
    public float damage;
    public bool isHost;

    public BaseDmgPacket()
    {
        type = PacketType.BaseDamage;
    }

    public BaseDmgPacket(float _damage, bool _isHost)
    {
        type = PacketType.BaseDamage;

        damage = _damage;
        isHost = _isHost;
    }

    public override byte[] ToByteArray()
    {
        byte[] data = base.ToByteArray();

        int offset = 8;
        
        BitConverter.GetBytes(damage).CopyTo(data, offset); //the next 4 bytes will be the damage
        offset += sizeof(float);

        BitConverter.GetBytes(isHost).CopyTo(data, offset); //the next 4 bytes will be the base identifier
        offset += sizeof(bool);


        return data;
    }

    public override void FromByteArray(byte[] data)
    {
        base.FromByteArray(data);

        int offset = 8;

        damage = BitConverter.ToSingle(data, offset); //the next 4 bytes will be the damage
        offset += sizeof(float);

        isHost = BitConverter.ToBoolean(data, offset); //the next 4 bytes will be the base identifier
        offset += sizeof(bool);


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