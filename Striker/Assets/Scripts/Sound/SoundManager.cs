//-----------------------------
// ImperfectlyCoded © 2017
//-----------------------------

namespace Assets.Scripts.Sound
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;

    public class SoundManager : MonoBehaviour
    {
        public AudioSource fxSource;
        public AudioSource musicSource;
        public static SoundManager instance = null;

        public float lowPitchRange = 0.95f;
        public float highPitchRange = 1.05f;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }

        public void PlaySingle(AudioClip clip)
        {
            fxSource.clip = clip;
            fxSource.Play();
        }

        public void RandomizeSfx(params AudioClip [] clips)
        {
            int randomIndex = UnityEngine.Random.Range(0, clips.Length);
            float randomPitch = UnityEngine.Random.Range(lowPitchRange, highPitchRange);

            fxSource.pitch = randomPitch;
            fxSource.clip = clips[randomIndex];
            fxSource.Play();
        }
    }
}
