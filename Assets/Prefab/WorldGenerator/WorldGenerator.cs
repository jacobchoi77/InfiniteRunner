using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class WorldGenerator : MonoBehaviour{
    [Serializable]
    public struct RoadSpawnDefinition{
        public GameObject roadBlock;
        public float weight;
    }

    [Header("Road Blocks")] [SerializeField] private Transform startingPoint;
    private float roadWeightTotalWeight;
    [SerializeField] private Transform endPoint;
    [SerializeField] private RoadSpawnDefinition[] roadBlocks;
    [Header("Buildings")] [SerializeField] private GameObject[] buildings;
    [SerializeField] private Transform[] buildingSpawnPoints;
    [SerializeField] private Vector2 buildingSpawnScaleRange = new Vector2(0.6f, 0.8f);

    [Header("Street Lights")] [SerializeField] private GameObject streetLight;

    [SerializeField] private Transform[] streetLightSpawnPoints;
    private Vector3 moveDirection;
    [Header("Threats")] [SerializeField] private Threat[] threats;
    [SerializeField] private Transform[] lanes;
    [SerializeField] private Vector3 occupationDetectionHalfExtend;

    [Header("Pickups")] [SerializeField] private Pickup[] pickups;

    private void Start(){
        roadWeightTotalWeight = roadBlocks.Sum(roadBlock => roadBlock.weight);
        var nextBlockPosition = startingPoint.position;
        var endPointDistance = Vector3.Distance(startingPoint.position, endPoint.position);
        moveDirection = (endPoint.position - startingPoint.position).normalized;
        while (Vector3.Distance(startingPoint.position, nextBlockPosition) < endPointDistance){
            var newRoadBlock = SpawnRoadBlock(nextBlockPosition);
            SpawnBuildings(newRoadBlock);
            SpawnStreetLights(newRoadBlock);
            var blockLength = newRoadBlock.GetComponent<Renderer>().bounds.size.z;
            nextBlockPosition += moveDirection * blockLength;
        }

        StartSpawnElements();
    }

    private GameObject SpawnRoadBlock(Vector3 blockPosition){
        var pickedBlock = roadBlocks[0].roadBlock;
        var newBlock = Instantiate(pickedBlock);
        newBlock.transform.position = blockPosition;
        var moveComponent = newBlock.GetComponent<MovementComp>();
        moveComponent.SetMoveDirection(moveDirection);
        moveComponent.SetDestination(endPoint.position);
        return newBlock;
    }

    private void StartSpawnElements(){
        foreach (var threat in threats){
            StartCoroutine(SpawnElementCoroutine(threat));
        }

        foreach (var pickup in pickups){
            StartCoroutine(SpawnElementCoroutine(pickup));
        }
    }

    private IEnumerator SpawnElementCoroutine(Spawnable spawnable){
        while (true){
            if (GetRandomSpawnPoint(out Vector3 spawnPoint, spawnable.gameObject.tag)){
                var newSpawnable = Instantiate(spawnable, spawnPoint, Quaternion.identity);
                newSpawnable.GetMovementComponent().SetDestination(endPoint.position);
                newSpawnable.GetMovementComponent().SetMoveDirection(moveDirection);
            }

            yield return new WaitForSeconds(spawnable.SpawnInterval);
        }
    }

    private bool GetRandomSpawnPoint(out Vector3 spawnPoint, string occupationCheckTag){
        var spawnPoints = GetAvaliableSpawnPoints(occupationCheckTag);
        if (spawnPoints.Length == 0){
            spawnPoint = new Vector3(0, 0, 0);
            return false;
        }

        var pick = Random.Range(0, spawnPoints.Length);
        spawnPoint = spawnPoints[pick];
        return true;
    }


    private Vector3[] GetAvaliableSpawnPoints(string occupationCheckTag){
        List<Vector3> availableSpawnPoints = new List<Vector3>();
        foreach (var spawnTransform in lanes){
            var spawnPoint = spawnTransform.position + new Vector3(0, 0, startingPoint.position.z);
            if (!GameplayStatics.IsPositionOccupied(spawnPoint, occupationDetectionHalfExtend, occupationCheckTag)){
                availableSpawnPoints.Add(spawnPoint);
            }
        }

        return availableSpawnPoints.ToArray();
    }

    private GameObject SuppleNewBlock(Vector3 spawnPosition){
        var pickedBlock = GetRandomBlockToSpawn();
        var newBlock = Instantiate(pickedBlock);
        newBlock.transform.position = spawnPosition;
        var moveComponent = newBlock.GetComponent<MovementComp>();
        moveComponent.SetMoveDirection(moveDirection);
        moveComponent.SetDestination(endPoint.position);

        SpawnBuildings(newBlock);
        SpawnStreetLights(newBlock);
        return newBlock;
    }

    private GameObject GetRandomBlockToSpawn(){
        var pickWeight = Random.Range(0f, roadWeightTotalWeight);
        var totalWeightSoFar = 0f;
        var pick = 0;
        for (var i = 0; i < roadBlocks.Length; i++){
            totalWeightSoFar += roadBlocks[i].weight;
            if (pickWeight < totalWeightSoFar){
                pick = i;
                break;
            }
        }

        return roadBlocks[pick].roadBlock;
    }

    private void SpawnStreetLights(GameObject parentBlock){
        foreach (var streetLightSpawnPoint in streetLightSpawnPoints){
            var spawnLocation =
                parentBlock.transform.position + (streetLightSpawnPoint.position - startingPoint.position);
            var spawnRotation =
                Quaternion.LookRotation((startingPoint.position - streetLightSpawnPoint.position).normalized,
                    Vector3.up);
            var spawnRotationOffset = Quaternion.Euler(0, -90, 0);
            var newBuilding = Instantiate(streetLight, spawnLocation, spawnRotation * spawnRotationOffset,
                parentBlock.transform);
        }
    }

    private void SpawnBuildings(GameObject parentBlock){
        foreach (var buildingSpawnPoint in buildingSpawnPoints){
            var buildingSpawnLocation =
                parentBlock.transform.position + (buildingSpawnPoint.position - startingPoint.position);
            var rotationOffsetBy90 = Random.Range(0, 3);
            var buildingSpawnRotation = Quaternion.Euler(0, rotationOffsetBy90 * 90, 0);
            var buildingSpawnSize = Vector3.one * Random.Range(buildingSpawnScaleRange.x, buildingSpawnScaleRange.y);
            var buildingPick = Random.Range(0, buildings.Length);
            var newBuilding = Instantiate(buildings[buildingPick], buildingSpawnLocation, buildingSpawnRotation,
                parentBlock.transform);
            newBuilding.transform.localScale = buildingSpawnSize;
        }
    }

    private void OnTriggerExit(Collider other){
        if (other.gameObject != null && other.gameObject.CompareTag("RoadBlock")){
            var newBlock = SuppleNewBlock(other.transform.position);
            var newBlockHalfWidth = newBlock.GetComponent<Renderer>().bounds.size.z / 2f;
            var previousBlockHalfWidth = other.GetComponent<Renderer>().bounds.size.z / 2f;
            var newBlockSpawnOffset = -(newBlockHalfWidth + previousBlockHalfWidth) * moveDirection;
            newBlock.transform.position += newBlockSpawnOffset;
        }
    }
}