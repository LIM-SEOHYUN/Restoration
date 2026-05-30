using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager instance;
    public int aliveMonsters = 0;

    void Awake()
    {
        instance = this;
    }

    public void RegisterMonster()
    {
        aliveMonsters++;
    }

    public void MonsterDied()
    {
        aliveMonsters--;
    }

    public bool AllMonstersDead()
    {
        return aliveMonsters <= 0;
    }
    public void ResetMonsters()
    {
        aliveMonsters = 5;

        foreach (var monster in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            GameObject.Destroy(monster);
        }
    }
}
