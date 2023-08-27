using UnityEngine;

public class TrainSegment : MonoBehaviour{
    [SerializeField] private Mesh headMesh;
    [SerializeField] private Mesh[] segmentMeshes;

    [SerializeField] private MeshFilter trainMesh;

    [SerializeField] private BoxCollider trainCollider;
    [SerializeField] private MovementComp movementComp;

    private bool isHead = false;

    private void Start(){
        RandomTrainMesh();
    }

    private void RandomTrainMesh(){
        if (isHead) return;
        var pick = Random.Range(0, segmentMeshes.Length);
        trainMesh.mesh = segmentMeshes[pick];
    }

    public float GetSegmentLength(){
        return trainCollider.size.z;
    }

    public void SetHead(){
        trainMesh.mesh = headMesh;
        isHead = true;
    }

    public MovementComp GetMovementComponent(){
        return movementComp;
    }
}