using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    // hp, geo, gauge UI에 나타내주는 스크립트
    [SerializeField] private GameObject[] hp;
    [SerializeField] private GameObject[] gaugeSoul;
    [SerializeField] private GameObject eyeImage;
    [SerializeField] private Text geoText;

    private Animator hpAnimator;
    private Animator gaugeAnimator;

    private int playerHp;
    private int playerGauge;
    private int playerMoney;
    private bool playerDead;
    private PlayerInfo playerInfo;

    private void Awake()
    {
        playerInfo = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<PlayerInfo>();
        playerHp = playerInfo.playerHp;
        playerMoney = playerInfo.playerMoney;
        playerGauge = playerInfo.playerGauge;
        playerDead = playerInfo.playerDead;
    }

    private void Start()
    {
        SetStartUI();
    }

    private void Update()
    {
        // 플레이어에 저장된 값이 바뀌었을 때
        UIHpSet();
        UIGaugeSoulSet();
        UIGeoSet();
        UIGaugeSoulOnOff();
    }

    private void SetStartUI()
    {
        // Hp
        for (int i = playerHp; i < hp.Length; i++)
        {
            hpAnimator = hp[i].transform.GetComponent<Animator>();
            hpAnimator.Play("Hp_no");
        }
        // Geo
        geoText.text = playerMoney.ToString();
        // Gauge
        gaugeAnimator = gaugeSoul[1].transform.GetComponent<Animator>();
        if (playerGauge <= 0)
        {
            gaugeSoul[1].SetActive(false);
        }
        else
        {
            gaugeSoul[1].SetActive(true);
        }

        // eyesImage setActive
        if (playerGauge < 2)
        {
            eyeImage.SetActive(false);
        }
        else
        {
            eyeImage.SetActive(true);
        }

        // full soul
        if (playerGauge == 4)
        {
            gaugeSoul[2].SetActive(true);
        }
        else
        {
            gaugeSoul[2].SetActive(false);
        }

        switch (playerGauge)
        {
            case 1:
                gaugeAnimator.Play("GaugeSmall");
                break;
            case 2:
                gaugeAnimator.Play("GaugeMiddle");
                break;
            case 3:
            case 4:
                gaugeAnimator.Play("GaugeMiddle");
                break;
        }
    }

    private void UIHpSet()
    {
        if (playerHp != playerInfo.playerHp && playerInfo.playerHp >= 0)
        {
            bool isHurt = (playerHp - playerInfo.playerHp) >= 0; // 0보다 작으면 회복, 아니라면 다침
            string animeName = string.Empty;
            playerHp = playerInfo.playerHp;
            if (isHurt)
            {
                animeName = "Hurt";
                HpAnimation(playerHp, animeName);
            }
            else
            {
                animeName = "Recovery";
                HpAnimation(playerHp - 1, animeName);
            }
        }
    }

    private void UIGeoSet()
    {
        if (playerMoney != playerInfo.playerMoney)
        {
            int geo = playerInfo.playerMoney;
            geoText.text = geo < 1000 ? $"{geo}" : $"{geo % 1000},{geo / 1000}";
        }
    }

    private void UIGaugeSoulOnOff()
    {
        if (!playerDead)
        { // 까만영혼이 존재하지 않을 때
            GaugeAnimation(0, "SoulOn");
        }
        else
        { // 까만영혼이 존재할 때
            GaugeAnimation(0, "SoulOff");
        }
    }

    private void UIGaugeSoulSet()
    {
        if (playerGauge != playerInfo.playerGauge)
        {
            Debug.Log(playerGauge);
            Debug.Log(playerInfo.playerGauge);
            bool isUsed = (playerGauge - playerInfo.playerGauge) >= 0; // 0보다 작으면 충전, 아니라면 사용
            // 죽고나서 영혼을 못얻었을 때 gaugeSoul[0]의 애니메이션 추가해주기... todo
            playerGauge = playerInfo.playerGauge;

            if (playerGauge <= 0)
            {
                gaugeSoul[1].SetActive(false);
            }
            else
            {
                gaugeSoul[1].SetActive(true);
            }

            // eyesImage setActive
            if (playerGauge < 2)
            {
                eyeImage.SetActive(false);
            }
            else
            {
                eyeImage.SetActive(true);
            }

            // full soul
            if (playerGauge == 4)
            {
                gaugeSoul[2].SetActive(true);
            }
            else
            {
                gaugeSoul[2].SetActive(false);
            }

            string animeName = string.Empty;
            int slashAttack = PlayerPrefs.GetInt("SlashAttack"); // UIController에서 사용할 프리팹, 사용하고 나서는 0으로 바꿔주기
            if (slashAttack == 1)
            { // 슬래시로 공격했을 때
                PlayerPrefs.SetInt("SlashAttack", 0);
                if (playerGauge != 1)
                {
                    GaugeAnimation(1, "Attack");
                }
            }
            // 포커스 스킬을 사용했을 때
            if (isUsed)
            {
                animeName = "UsedSoul";
                GaugeAnimation(1, animeName);
            }
            else
            {
                animeName = "NextSoul";
                GaugeAnimation(1, animeName);
            }
        }
    }

    private void HpAnimation(int i, string animeName)
    {
        hpAnimator = hp[i].transform.GetComponent<Animator>();
        hpAnimator.SetBool($"{animeName}", true);

        if (hpAnimator.GetCurrentAnimatorStateInfo(0).IsName("Hp_No"))
        {
            hpAnimator.SetBool("Hurt", false);
        }
        else if (hpAnimator.GetCurrentAnimatorStateInfo(0).IsName("Hp"))
        {
            hpAnimator.SetBool("Recovery", false);
        }
    }

    private void GaugeAnimation(int i, string animeName)
    {
        // 게이지 사용하거나 충전할 때
        gaugeAnimator = gaugeSoul[i].transform.GetComponent<Animator>();
        gaugeAnimator.SetTrigger($"{animeName}");
    }
}
