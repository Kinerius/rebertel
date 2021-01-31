using UnityEngine;

namespace Sound
{
    public class SoundManager : MonoBehaviour
    {
        public AudioSource[] EffectsSource;
        public AudioSource MusicSource;

        public float LowPitchRange = .95f;
        public float HighPitchRange = 1.05f;

        private int lastAudioSourceIndex = 0;
    
        public static SoundManager Instance = null;
        
        public AudioClip Disparo;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad (gameObject);
        }

        public void Play(AudioClip clip)
        {
            var source = EffectsSource[lastAudioSourceIndex];
            float randomPitch = Random.Range(LowPitchRange, HighPitchRange);
            source.Stop();
            source.clip = clip;
            source.Play();
            lastAudioSourceIndex = (lastAudioSourceIndex + 1) % EffectsSource.Length;
        }

        public void PlayMusic(AudioClip clip)
        {
            MusicSource.clip = clip;
            MusicSource.Play();
        }

	
    }
}