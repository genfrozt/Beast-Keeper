using UnityEngine;

public static class MonsterTransfer
{
    public static GameObject monsterPrefab;  // Holds the summoned monster's prefab
    public static string monsterName;        // Holds the summoned monster's name
    public static int health, strength, speed;  // Holds the summoned monster's stats

    public static void SetMonsterData(GameObject prefab, string name, int h, int s, int spd)
    {
        monsterPrefab = prefab;
        monsterName = name;
        health = h;
        strength = s;
        speed = spd;
    }

    public static bool HasMonsterData()
    {
        return monsterPrefab != null;
    }
}
