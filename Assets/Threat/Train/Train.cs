using UnityEngine;

public class Train : MonoBehaviour{
    [SerializeField] private TrainSegment segmentPrefab;
    [SerializeField] private Vector2 segmentCountRange;

    [SerializeField] private Threat threat;

    private void Start(){
        GenerateTrainBody();
    }

    private void GenerateTrainBody(){
        var bodyCount = Random.Range((int)segmentCountRange.x, (int)segmentCountRange.y);
        for (var i = 0; i < bodyCount; i++){
            var spawnPos = transform.position + transform.forward * segmentPrefab.GetSegmentLength() * i;
            var newSegment = Instantiate(segmentPrefab, spawnPos, Quaternion.identity);
            if (i == 0){
                newSegment.SetHead();
            }

            newSegment.GetMovementComponent().CopyFrom(threat.GetMovementComponent());
        }
    }
}