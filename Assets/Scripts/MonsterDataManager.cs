using UnityEngine;

public class MonsterDataManager : MonoBehaviour
{
    public static MonsterDataManager Instance { get; private set; }

    public GameObject MonsterPrefab { get; private set; }
    public string MonsterName { get; private set; }
    public int Health { get; private set; }
    public int Strength { get; private set; }
    public int Speed { get; private set; }
    public int Stamina { get; private set; }
    public int Lifespan { get; private set; }

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
        return MonsterPrefab != null;
    }
}