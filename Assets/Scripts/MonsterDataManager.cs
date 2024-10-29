using System;
using UnityEngine;


public class MonsterDataManager : MonoBehaviour
{
    
    public static MonsterDataManager Instance { get; private set; }

    private int speed;
    private int strength;
    private int health;
    private int stamina;
    private int lifespan;

    private string monsterName;
    private GameObject monsterPrefab;


    public string MonsterName
    {
        get => monsterName;
        set => monsterName = value;
    }

    public GameObject MonsterPrefab
    {
        get => monsterPrefab;
        set => monsterPrefab = value;
    }

    public int Speed
    {
        get => speed;
        set => speed = Mathf.Max(0, value); // Ensure speed is not negative
    }

    public int Strength
    {
        get => strength;
        set => strength = Mathf.Max(0, value); // Ensure strength is not negative
    }

    public int Health
    {
        get => health;
        set => health = Mathf.Max(0, value); // Ensure health is not negative
    }

    public int Stamina
    {
        get => stamina;
        set => stamina = Mathf.Max(0, value); // Ensure stamina is not negative
    }

    public int Lifespan
    {
        get => lifespan;
        set => lifespan = Mathf.Max(0, value); // Ensure lifespan is not negative
    }

    // Singleton setup
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetMonsterData(GameObject prefab, string name, int hp, int str, int spd, int stam, int life)
    {
        MonsterPrefab = prefab;
        MonsterName = name;
        Health = hp;
        Strength = str;
        Speed = spd;
        Stamina = stam;
        Lifespan = life;
    }



    public bool HasMonsterData()
    {
        // You can adjust which conditions you consider as having data; this is a sample implementation
        return !string.IsNullOrEmpty(MonsterName) && MonsterPrefab != null;
       
    }
}



