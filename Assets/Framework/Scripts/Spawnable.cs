using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Spawnable : MonoBehaviour{
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private MovementComp movementComp;

    public float SpawnInterval => spawnInterval;

    public MovementComp GetMovementComponent(){
        return movementComp;
    }
}