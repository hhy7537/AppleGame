using UnityEngine;
using UnityEngine.Audio;

using AppleGame.Util;

namespace AppleGame.Sound
{
    public class SoundPlayer : BehaviourBase
    {
        [SerializeField] AudioSource[] _audioSources;
        [SerializeField] AudioClip[] _bgmClip;
        [SerializeField] AudioClip[] _sfxClip;

#if UNITY_EDITOR
        protected override void OnBindSerialzedField()
        {
            _audioSources = GetComponents<AudioSource>();
            if (_audioSources.Length <= 0)
            {
                Debug.LogError("Sound Player에 AudioSource를 추가해 주세요.");
                return;
            }

            AudioClip[] bgmClip = Resources.LoadAll<AudioClip>("Sound/AudioClips/Bgm");
            if (bgmClip.Length <= 0)
            {
                Debug.LogError("Sound/Bgm 에 AudioClip이 없습니다.");
                return;
            }
            _bgmClip = bgmClip;

            AudioClip[] sfxClip = Resources.LoadAll<AudioClip>("Sound/AudioClips/Sfx");
            if (sfxClip.Length <= 0)
            {
                Debug.LogError("Sound/Sfx 에 AudioClip이 없습니다.");
                return;
            }
            _sfxClip = sfxClip;
        }
#endif

        [SerializeField] AudioMixerGroup _bgmGroup;
        [SerializeField] AudioMixerGroup _sfxGroup;

        public void Init(AudioMixerGroup bgmGroup, AudioMixerGroup sfxGroup)
        {
            _bgmGroup = bgmGroup;
            _sfxGroup = sfxGroup;

            if (_audioSources.Length <= 1)
            {
                Debug.LogError("AudioSource를 더 추가해 주세요.");
                return;
            }
            _audioSources[0].playOnAwake = true;
            _audioSources[0].loop = true;
            for (int i = 1; i < _audioSources.Length; i++)
            {
                _audioSources[i].playOnAwake = false;
                _audioSources[i].loop = false;
            }
        }

        public void PlayBgm(int bgmIndex)
        {
            if (_bgmClip == null)
            {
                Debug.LogError("Audio Clip을 배열에 추가 해주세요.");
                return;
            }

            _audioSources[0].clip = _bgmClip[bgmIndex];
            _audioSources[0].Play();
        }

        public void StopBgm()
        {
            _audioSources[0].Stop();
        }

        public void PlaySfx(int sfxIndex)
        {
            if (_sfxClip == null)
            {
                Debug.LogError("Audio Clip을 배열에 추가 해주세요.");
                return;
            }

            for (int i = 1; i < _audioSources.Length; i++)
            {
                if(_audioSources[i].isPlaying == false)
                {
                    _audioSources[i].clip = _sfxClip[sfxIndex];
                    _audioSources[i].Play();
                    return;
                }
            }

            Debug.Log("모든 AudioSource가 사용중입니다.");
        }
    }
}
