using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager instance; // 싱글톤 인스턴스
    public int aliveMonsters = 0;          // 현재 살아있는 몬스터 수

    void Awake()
    {
        instance = this;                   // 싱글톤 초기화
    }

    // 몬스터 등록 (생성 시 호출)
    public void RegisterMonster()
    {
        aliveMonsters++;
    }

    // 몬스터 사망 처리 (죽을 때 호출)
    public void MonsterDied()
    {
        aliveMonsters--;
    }

    // 모든 몬스터가 죽었는지 확인
    public bool AllMonstersDead()
    {
        return aliveMonsters <= 0;
    }

    // 몬스터 초기화 (예: 씬 리셋 시)
    public void ResetMonsters()
    {
        aliveMonsters = 5; // 기본 몬스터 수 설정

        // 씬 내 모든 Enemy 태그 오브젝트 제거
        foreach (var monster in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            GameObject.Destroy(monster);
        }
    }
}
