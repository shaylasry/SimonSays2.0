using System;
using UnityEngine;

public class PlayClickButtonSound : MonoBehaviour
{
        [SerializeField] private AudioClip onButtonClickClip;
        
        public void PlayClickSound()
        {
                if (Camera.main == null) throw new NullReferenceException("Main camera not found. Sound cannot be played.");
                
                //use camera to find the listener position so the sound won't effect by distance
                Vector3 listenerPosition = Camera.main.transform.position;
                AudioSource.PlayClipAtPoint(onButtonClickClip, listenerPosition);
        }
}