using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace NetworkBasedFPS
{
    [RequireComponent(typeof(RigBuilder))]
    public class AnimationRiggingWeapon : MonoBehaviour
    {
        [SerializeField]
        List<GameObject> m_weaponList;

        RigBuilder m_rigBuilder;

        // Start is called before the first frame update
        void Awake()
        {
            m_rigBuilder = GetComponent<RigBuilder>();
        }

        private void OnEnable()
        {
            GameEntry.Event.Subscribe(SwapWeaponSuccessEventArgs.EventId, OnSwapWeaponSuccess);
        }

        private void OnDisable()
        {
            GameEntry.Event.Unsubscribe(SwapWeaponSuccessEventArgs.EventId, OnSwapWeaponSuccess);
        }

        private void OnSwapWeaponSuccess(object sender, GameEventArgs e)
        {
            SwapWeaponSuccessEventArgs ne = (SwapWeaponSuccessEventArgs)e;
            for (int i = 1; i < m_rigBuilder.layers.Count; i++)
            {
                m_rigBuilder.layers[i].active = false;
                m_weaponList[i].SetActive(false);
            }
            switch (ne.Key)
            {
                case KeyCode.Alpha1:
                    m_weaponList[1].SetActive(true);
                    m_rigBuilder.layers[1].active = true;
                    break;
                case KeyCode.Alpha2:
                    m_weaponList[2].SetActive(true);
                    m_rigBuilder.layers[2].active = true;
                    break;
            }
        }
    }
}

