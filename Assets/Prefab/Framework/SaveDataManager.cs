using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveDataManager{
    [Serializable]
    public class PlayerProfilesData{
        public List<string> playerNames;

        public PlayerProfilesData(List<string> names){
            playerNames = names;
        }
    }

    [Serializable]
    public class LeaderBoardEntryData{
        public string name;
        public string date;
        public int score;

        public LeaderBoardEntryData(string name, DateTime date, int score){
            this.name = name;
            this.date = date.ToString();
            this.score = score;
        }
    }

    [Serializable]
    class LeaderboardListData{
        public List<LeaderBoardEntryData> entries;

        public LeaderboardListData(List<LeaderBoardEntryData> entries){
            this.entries = entries;
        }
    }

    public static string GetSaveDir(){
        return Application.persistentDataPath;
    }

    private static string GetPlayerProfileFileName(){
        return "players.pl";
    }

    private static string GetPlayerProfileSaveDir(){
        return GetSaveDir() + "/" + GetPlayerProfileFileName();
    }

    public static void SavePlayerProfile(string playerName){
        GetSavedPlayerProfiles(out List<string> players);
        if (players.Contains(playerName)) return;
        players.Insert(0, playerName);
        SavePlayerProfilesFromList(players);
    }

    private static void SavePlayerProfilesFromList(List<string> players){
        var data = new PlayerProfilesData(players);
        var dataJSON = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetPlayerProfileSaveDir(), dataJSON);
    }

    public static bool GetSavedPlayerProfiles(out List<string> data){
        if (File.Exists(GetPlayerProfileSaveDir())){
            var dataJSON = File.ReadAllText(GetPlayerProfileSaveDir());
            var loadedData = JsonUtility.FromJson<PlayerProfilesData>(dataJSON);
            data = loadedData.playerNames;
            return true;
        }

        data = new List<string>();
        return false;
    }

    public static void DeletePlayerProfile(string playerName){
        GetSavedPlayerProfiles(out List<string> players);
        players.Remove(playerName);
        SavePlayerProfilesFromList(players);
    }

    public static void SaveNewLeaderBoardEntry(string name, DateTime date, int score){
        var newEntry = new LeaderBoardEntryData(name, date, score);
        GetSavedLeaderBoardEntryList(out List<LeaderBoardEntryData> entries);
        if (entries.Count == 0){
            entries.Add(newEntry);
        }
        else{
            for (var i = 0; i < entries.Count; i++){
                if (newEntry.score > entries[i].score){
                    entries.Insert(i, newEntry);
                    break;
                }
            }
        }

        var data = new LeaderboardListData(entries);
        var dataJSON = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetLeaderBoardSaveDir(), dataJSON);
    }

    public static bool GetSavedLeaderBoardEntryList(out List<LeaderBoardEntryData> entries){
        if (File.Exists(GetLeaderBoardSaveDir())){
            var loadedDataJSON = File.ReadAllText(GetLeaderBoardSaveDir());
            var loadedData = JsonUtility.FromJson<LeaderboardListData>(loadedDataJSON);
            entries = loadedData.entries;
            return true;
        }

        entries = new List<LeaderBoardEntryData>();
        return false;
    }

    private static string GetLeaderBoardSaveDir(){
        return GetSaveDir() + "/" + GetLeaderBoardSaveFileName();
    }

    private static string GetLeaderBoardSaveFileName(){
        return "LeaderBoard.lb";
    }

    public static void SetActivePlayer(string playerName){
        GetSavedPlayerProfiles(out List<string> players);
        if (players.Remove(playerName)){
            players.Insert(0, playerName);
            SavePlayerProfilesFromList(players);
        }
    }

    public static string GetActivePlayerName(){
        GetSavedPlayerProfiles(out List<string> players);
        if (players.Count != 0){
            return players[0];
        }

        return "Default Player";
    }
}