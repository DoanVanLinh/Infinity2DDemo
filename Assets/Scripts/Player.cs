using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int level;
    [SerializeField] private float health;
    [SerializeField] private Vector3 position;

    public int Level { get => level; set => level = value; }
    public float Health { get => health; set => health = value; }
    public Vector3 Position { get => position; set => position = value; }

    public void SaveData()
    {
        SaveSystem.SaveData(this);
    }
    public void LoadData()
    {
        PlayerData data = SaveSystem.LoadData();
        Debug.Log("Level "+data.level);
        Debug.Log("hea;th " + data.health);
        Debug.Log("postition (" + data.position[0]+"," + data.position[1] + "," + data.position[2] + ")");

    }
}
