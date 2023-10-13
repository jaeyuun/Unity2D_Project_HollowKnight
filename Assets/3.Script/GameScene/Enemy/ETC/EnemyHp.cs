using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHp : MonoBehaviour
{
    public int enemyHp;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Slash"))
        {
            switch (collision.gameObject.name)
            { // 다른 스킬로 때렸을 때 더 많이 깎이는 걸로 추가... todo
                case "Slash":
                case "SlashAlt":
                case "UpSlash":
                case "DownSlash":
                    enemyHp -= PlayerPrefs.GetInt("PlayerPower");
                    PlayerPrefs.SetInt("SlashAttack", 1); // UIController에서 사용할 프리팹
                    PlayerInfo.PlayerGaugeInfo(Skill.Slash);
                    break;
            }
        }
    }
}
