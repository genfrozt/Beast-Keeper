using UnityEngine;

public class MonsterTransfer : MonoBehaviour
{
    public static MonsterTransfer instance;

    public GameObject monsterPrefab;  // The summoned monster's prefab
    public string monsterName;        // The summoned monster's name
    public int health, strength, speed;  // The summoned monster's stats

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // Don't destroy this object between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetMonsterData(GameObject prefab, string name, int health, int strength, int speed)
    {
        monsterPrefab = prefab;
        monsterName = name;
        this.health = health;
        this.strength = strength;
        this.speed = speed;
    }
}
