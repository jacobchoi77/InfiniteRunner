using UnityEngine;

public class Spinner : MonoBehaviour{
    [SerializeField] private float spinSpeed = 50;

    void Update(){
        transform.Rotate(new Vector3(0, 0, 1), spinSpeed * Time.deltaTime);
    }
}