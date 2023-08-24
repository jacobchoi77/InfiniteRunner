using UnityEngine;

public class AudioPlayer : MonoBehaviour{
    [SerializeField] private AudioSource audioSrc;

    public void PlayAudio(AudioClip audioToPlay, float volume, float pitch, bool destroyOnFinish = true){
        audioSrc.clip = audioToPlay;
        audioSrc.volume = volume;
        audioSrc.pitch = pitch;
        audioSrc.Play();
        if (destroyOnFinish){
            Invoke(nameof(DestroySelf), audioToPlay.length);
        }
    }

    private void DestroySelf(){
        Destroy(gameObject);
    }
}