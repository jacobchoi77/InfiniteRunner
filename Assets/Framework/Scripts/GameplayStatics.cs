using System.Linq;
using UnityEngine;

public static class GameplayStatics{
    private static GameMode gameMode;

    public static bool IsPositionOccupied(Vector3 pos, Vector3 detectionHalfExtend, string occupationCheckTag){
        var colliders = Physics.OverlapBox(pos, detectionHalfExtend, Quaternion.identity);
        return colliders.Any(col =>
            col.gameObject.CompareTag(occupationCheckTag) || col.gameObject.CompareTag("NoSpawn"));
    }

    public static GameMode GetGameMode(){
        if (gameMode == null){
            gameMode = GameObject.FindObjectOfType<GameMode>();
        }

        return gameMode;
    }
}