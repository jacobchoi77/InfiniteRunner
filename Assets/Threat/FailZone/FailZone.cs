using UnityEngine;

public class FailZone : MonoBehaviour{
    private void OnTriggerEnter(Collider other){
        if (other.gameObject.CompareTag("Player")){
            GameplayStatics.GetGameMode().GameOver();
        }
    }
}