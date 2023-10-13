using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    [Header("테스트 시 체크")]
    public bool isTest; // 삭제할 것... todo

    public int playerHp;
    public int playerGauge;
    public int playerMoney;
    public int playerPower;
    public int playerDead;

    public int[] playerSkill; // 스킬 얻었는지 아닌지 확인하는 변수, 0이면 없는 것 1이면 있는 것
    private int playerSkillCount = 1; // 스킬의 총 개수

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("PlayerHp"))
        {
            PlayerPrefs.SetInt("PlayerHp", 5);
        }
        if (!PlayerPrefs.HasKey("PlayerGauge"))
        {
            PlayerPrefs.SetInt("PlayerGauge", 0);
        }
        if (!PlayerPrefs.HasKey("PlayerMoney"))
        {
            PlayerPrefs.SetInt("PlayerMoney", 0);
        }
        if (!PlayerPrefs.HasKey("PlayerPower"))
        {
            PlayerPrefs.SetInt("PlayerPower", 5);
        }
        if (!PlayerPrefs.HasKey("PlayerDead"))
        {
            PlayerPrefs.SetInt("PlayerDead", 0); // false 0, true 1
        }

        // 삭제할 것... todo
        PlayerPrefs.SetInt("PlayerHp", 5);
        PlayerPrefs.SetInt("PlayerGauge", 0);
        PlayerPrefs.SetInt("PlayerMoney", 0);
        PlayerPrefs.SetInt("PlayerPower", 5);

        playerHp = PlayerPrefs.GetInt("PlayerHp");
        playerGauge = PlayerPrefs.GetInt("PlayerGauge");
        playerMoney = PlayerPrefs.GetInt("PlayerMoney");
        playerPower = PlayerPrefs.GetInt("PlayerPower");
        playerDead = PlayerPrefs.GetInt("PlayerDead");

        for (int i = 0; i < playerSkillCount; i++)
        {
            playerSkill[i] = new int();
        }
    }

    public static void PlayerHpInfo(int skillPower = 1, bool isMinus = true)
    {
        int playerHp = PlayerPrefs.GetInt("PlayerHp");
        if (isMinus)
        {
            playerHp -= skillPower;
        } else
        {
            playerHp += skillPower;
        }
        PlayerPrefs.SetInt("PlayerHp", playerHp);
    }

    public static void PlayerGaugeInfo(Skill skillName)
    { // Enemy를 hit했을 때만
        int playerGauge = PlayerPrefs.GetInt("PlayerGauge");
        switch (skillName)
        {
            case Skill.Slash:
            /*case Skill.SlashAlt:*/
            case Skill.UpSlash:
            case Skill.DownSlash:
                playerGauge++;
                break;
            case Skill.Focus: // 나중에 스킬에 따라 얼마나 게이지 감소하는지 추가... todo
                playerGauge--;
                break;
        }

        int maxGauge;
        if (PlayerPrefs.GetInt("PlayerDead").Equals(0))
        { // 까만 영혼이 존재하지 않을 때
            maxGauge = 4;
        }
        else { // 까만 영혼이 존재할 때
            maxGauge = 3;
        }
        if (playerGauge > maxGauge)
        {
            PlayerPrefs.SetInt("PlayerGauge", maxGauge);
        }
        else if (playerGauge < 0)
        {
            PlayerPrefs.SetInt("PlayerGauge", 0);
        }
        else
        {
            PlayerPrefs.SetInt("PlayerGauge", playerGauge);
        }
    }

    public static void PlayerMoneyInfo(int geo, bool isMinus = true)
    {
        int playerMoney = PlayerPrefs.GetInt("PlayerMoney");
        if (isMinus)
        {
            playerMoney -= geo;
        } else
        {
            playerMoney += geo;
        }
        PlayerPrefs.SetInt("PlayerMoney", playerMoney);
    }
}
