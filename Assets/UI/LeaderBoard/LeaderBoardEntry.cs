using TMPro;
using UnityEngine;

public class LeaderBoardEntry : MonoBehaviour{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dateText;
    [SerializeField] private TextMeshProUGUI scoreText;

    public void Init(string name, string date, int score){
        nameText.SetText(name);
        dateText.SetText(date);
        scoreText.SetText(score.ToString());
    }
}