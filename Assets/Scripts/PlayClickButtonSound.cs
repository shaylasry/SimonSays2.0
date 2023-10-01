using UnityEngine;

public class PlayClickButtonSound : MonoBehaviour
{
        [SerializeField] private AudioSource onButtonClickAudioSource;
        
        public void PlayClickSound()
        {
                Debug.Log(onButtonClickAudioSource.clip.name);
                onButtonClickAudioSource.Play();
                Debug.Log("we need to playyyy");
        }
        
}