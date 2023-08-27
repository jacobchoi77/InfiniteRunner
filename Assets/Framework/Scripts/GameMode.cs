using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMode : MonoBehaviour{
    private bool isGamePaused = false;

    public delegate void OnGameOver();

    [SerializeField] private int mainMenuSceneBuildIndex = 0;
    [SerializeField] private int firstSceneIndex = 1;

    public event OnGameOver onGameOver;

    private bool isGameOver = false;

    public void GameOver(){
        SetGamePaused(true);
        UpdateLeadboard();
        isGameOver = true;
        onGameOver?.Invoke();
    }

    private void UpdateLeadboard(){
        var score = GetComponent<ScoreKeeper>().Score;
        SaveDataManager.SaveNewLeaderBoardEntry(SaveDataManager.GetActivePlayerName(), DateTime.Now, score);
    }

    public void SetGamePaused(bool isPaused){
        if (isGameOver) return;
        isGamePaused = isPaused;
        Time.timeScale = isPaused ? 0 : 1;
    }

    public void TogglePause(){
        if (IsGamePaused()){
            SetGamePaused(false);
        }
        else{
            SetGamePaused(true);
        }
    }

    public bool IsGamePaused(){
        return isGamePaused;
    }

    public void BackToMainMenu(){
        SetGamePaused(false);
        LoadScene(mainMenuSceneBuildIndex);
    }

    private void LoadScene(int sceneBuildIndex){
        isGameOver = false;
        SetGamePaused(false);
        SceneManager.LoadScene(sceneBuildIndex);
    }

    public void RestartCurrentLevel(){
        LoadScene(SceneManager.GetActiveScene().buildIndex);
        SetGamePaused(false);
    }

    public bool IsGameOver(){
        return isGameOver;
    }

    public void LoadFirstLevel(){
        LoadScene(firstSceneIndex);
    }

    public void QuitGame(){
        Application.Quit();
    }
}