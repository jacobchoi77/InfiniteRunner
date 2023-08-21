using UnityEngine;
using UnityEngine.Serialization;

public class Forward : MonoBehaviour{
    [SerializeField] private int forwardSpeed = 2;

    void Update(){
        transform.position += new Vector3(0f, 0f, 1f) * (Time.deltaTime * forwardSpeed);
    }
}