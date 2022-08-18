//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.Localization;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace NetworkBasedFPS
{
    public class SettingForm : UGuiForm
    {
        //  背景音乐音量滑动条
        [SerializeField]
        private Slider m_MusicVolumeSlider = null;

        //  音效音量滑动条
        [SerializeField]
        private Slider m_SoundVolumeSlider = null;

        //  鼠标灵敏度滑动条
        [SerializeField]
        private Slider m_MouseSensitivitySlider = null;

        //  返回按钮
        [SerializeField]
        private Button m_ReturnButton = null;

        public void OnMusicVolumeChanged(float volume)
        {
            GameEntry.Sound.SetVolume("Music", volume);
        }

        public void OnSoundVolumeChanged(float volume)
        {
            GameEntry.Sound.SetVolume("Sound", volume);
        }

        public void OnMouseSensitivityChanged(float volume)
        {
            GameEntry.Setting.SetFloat("MouseSensitivity", volume);
        }

        public void OnReturnButtonClick()
        {
            Close();
        }

        public void OnSubmitButtonClick()
        {
            //if (m_SelectedLanguage == GameEntry.Localization.Language)
            //{
            //    Close();
            //    return;
            //}

            //GameEntry.Setting.SetString(Constant.Setting.Language, m_SelectedLanguage.ToString());
            //GameEntry.Setting.Save();

            //GameEntry.Sound.StopMusic();
            //UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Restart);
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_ReturnButton.onClick.AddListener(OnReturnButtonClick);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_MusicVolumeSlider.value = GameEntry.Sound.GetVolume("Music");

            m_SoundVolumeSlider.value = GameEntry.Sound.GetVolume("Sound");

            m_MouseSensitivitySlider.value = GameEntry.Setting.GetFloat("MouseSensitivity");

        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }
    }
}
