using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SocialPlatforms.Impl;

public class MainMenuUI : MonoBehaviour{
    [SerializeField] private UISwitcher menuSwitcher;
    [SerializeField] private Transform mainMenu;
    [SerializeField] private Transform howToPlayMenu;
    [SerializeField] private Transform leaderboardMenu;

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
}