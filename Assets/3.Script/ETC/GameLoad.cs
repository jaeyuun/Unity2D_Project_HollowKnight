using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class GameLoad : MonoBehaviour
{
    private GameSave gameSave;
    public GameData gameData;

    private void Awake()
    {
        gameSave = transform.GetComponent<GameSave>();
    }

    public GameData Load(string fileName)
    {
        gameData = new GameData();
        if (!File.Exists(Application.streamingAssetsPath))
        {
            if (!fileName.Contains(".json"))
            {
                fileName += ".json";
            }

            fileName = Path.Combine(Application.streamingAssetsPath, fileName);
            string readData = File.ReadAllText(fileName);
            gameData = JsonConvert.DeserializeObject<GameData>(readData);
        } else
        {
            gameSave.Save(false);
        }

        return gameData;
    }
}
