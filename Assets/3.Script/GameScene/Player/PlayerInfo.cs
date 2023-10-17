using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInfo : MonoBehaviour
{
    private GameLoad gameLoad;
    private GameData gameData;

    public int playerHp;
    public int playerGauge;
    public int playerMoney;
    public int playerPower;
    public bool playerDead;
    
    public float playerX;
    public float playerY;
    public string sceneName;

    public Vector2 playerTransform;

    public int[] playerSkill; // 스킬 얻었는지 아닌지 확인하는 변수, 0이면 없는 것 1이면 있는 것
    private int playerSkillCount = 1; // 스킬의 총 개수

    private void Awake()
    {
        gameLoad = GameObject.FindGameObjectWithTag("SceneLoader").transform.GetComponent<GameLoad>();
        gameData = gameLoad.Load("save");

        playerX = gameData.playerX;
        playerY = gameData.playerY;
        sceneName = gameData.sceneName;

        playerHp = gameData.playerHp;
        playerGauge = gameData.playerGauge;
        playerMoney = gameData.playerMoney;
        playerPower = gameData.playerPower;
        playerDead = gameData.playerDead;

        for (int i = 0; i < playerSkillCount; i++)
        {
            playerSkill[i] = new int();
        }

        playerTransform = new Vector2(playerX, playerY);
        transform.position = playerTransform;
    }

    public GameData GetGameData(bool isBench = true)
    {
        gameData.playerHp = playerHp;
        gameData.playerGauge = playerGauge;
        gameData.playerMoney = playerMoney;
        gameData.playerPower = playerPower;
        gameData.playerDead = playerDead;

        if (isBench)
        {
            gameData.playerX = playerX;
            gameData.playerY = playerY;
        }
        gameData.sceneName = sceneName;

        return gameData;
    }

    public void PlayerHpInfo(int skillPower = 1, bool isMinus = true)
    {
        if (isMinus)
        {
            playerHp -= skillPower;
        } else
        {
            playerHp += skillPower;
        }
    }

    public void PlayerGaugeInfo(Skill skillName)
    { // Enemy를 hit했을 때만
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
        if (playerDead)
        { // 까만 영혼이 존재하지 않을 때
            maxGauge = 4;
        }
        else { // 까만 영혼이 존재할 때
            maxGauge = 3;
        }

        if (playerGauge > maxGauge)
        {
            playerGauge = maxGauge;
        }
        else if (playerGauge < 0)
        {
            playerGauge = 0;
        }
    }

    public void PlayerMoneyInfo(int geo, bool isMinus = true)
    {
        if (isMinus)
        {
            playerMoney -= geo;
        } else
        {
            playerMoney += geo;
        }
    }
}
