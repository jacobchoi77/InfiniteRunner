using TMPro;
using UnityEngine;

public class InGameUI : MonoBehaviour{
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Start(){
        var scoreKeeper = FindObjectOfType<ScoreKeeper>();
        if (scoreKeeper != null)
            scoreKeeper.onScoreChanged += UpdateScoreText;
    }

    private void UpdateScoreText(int newval){
        scoreText.SetText($"Score: {newval}");
    }
}