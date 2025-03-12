using UnityEngine;
using UnityEngine.Audio;

using AppleGame.Sound;

namespace AppleGame.Singleton
{
    public class SoundManager
    {
        static SoundManager _instance;
        public static SoundManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SoundManager();
                }

                return _instance;
            }
        }

        const string mainAudioMixerPath = "Sound/Audio Mixer/Main";
        const string soundPlayerPath = "Sound/Prefab/Sound Player";
        
        SoundPlayer _soundPlayer;
        AudioMixer _mainAudioMixer;

        public void Init()
        {
            SoundPlayer soundPlayer = Resources.Load<SoundPlayer>(soundPlayerPath);
            if (soundPlayer == null)
            {
                Debug.LogError($"{soundPlayerPath} 에 프리펩이 없습니다");
                return;
            }
            _soundPlayer = Object.Instantiate(soundPlayer);

            AudioMixer mainAudioMixer = Resources.Load<AudioMixer>(mainAudioMixerPath);
            if (mainAudioMixer == null)
            {
                Debug.LogError($"{soundPlayerPath} 에 AudioMixer가 없습니다");
                return;
            }
            _mainAudioMixer = mainAudioMixer;

            _soundPlayer.Init(_mainAudioMixer.FindMatchingGroups("Bgm")[0], _mainAudioMixer.FindMatchingGroups("Sfx")[0]);
        }

        public void OnPlayBgm(int bgmIndex)
        {
            _soundPlayer.PlayBgm(bgmIndex);
        }

        public void OnPlaySfx(int sfxIndex)
        {
            _soundPlayer.PlaySfx(sfxIndex);
        }

        public void StopBgm()
        {
            _soundPlayer.StopBgm();
        }
    }
}
