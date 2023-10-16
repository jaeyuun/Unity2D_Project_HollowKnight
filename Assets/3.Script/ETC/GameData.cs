using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    // 플레이어 정보
    public int playerHp = 5;
    public int playerGauge = 0;
    public int playerMoney = 0;
    public int playerPower = 5;
    public bool playerDead = false;

    // 위치 정보
    public string sceneName = "GameScene";
    public float playerX = -2f;
    public float playerY = 2f;
}
