using UnityEngine;

public class MonsterStats : MonoBehaviour
{
    public int health;
    public int strength;
    public int speed;

    public void SetStats(int hp, int str, int spd)
    {
        health = hp;
        strength = str;
        speed = spd;
    }

    // Additional logic for monster behavior (e.g., attack, defense, etc.)
}
