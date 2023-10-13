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
    private int playerDead;

    private void Start()
    {
        playerHp = PlayerPrefs.GetInt("PlayerHp");
        playerMoney = PlayerPrefs.GetInt("PlayerMoney");
        playerGauge = PlayerPrefs.GetInt("PlayerGauge");
        playerDead = PlayerPrefs.GetInt("PlayerDead");
    }

    private void Update()
    {
        // 플레이어에 저장된 값이 바뀌었을 때
        if (playerHp != PlayerPrefs.GetInt("PlayerHp") && PlayerPrefs.GetInt("PlayerHp") >= 0)
        {
            UIHpSet();
        }
        if (playerGauge != PlayerPrefs.GetInt("PlayerGauge"))
        {
            UIGaugeSoulSet();
        }
        if (playerMoney != PlayerPrefs.GetInt("PlayerMoney"))
        {
            UIGeoSet();
        }
        if (playerDead != PlayerPrefs.GetInt("PlayerDead"))
        {
            UIGaugeSoulOnOff();
        }
    }

    private void UIHpSet()
    {
        bool isHurt = ((playerHp - PlayerPrefs.GetInt("PlayerHp")) >= 0) ? true : false; // 0보다 작으면 회복, 아니라면 다침
        string animeName = string.Empty;
        playerHp = PlayerPrefs.GetInt("PlayerHp");
        if (isHurt)
        {
            animeName = "Hurt";
            HpAnimation(playerHp, animeName);
        } else
        {
            animeName = "Recovery";
            HpAnimation(playerHp - 1, animeName);
        }
    }

    private void UIGeoSet()
    {
        int geo = PlayerPrefs.GetInt("PlayerMoney");
        geoText.text = (geo < 1000) ? $"{geo}" : $"{geo % 1000},{geo / 1000}";
    }

    private void UIGaugeSoulOnOff()
    {
        if (PlayerPrefs.GetInt("PlayerDead").Equals(0))
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
        int gauge = PlayerPrefs.GetInt("PlayerGauge");
        bool isUsed = ((playerGauge - PlayerPrefs.GetInt("PlayerGauge")) >= 0) ? true : false; // 0보다 작으면 충전, 아니라면 사용
        // 죽고나서 영혼을 못얻었을 때 gaugeSoul[0]의 애니메이션 추가해주기... todo
        // soul gauge
        playerGauge = PlayerPrefs.GetInt("PlayerGauge");

        if (playerGauge <= 0)
        {
            gaugeSoul[1].SetActive(false);
        } else
        {
            gaugeSoul[1].SetActive(true);
        }

        // eyesImage setActive
        if (gauge < 2)
        {
            eyeImage.SetActive(false);
        } else
        {
            eyeImage.SetActive(true);
        }

        // full soul
        if (gauge == 4)
        {
            gaugeSoul[2].SetActive(true);
        } else
        {
            gaugeSoul[2].SetActive(false);
        }

        string animeName = string.Empty;
        int slashAttack = PlayerPrefs.GetInt("SlashAttack"); // UIController에서 사용할 프리팹, 사용하고 나서는 0으로 바꿔주기
        if (slashAttack == 1)
        { // 슬래시로 공격했을 때
            PlayerPrefs.SetInt("SlashAttack", 0);
             if (gauge != 1)
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

    // 비동기 로딩씬 추가... todo, Load와 Save때 활용
}
