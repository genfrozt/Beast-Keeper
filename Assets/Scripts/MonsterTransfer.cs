using UnityEngine;

public class MonsterTransfer : MonoBehaviour
{
    public static GameObject monsterPrefab;
    public static string monsterName;
    public static int health;
    public static int strength;
    public static int speed;
    public static int stamina;
    public static int lifespan;

    private static bool hasData = false;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);  // Prevent MonsterTransfer from being destroyed on scene load
    }

    public static void SetMonsterData(GameObject prefab, string name, int hp, int str, int spd, int stam, int life)
    {
        monsterPrefab = prefab;
        monsterName = name;
        health = hp;
        strength = str;
        speed = spd;
        stamina = stam;
        lifespan = life;

        hasData = true;
    }

    public static bool HasMonsterData()
    {
        return hasData;
    }
}
