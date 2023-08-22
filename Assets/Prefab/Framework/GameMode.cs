using UnityEngine;

public class GameMode : MonoBehaviour{
    public void GameOver(){
        SetGamePaused(true);
    }

    public void SetGamePaused(bool isPaused){
        Time.timeScale = isPaused ? 0 : 1;
    }
}