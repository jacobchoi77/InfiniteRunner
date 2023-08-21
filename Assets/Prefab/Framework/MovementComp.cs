using UnityEngine;

public class MovementComp : MonoBehaviour{
    [SerializeField] private float moveSpeed = 5f;
    private Vector3 moveDirection;
    private Vector3 destination;

    public void SetMoveDirection(Vector3 direction){
        moveDirection = direction;
    }

    void Update(){
        transform.position += moveDirection * (moveSpeed * Time.deltaTime);
        if (Vector3.Dot((destination - transform.position).normalized, moveDirection) < 0){
            Destroy(gameObject, 2f);
        }
    }

    public void SetMoveSpeed(float envMoveSpeed){
        moveSpeed = envMoveSpeed;
    }

    public void SetDestination(Vector3 newDestination){
        destination = newDestination;
    }
}