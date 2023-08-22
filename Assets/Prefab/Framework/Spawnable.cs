using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnable : MonoBehaviour
{
    [SerializeField] private float _spawnInterval = 2f;
    [SerializeField] private MovementComp movementComp;

    public float SpawnInterval{
        get{ return _spawnInterval; }
    }

    public MovementComp GetMovementComponent(){
        return movementComp;
    }
}
