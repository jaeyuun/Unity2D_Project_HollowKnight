using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDead : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = transform.GetComponent<Animator>();
    }

    private void Update()
    {
        PlayerDeath();
    }

    private void PlayerDeath()
    {
        if (PlayerPrefs.GetInt("PlayerHp").Equals(0))
        {
            PlayerPrefs.SetInt("PlayerDead", 1); // 플레이어의 까만 영혼이 존재함
            StartCoroutine(DeadAnimation_Co());
        }
    }

    private IEnumerator DeadAnimation_Co()
    {
        animator.SetTrigger("Dead");
        // 파티클 동시에 생성
        // 플레이어 위치에 까만 영혼 생성... todo
        yield return new WaitForSeconds(2f);
        PlayerInfo.PlayerMoneyInfo(PlayerPrefs.GetInt("PlayerMoney"), true); // 플레이어가 가진 만큼 돈 없어짐
        PlayerInfo.PlayerHpInfo(5, false);
        // 비동기 로딩씬 코루틴 추가.. todo
        PlayerPrefs.SetInt("PlayerHp", 5);
        // 플레이어가 저장한 마지막 위치로 이동
        SceneManager.LoadScene(PlayerPrefs.GetString("SceneName"));
        /*transform.position = new Vector2(PlayerPrefs.GetFloat("BenchPosX"), PlayerPrefs.GetFloat("BenchPosY"));
        animator.SetTrigger("Resurrection");*/
    }
}
