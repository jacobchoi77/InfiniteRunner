using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveDataManager{
    [Serializable]
    class PlayerProfilesData{
        public List<string> playerNames;

        public PlayerProfilesData(List<string> names){
            playerNames = names;
        }
    }

    private static string GetSaveDir(){
        return Application.persistentDataPath;
    }

    private static string GetPlayerProfileFileName(){
        return "players.json";
    }

    private static string GetPlayerProfileSaveDir(){
        return GetSaveDir() + "/" + GetPlayerProfileFileName();
    }

    public static void SavePlayerProfile(string playerName){
        GetSavedPlayerProfile(out List<string> players);
        if (players.Contains(playerName)) return;
        players.Insert(0, playerName);
        var data = new PlayerProfilesData(players);
        var dataJSON = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetPlayerProfileSaveDir(), dataJSON);
    }

    public static bool GetSavedPlayerProfile(out List<string> data){
        if (File.Exists(GetPlayerProfileSaveDir())){
            var dataJSON = File.ReadAllText(GetPlayerProfileSaveDir());
            var loadedData = JsonUtility.FromJson<PlayerProfilesData>(dataJSON);
            data = loadedData.playerNames;
            return true;
        }

        data = new List<string>();
        return false;
    }
}