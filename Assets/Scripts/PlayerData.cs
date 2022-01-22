using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerData
{
    public int level;
    public float health;
    public float[] position;
    
    public PlayerData(Player player)
    {
        this.level = player.Level;
        this.health = player.Health;

        this.position = new float[3];
        this.position[0] = player.Position.x;
        this.position[1] = player.Position.y;
        this.position[2] = player.Position.z;

    }

}
