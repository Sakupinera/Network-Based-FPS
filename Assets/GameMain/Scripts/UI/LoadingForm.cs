using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NetworkBasedFPS
{
    public class LoadingForm : UGuiForm
    {
        [SerializeField]
        private Slider m_LoadingSlider;

        public float BarValue { get; set; }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            m_LoadingSlider.value = BarValue;
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            BarValue = 0;
            base.OnClose(isShutdown, userData);
        }
    }

}
