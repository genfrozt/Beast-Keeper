using UnityEngine;

public class MonsterTransfer : MonoBehaviour
{
    public static GameObject monsterPrefab;
    public static string monsterName;
    public static int health;
    public static int strength;
    public static int speed;

    private static bool hasData = false;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);  // Prevent MonsterTransfer from being destroyed on scene load
    }

    public static void SetMonsterData(GameObject prefab, string name, int hp, int str, int spd)
    {
        monsterPrefab = prefab;
        monsterName = name;
        health = hp;
        strength = str;
        speed = spd;
        hasData = true;
    }

    public static bool HasMonsterData()
    {
        return hasData;
    }
}
