using GameFramework.Event;
using GamePlayer;
using GameServer;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace NetworkBasedFPS
{
    public class WeaponItems : UGuiForm
    {
        //主武器的UI
        [SerializeField]
        private TMP_Text pCurrentBullet;
        [SerializeField]
        private TMP_Text pRestBullet;

        //副武器的UI
        //主武器的UI
        [SerializeField]
        private TMP_Text sCurrentBullet;
        [SerializeField]
        private TMP_Text sRestBullet;

        public void Init()
        {
            GameEntry.Event.Subscribe(WeaponOnBulletChangedEventArgs.EventId, ChangeBullet);

        }

        public void OnClose()
        {
            GameEntry.Event.Unsubscribe(WeaponOnBulletChangedEventArgs.EventId, ChangeBullet);

        }


        public void ChangeBullet(object sender, GameEventArgs e)
        {
            WeaponOnBulletChangedEventArgs weaponOnBulletChangedEventArgs = e as WeaponOnBulletChangedEventArgs;
            if (weaponOnBulletChangedEventArgs.WeaponTypeID == 30000)
            {
                pCurrentBullet.text = weaponOnBulletChangedEventArgs.CurrentBullet.ToString();
                pRestBullet.text = "/" + weaponOnBulletChangedEventArgs.RestBullet.ToString();

            }
            if (weaponOnBulletChangedEventArgs.WeaponTypeID == 30001)
            {
                sCurrentBullet.text = weaponOnBulletChangedEventArgs.CurrentBullet.ToString();
                sRestBullet.text = "/" + weaponOnBulletChangedEventArgs.RestBullet.ToString();
            }
        }
    }
}
