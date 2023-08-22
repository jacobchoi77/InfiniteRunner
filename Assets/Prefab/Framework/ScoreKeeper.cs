using UnityEngine;

public class ScoreKeeper : MonoBehaviour{
    private int score;

    public delegate void OnScoreChanged(int newVal);

    public event OnScoreChanged onScoreChanged;

    public void ChangeScore(int amount){
        score += amount;
        onScoreChanged?.Invoke(score);
    }
}