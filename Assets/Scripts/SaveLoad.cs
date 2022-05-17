using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class SaveLoad : MonoBehaviour
{

    [SerializeField] Critter critter = null;

    gameSettings settings = null;
    
    private class SaveData
    {
        public int score = 0;
    }

    private SaveData GetSaveData()
    {
        SaveData saveData = new SaveData();
        if (critter == null) { return saveData; }

        saveData.score = critter.GetFoodEaten();

        return saveData;
    }

    private void Awake()
    {
        SaveSystem.Init();
    }

    private void Start()
    {
        settings = GameObject.FindGameObjectWithTag("GameSettings").GetComponent<gameSettings>();
    }

    public void OnSave()
    {
        SaveGameState();
    }

    #region Getters
    public int GetCurrentGridHighScore()
    {
        SaveData loadedData = GetLoadGameData();

        return loadedData.score;
    }

    private SaveData GetLoadGameData()
    {
        SaveData loadedData = new SaveData();

        string saveFileName = "/";
        saveFileName = FormatSaveFileName(saveFileName);

        string loadedString = SaveSystem.Load(saveFileName);

        if (loadedString != null)
            loadedData = JsonUtility.FromJson<SaveData>(loadedString);
        else
            Debug.Log($"No save file");

        return loadedData;
    }
    #endregion

    private void SaveGameState()
    {
        SaveData saveData = new SaveData();
        saveData = GetSaveData();

        SaveData loadedData = GetLoadGameData();

        //save data score is lower than previous map high score
        if(saveData.score <= loadedData.score)
        {
            if (loadedData.score != 0) { return; }
        }

        string json = JsonUtility.ToJson(saveData);
        string saveFileName = "/";

        saveFileName = FormatSaveFileName(saveFileName);

        SaveSystem.Save(json, saveFileName);
    }

    private string FormatSaveFileName(string saveFileName)
    {
        if(settings == null) { return null; }

        if(settings.GetInCritterGameMode())
            saveFileName += "cr";
        else
            saveFileName += "cl";

        saveFileName += settings.GetNumberOfRows().ToString();
        saveFileName += "x";
        saveFileName += settings.GetNumberOfCols().ToString();

        return saveFileName;
    }


}
