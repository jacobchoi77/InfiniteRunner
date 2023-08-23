using UnityEngine;

public class Pickup : Spawnable{
    [SerializeField] private int scoreEffect;
    [SerializeField] private float speedEffect;
    [SerializeField] private float speedEffectDuration;
    private bool isAdjusted = false;

    private void OnTriggerEnter(Collider other){
        if (other.gameObject.CompareTag("Player")){
            var speedController = FindObjectOfType<SpeedController>();
            if (speedController != null && speedEffect != 0){
                speedController.ChangeGlobalSpeed(speedEffect, speedEffectDuration);
            }

            var scoreKeeper = FindObjectOfType<ScoreKeeper>();
            if (scoreKeeper != null && scoreEffect != 0){
                scoreKeeper.ChangeScore(scoreEffect);
            }

            PickedUpBy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Threat")){
            var col = other.GetComponent<Collider>();
            if (col != null && !isAdjusted){
                transform.position = col.bounds.center + col.bounds.extents.y * Vector3.up;
                isAdjusted = true;
            }
        }
    }

    protected virtual void PickedUpBy(GameObject picker){
        Destroy(gameObject);
    }
}