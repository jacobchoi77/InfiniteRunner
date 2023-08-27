using UnityEngine;

public class AudioTrigger : MonoBehaviour{
    [SerializeField] private string triggerTag = "Player";
    [SerializeField] private AudioPlayer audioPlayerPrefab;

    [SerializeField] private AudioClip audioToPlay;
    [SerializeField] private float volume = 0.3f;
    [SerializeField] private float pitch = 0.3f;

    private void OnTriggerEnter(Collider other){
        if (other.gameObject.CompareTag(triggerTag)){
            PlayAudio();
        }
    }

    private void PlayAudio(){
        var newPlayer = Instantiate(audioPlayerPrefab);
        newPlayer.PlayAudio(audioToPlay, volume, pitch);
    }
}