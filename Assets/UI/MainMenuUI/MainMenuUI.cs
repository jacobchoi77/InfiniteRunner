using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MainMenuUI : MonoBehaviour{
    [SerializeField] private UISwitcher menuSwitcher;
    [SerializeField] private Transform mainMenu;
    [SerializeField] private Transform howToPlayMenu;
    [SerializeField] private Transform leaderboardMenu;
    [SerializeField] private Transform createPlayerProfileMenu;
    [SerializeField] private TMP_InputField newPlayerNameField;
    [SerializeField] private TMP_Dropdown playerList;

    private void Start(){
        UpdatePlayerList();
        Debug.Log(SaveDataManager.GetSaveDir());
        playerList.onValueChanged.AddListener(UpdateSaveActivePlayer);
    }

    private void UpdateSaveActivePlayer(int index){
        var currentActivePlayer = playerList.options[index].text;
        SaveDataManager.SetActivePlayer(currentActivePlayer);
    }

    private void UpdatePlayerList(){
        SaveDataManager.GetSavedPlayerProfiles(out List<string> players);
        playerList.ClearOptions();
        playerList.AddOptions(players);
    }

    public void StartGame(){
        GameplayStatics.GetGameMode().LoadFirstLevel();
    }

    public void BackToMainMenu(){
        menuSwitcher.SetActiveUI(mainMenu);
    }

    public void GoToHowToPlayMenu(){
        menuSwitcher.SetActiveUI(howToPlayMenu);
    }

    public void GoToLeaderboardMenu(){
        menuSwitcher.SetActiveUI(leaderboardMenu);
    }

    public void QuitGame(){
        GameplayStatics.GetGameMode().QuitGame();
    }

    public void SwitchToPlayerProfileMenu(){
        menuSwitcher.SetActiveUI(createPlayerProfileMenu);
    }

    public void AddPlayerProfile(){
        var newPlayerName = newPlayerNameField.text;
        SaveDataManager.SavePlayerProfile(newPlayerName);
        UpdatePlayerList();
        BackToMainMenu();
    }

    public void DeleteSelectedPlayerProfile(){
        if (playerList.options.Count != 0){
            var playerName = playerList.options[playerList.value].text;
            SaveDataManager.DeletePlayerProfile(playerName);
            UpdatePlayerList();
        }
    }
}