using UnityEngine;

public class MonsterStats : MonoBehaviour
{
    public int health;
    public int strength;
    public int speed;
    public int stamina;
    public int lifespan;

    public void SetStats(int hp, int str, int spd, int stam, int life)
    {
        health = hp;
        strength = str;
        speed = spd;
        stamina = stam;
        lifespan = life;
    }

    // Additional logic for monster behavior (e.g., attack, defense, etc.)
}
