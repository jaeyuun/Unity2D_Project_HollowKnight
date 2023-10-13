using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HornetController : MonoBehaviour
{
    private Movement2D movement2D;
    private Animator animator;
    private Raycast raycast;
    private EnemyHp enemyHp;
    private Rigidbody2D rigidbody2D;

    [SerializeField] private GameObject[] skillEffect;
    [SerializeField] GameObject needle;
    private GameObject player;

    private int random;
    private bool isTurn;
    private bool isPlayerLeft;
    private bool isJump = false;
    private bool isStun = false;
    private bool isFirst = true;
    private bool isDead; // 죽었을 때 한번만 코루틴 실행
    Vector3 vDir;

    private int attackCount = 1;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemyHp = transform.GetComponent<EnemyHp>();
        movement2D = transform.GetComponent<Movement2D>();
        animator = transform.GetComponent<Animator>();
        raycast = transform.GetComponent<Raycast>();
        rigidbody2D = transform.GetComponent<Rigidbody2D>();

        PlayerPrefs.SetString("Hornet", "Off");

        random = Random.Range(0, 4);
    }

    private void Update()
    {
        if (PlayerPrefs.GetString("Hornet").Equals("In"))
        {
            if (isFirst)
            {
                StartCoroutine(HornetStart_Co());
            } else
            {
                if (enemyHp.enemyHp <= 0)
                { // 죽었을 때 한 번
                    StartCoroutine(StunAnimation_Co());

                    if (!isDead)
                    {
                        StartCoroutine(DeadAnimation_Co());
                    }
                }
                else if (enemyHp.enemyHp > 0)
                {
                    if (enemyHp.enemyHp == 50 || enemyHp.enemyHp == 20)
                    {
                        if (!isStun)
                        {
                            isStun = true;
                            StartCoroutine(StunAnimation_Co());
                        }
                    }
                    HornetJump();
                }
            }
        }
    }

    private IEnumerator HornetStart_Co()
    { // 플레이어가 입장했을 때
        yield return new WaitForSeconds(3f);
        animator.SetTrigger("Hornet");
        yield return null;
        animator.SetTrigger("Run");
        isFirst = false;
    }
    
    private void HornetMovement()
    {
        if (!raycast.hitPlayerX)
        { // Player와 닿은 RaycastHit이 전부 null이 되었을 때, 이외
            switch (2)
            {
                case 0: // Dash_Land
                    HornetDash_Land();
                    break;
                case 2: // Throw
                    Throw();
                    break;
            }
        } else
        {
            switch (2)
            {
                case 4: // Sphere_Land
                    Sphere_Land();
                    break;
                case 5: // Sphere_Air
                    // 점프 후 스킬 사용
                    Sphere_Air();
                    break;
            }
        }

        StartCoroutine(HornetLocalScale_Co());

        if (raycast.hitX)
        { // 벽이랑 가까울 때
            isTurn = true;
        }
    }

    #region Skill
    private void HornetRun(string name)
    {
        animator.SetBool($"{name} 0", false);
        movement2D.MoveTo(new Vector3(transform.localScale.x * -1, 0, 0));
        movement2D.DefaultMovement();
    }

    private void HornetJump()
    {
        animator.SetBool("Jump 0", true);
        movement2D.MoveTo(new Vector3(0, 1, 0));
        movement2D.AccelMovement();
        animator.SetTrigger("Jump");
        rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation; // 잠깐 위치 고정, 이후에 z만 고정으로 바꿈
        animator.SetBool("Jump 0", false);
    }

    private void HornetDash_Land()
    {
        animator.SetBool("Dash_Land 0", true);
        movement2D.MoveTo(new Vector3(0, 0, 0));
        movement2D.DefaultMovement();
        animator.SetTrigger("Dash_Land");
        movement2D.MoveTo(new Vector3(transform.localScale.x * -1, 0, 0));
        movement2D.AccelMovement();
        animator.SetTrigger("Dash_Land");
        movement2D.MoveTo(new Vector3(0, 0, 0));
        movement2D.DefaultMovement();
    }

/*    private void HornetDash_Air()
    {
        // jump 먼저 시작
        animator.SetTrigger("Jump");
        Vector3 playerPos = player.transform.position;
        Vector3 hornetPos = transform.position;
        transform.position = Vector3.Lerp(hornetPos, playerPos, animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        vDir = (hornetPos - playerPos).normalized;

        animator.SetTrigger("Jump");
        animator.SetTrigger("Dash_Air");

        transform.position = Vector3.Lerp(playerPos, playerPos, animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        movement2D.MoveTo(vDir);
        movement2D.DefaultMovement();
        animator.SetTrigger("Dash_Air");

        animator.SetTrigger("Dash_Air");
    }*/

    private void Sphere_Land() {
        animator.SetTrigger("Sphere");
        movement2D.MoveTo(new Vector3(0, 0, 0));
        movement2D.DefaultMovement();
        animator.SetTrigger("Sphere");
        skillEffect[1].SetActive(true);
        animator.SetTrigger("Sphere");
        skillEffect[1].SetActive(false);
    }

    private void Sphere_Air()
    {
        HornetJump();
        animator.SetTrigger("Sphere");
        movement2D.MoveTo(new Vector3(0, 0, 0));
        movement2D.DefaultMovement();
        animator.SetTrigger("Sphere");
        skillEffect[1].SetActive(true);
        animator.SetTrigger("Sphere");
        skillEffect[1].SetActive(false);
        rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void Throw()
    {
        Vector3 playerPos = player.transform.position;
        animator.SetTrigger("Throw");
        movement2D.MoveTo(new Vector3(0, 0, 0));
        movement2D.DefaultMovement();
        animator.SetTrigger("Throw");
        skillEffect[0].SetActive(true);
        needle.transform.position = Vector3.Lerp(transform.position, playerPos, animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        animator.SetTrigger("Throw");
        needle.transform.position = Vector3.Lerp(playerPos, transform.position, animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        skillEffect[0].SetActive(false);
    }
    #endregion

    private IEnumerator HornetLocalScale_Co()
    {
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        { // 애니메이션 동작 시
            yield return new WaitForSeconds(0.2f);
        }

        if (transform.position.x - player.transform.position.x < 0)
        { // 호넷이 왼, 플레이어 오른쪽 ... 호넷 localScale.x를 -1f로 변경
            isPlayerLeft = false;
        }
        else
        { // 플레이어 오, 호넷 왼 ... 호넷 localScale.x를 1f로 변경
            isPlayerLeft = true;
        }

        // 애니메이션 동작 후
        if (isTurn)
        { // 좌우 반전 해주기
            isTurn = false;

            if (transform.localScale.x == 1)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else if (transform.localScale.x == -1)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }

        if (isPlayerLeft)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        movement2D.MoveTo(new Vector3(0, 0, 0));
        movement2D.DefaultMovement();

        yield return new WaitForSeconds(2f);
    }

    private IEnumerator DeadAnimation_Co()
    {
        isDead = true;
        animator.SetBool("Dead", true);
        movement2D.MoveTo(new Vector3(0, 0, 0));

        yield return new WaitForSeconds(2.0f);

        Destroy(gameObject);
        SceneManager.LoadScene("Intro"); // 게임 클리어, 다른 오브젝트에서 진행해야할 듯
    }

    private IEnumerator StunAnimation_Co()
    {
        if (enemyHp.enemyHp <= 0)
        {
            animator.SetTrigger("Stun_End");
        } else
        {
            animator.SetBool("Stun", true);
            /*if ()
            {
                // 플레이어가 
            }*/
            yield return new WaitForSeconds(2.0f);
            animator.SetBool("Stun", false);
        }

        yield return new WaitForSeconds(2f);

        isStun = false;

        if (enemyHp.enemyHp > 0)
        {
            animator.SetTrigger("Run");
        }
    }
}
