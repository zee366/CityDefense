using UnityEngine;

namespace Utils {
    public class RandomSoundPlayer : MonoBehaviour {

        private static RandomSoundPlayer _singleton;
        private static AudioSource       _audioSource;


        public static void PlaySoundFx(AudioClip clip, float vol = 1f) {
            if ( clip == null ) return;

            if ( _singleton == null ) {
                GameObject go = new GameObject("RandomSoundPlayer");
                _singleton   = go.AddComponent<RandomSoundPlayer>();
                _audioSource = go.AddComponent<AudioSource>();
            }

            _audioSource.PlayOneShot(clip, vol);
        }

    }
}