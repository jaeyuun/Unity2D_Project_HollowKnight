using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDead : MonoBehaviour
{
    private SceneLoader sceneLoader;
    private Animator animator;
    private PlayerInfo playerInfo;

    private void Awake()
    {
        animator = transform.GetComponent<Animator>();
        playerInfo = transform.GetComponent<PlayerInfo>();
        sceneLoader = GameObject.FindGameObjectWithTag("SceneLoader").transform.GetComponent<SceneLoader>();
    }

    private void Update()
    {
        PlayerDeath();
    }

    private void PlayerDeath()
    {
        if (playerInfo.playerHp == 0)
        {
            playerInfo.playerDead = true; // 플레이어의 까만 영혼이 존재함
            StartCoroutine(DeadAnimation_Co());
            playerInfo.getGameData(false);
        }
    }

    private IEnumerator DeadAnimation_Co()
    {

        animator.SetTrigger("Dead");
        PlayerPrefs.SetInt("DeadTime", 1);
        // 파티클 동시에 생성
        // 플레이어 위치에 까만 영혼 생성... todo
        yield return new WaitForSeconds(1f);
        sceneLoader.DeadLoadingTrue();
        playerInfo.PlayerMoneyInfo(playerInfo.playerMoney, true); // 플레이어가 가진 만큼 돈 없어짐
        yield return new WaitForSeconds(2f);
        // 비동기 로딩씬 코루틴 추가.. todo
        if (PlayerPrefs.GetInt("DeadTime").Equals(1))
        {
            playerInfo.PlayerHpInfo(5, false);
            PlayerPrefs.SetInt("DeadTime", 0);
        }
        sceneLoader.DeadLodingFalse();
        // 플레이어가 저장한 마지막 위치로 이동
    }
}
