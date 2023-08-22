using System;
using UnityEngine;

public class MovementComp : MonoBehaviour{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Vector3 moveDirection;
    [SerializeField] private Vector3 destination;

    private void Start(){
        var speedController = FindObjectOfType<SpeedController>();
        if (speedController != null){
            speedController.onGlobalSpeedChanged += SetMoveSpeed;
            SetMoveSpeed(speedController.GetGlobalSpeed());
        }
    }

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

    public void CopyFrom(MovementComp other){
        SetMoveSpeed(other.moveSpeed);
        SetMoveDirection(other.moveDirection);
        SetDestination(other.destination);
    }
}