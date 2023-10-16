using System.IO; // 입출력
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json; // 가져오는 것

public class GameSave : MonoBehaviour
{
    private PlayerInfo playerInfo;

    private void Awake()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            playerInfo = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<PlayerInfo>();
        }
    }

    public void Save(bool exFile = true)
    {
        GameData data;
        if (exFile)
        {
            data = playerInfo.getGameData();
        } else
        {
            data = new GameData();
        }
        string fileName = "save";

        if (!fileName.Contains(".json"))
        { // 파일 이름에 .json 문구가 포함되지 않았다면
            fileName += ".json";
        }
        fileName = Path.Combine(Application.streamingAssetsPath, fileName); // 경로 지정
        string toJson = JsonConvert.SerializeObject(data, Formatting.Indented); // json은 dictionary와 유사
        File.WriteAllText(fileName, toJson);
    }
}
