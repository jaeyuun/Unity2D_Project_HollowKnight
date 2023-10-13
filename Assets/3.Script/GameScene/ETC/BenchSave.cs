using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BenchSave : MonoBehaviour
{
    private Transform benchTransform; // 벤치 위치
    public GameObject benchPoint;

    private void Awake()
    {
        benchTransform = gameObject.transform;
    }

    public void Save()
    {
        PlayerPrefs.SetFloat("BenchPosX", benchTransform.position.x);
        PlayerPrefs.SetFloat("BenchPosY", benchTransform.position.y);
        PlayerPrefs.SetString("SceneName", $"{SceneManager.GetActiveScene().name}"); // 현재 씬 네임 저장
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            benchPoint.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            benchPoint.SetActive(false);
        }
    }
}
