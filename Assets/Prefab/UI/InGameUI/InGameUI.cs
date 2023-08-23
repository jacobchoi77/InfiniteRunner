using TMPro;
using UnityEngine;

public class InGameUI : MonoBehaviour{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private UISwitcher menuSwitcher;
    [SerializeField] private Transform inGameUI;
    [SerializeField] private Transform pauseUI;
    [SerializeField] private Transform gameoverUI;

    private void Start(){
        var scoreKeeper = FindObjectOfType<ScoreKeeper>();
        if (scoreKeeper != null)
            scoreKeeper.onScoreChanged += UpdateScoreText;
        GameplayStatics.GetGameMode().onGameOver += OnGameOver;
    }

    private void OnGameOver(){
        menuSwitcher.SetActiveUI(gameoverUI);
    }

    private void UpdateScoreText(int newval){
        scoreText.SetText($"Score: {newval}");
    }

    public void SignalPause(bool isGamePaused){
        if (isGamePaused){
            menuSwitcher.SetActiveUI(pauseUI);
        }
        else{
            menuSwitcher.SetActiveUI(inGameUI);
        }
    }

    public void ResumeGame(){
        GameplayStatics.GetGameMode().SetGamePaused(false);
        menuSwitcher.SetActiveUI(inGameUI);
    }

    public void BackToMainMenu(){
        GameplayStatics.GetGameMode().BackToMainMenu();
    }

    public void RestartCurrentLevel(){
        GameplayStatics.GetGameMode().RestartCurrentLevel();
    }
}