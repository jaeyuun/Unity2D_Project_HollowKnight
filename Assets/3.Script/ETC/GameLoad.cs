using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class GameLoad : MonoBehaviour
{
    [SerializeField] private GameSave gameSave;
    public GameData gameData;

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
