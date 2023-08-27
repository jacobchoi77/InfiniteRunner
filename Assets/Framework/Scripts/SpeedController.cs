using System.Collections;
using UnityEngine;

public class SpeedController : MonoBehaviour{
    [SerializeField] private float globalSpeed = 15f;

    public delegate void OnGlobalSpeedChanged(float newSpeed);

    public event OnGlobalSpeedChanged onGlobalSpeedChanged;

    public float GetGlobalSpeed(){
        return globalSpeed;
    }

    public void ChangeGlobalSpeed(float speedChange, float duration){
        globalSpeed += speedChange;
        onGlobalSpeedChanged?.Invoke(globalSpeed);
        StartCoroutine(RemoveSpeedChange(speedChange, duration));
    }

    private IEnumerator RemoveSpeedChange(float speedChangedAmount, float waitTime){
        yield return new WaitForSeconds(waitTime);
        globalSpeed -= speedChangedAmount;
        onGlobalSpeedChanged?.Invoke(globalSpeed);
        InformSpeedChange();
    }

    private void InformSpeedChange(){
        onGlobalSpeedChanged?.Invoke(globalSpeed);
    }
}