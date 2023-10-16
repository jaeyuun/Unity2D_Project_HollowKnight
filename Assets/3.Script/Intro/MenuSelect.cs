using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;

public class MenuSelect : MonoBehaviour
{
    [SerializeField] private GameObject saveLoad;

    // 마우스와 키보드를 한 메뉴 선택
    public void SaveFileLoad()
    {
        if (!File.Exists(Application.streamingAssetsPath))
        {
            saveLoad.SetActive(true);
        }
    }
}
