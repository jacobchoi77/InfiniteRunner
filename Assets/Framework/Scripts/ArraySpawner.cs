using UnityEngine;

public class ArraySpawner : MonoBehaviour{
    [SerializeField] private int amount = 10;
    [SerializeField] private float gap = 1f;

    private void Start(){
        for (var i = 1; i <= amount; i++){
            var spawnPosition = new Vector3(transform.position.x, 0f, transform.position.z) +
                                transform.forward * gap * i;
            var nextCoin = Instantiate(gameObject, spawnPosition, Quaternion.identity);
            nextCoin.GetComponent<ArraySpawner>().enabled = false;
        }
    }
}