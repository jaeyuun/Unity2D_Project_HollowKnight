using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Skill
{
    Slash = 0,
    // SlashAlt,
    UpSlash,
    DownSlash,
    Focus,
    // Dash,
    // 팍 나가는 스킬... todo
}

public class PlayerController : MonoBehaviour
{
    // 플레이어 이동, 점프, 공격, 스킬
    private Movement2D movement2D;
    private Animator animator;
    private Raycast raycast;

    [SerializeField] private GameObject[] skillEffect;
    private Animator skillAnimator;

    private bool isJump = false;
    private bool isJumpSlash = false;
    private float jumpTimeLimit = 0.3f;
    private bool isSlash = false;
    private float holdTimer = 0; // key press timer

    private bool isBench = false;
    private PlayerInfo playerInfo;

    private void Awake()
    {
        movement2D = transform.GetComponent<Movement2D>();
        animator = transform.GetComponent<Animator>();
        raycast = transform.GetComponent<Raycast>();
        playerInfo = transform.GetComponent<PlayerInfo>();
        PlayerPrefs.SetString("Hornet", "Off");
    }

    private void Update()
    {
        PlayerMovement();
        PlayerJump();
        PlayerSkill();
    }

    private void PlayerMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");

        movement2D.MoveTo(new Vector3(x, 0, 0));

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            animator.SetBool("Run", true);
            gameObject.transform.localScale = new Vector3(-1f, 1f, 1f);
            movement2D.DefaultMovement();
        } else if (Input.GetKey(KeyCode.RightArrow))
        {
            animator.SetBool("Run", true);
            gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
            movement2D.DefaultMovement();
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Sitting_Asleep"))
            {
                animator.SetTrigger("Idle");
            }
            animator.SetBool("Run", false);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && isBench)
        {
            animator.SetTrigger("Sit");
            playerInfo.getGameData(isBench);
        }
    }

    private void PlayerJump()
    {
        if (!isJump)
        {
            movement2D.MoveTo(new Vector3(0, 1, 0));

            if (Input.GetKey(KeyCode.Space))
            {
                isJump = false;
                animator.SetBool("Jump", true);
                movement2D.JumpMovement();

                movement2D.jumpTime += Time.deltaTime;
                if (movement2D.jumpTime >= jumpTimeLimit)
                {
                    isJump = true;
                    movement2D.jumpTime = 0;

                    return;
                }
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                isJump = true;
                movement2D.jumpTime = 0;
                return;
            }
        }
        if (raycast.hitY)
        { // 점프 초기화
            isJumpSlash = false;
            isJump = false;
            animator.SetBool("Jump", false);
        }
    }

    private void PlayerSkill()
    {
        // 이펙트 켜짐
        if (isJump)
        {
            if (Input.GetKeyDown(KeyCode.Z) && !isSlash && !isJumpSlash)
            {
                if (Input.GetKey(KeyCode.DownArrow))
                {
               
                    StartCoroutine(SkillEffect_Co(Skill.DownSlash));
                    isJumpSlash = true;
                }
                else if (Input.GetKey(KeyCode.UpArrow))
                {
                    StartCoroutine(SkillEffect_Co(Skill.UpSlash));
                    isJumpSlash = true;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Z) && !isSlash)
        {
            StartCoroutine(SkillEffect_Co(Skill.Slash));
        }
        else if (Input.GetKey(KeyCode.A) && !isJump)
        { // Focus
            if (PlayerPrefs.GetInt("PlayerGauge") > 0)
            {
                holdTimer += Time.deltaTime;

                if (0.1f <= holdTimer && holdTimer <= 0.3f)
                {
                    skillAnimator = skillEffect[(int)Skill.Focus].GetComponent<Animator>(); // SkillEffect
                    skillEffect[(int)Skill.Focus].SetActive(true);
                    animator.SetBool("Focus", true);
                    isSlash = true;
                }
                if (holdTimer > 2f)
                {
                    animator.SetBool("Focus", false);
                    skillEffect[(int)Skill.Focus].SetActive(false);
                    isSlash = false;
                    holdTimer = 0;
                    playerInfo.PlayerGaugeInfo(Skill.Focus);
                    if (!PlayerPrefs.GetInt("PlayerHp").Equals(5))
                    {
                        playerInfo.PlayerHpInfo(1, false);
                    }
                }
            }
        }

        #region Key A, Focus
        if (Input.GetKeyUp(KeyCode.A))
        {
            animator.SetBool("Focus", false);
            skillEffect[(int)Skill.Focus].SetActive(false);
            isSlash = false;
            holdTimer = 0;
        }
        #endregion
    }

    private IEnumerator SkillEffect_Co(Skill skill)
    {
        string skillName = string.Empty;
        switch (skill)
        {
            case Skill.Slash:
                skillName = "Slash";
                break;
            case Skill.UpSlash:
                skillName = "UpSlash";
                break;
            case Skill.DownSlash:
                skillName = "DownSlash";
                break;
            /*case Skill.Focus:
                skillName = "Focus";
                break;*/
        }
        skillAnimator = skillEffect[(int)skill].GetComponent<Animator>(); // SkillEffect

        while (skillAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        { // 애니메이션이 재생되고 있을 때
            animator.SetBool($"{skillName}", true);
            skillEffect[(int)skill].SetActive(true);
            isSlash = true;
            yield return null;
        }
        // 애니메이션 완료 후 실행
        animator.SetBool($"{skillName}", false);
        skillEffect[(int)skill].SetActive(false);
        isSlash = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bench"))
        {
            isBench = true;
        }

        if (collision.gameObject.CompareTag("Area"))
        {
            PlayerPrefs.SetString("Hornet", "In");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bench"))
        {
            isBench = false;
        }
        if (collision.gameObject.CompareTag("Area"))
        {
            PlayerPrefs.SetString("Hornet", "Off");
        }
    }
}
