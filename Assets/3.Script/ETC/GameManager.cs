using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    private GameObject player;
    private Animator animator;

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else
        {
            Destroy(this.gameObject);
        }

        player = GameObject.FindGameObjectWithTag("Player");
        animator = player.GetComponent<Animator>();
    }

    private void Start()
    {
        GameLoad();
    }

    private void GameLoad()
    {
        /*SceneManager.LoadScene(PlayerPrefs.GetString("SceneName"));*/
        /*animator.SetTrigger("Resurrection");*/
    }
}

