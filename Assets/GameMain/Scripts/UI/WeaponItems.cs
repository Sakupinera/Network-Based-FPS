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
        private GameObject primaryWeapon;
        private Transform pCurrentBullet;
        private Transform pRestBullet;
        private TextMeshPro pCurrentText;
        private TextMeshPro pRestText;

        //副武器的UI
        [SerializeField]
        private GameObject secondaryWeapon;
        private Transform sCurrentBullet;
        private Transform sRestBullet;
        private TextMeshPro sCurrentText;
        private TextMeshPro sRestText;

        public void Init()
        {
            pCurrentBullet = primaryWeapon.transform.Find("currentBullet");
            pRestBullet = primaryWeapon.transform.Find("restBullet");
            pCurrentText = pCurrentBullet.gameObject.GetComponent<TextMeshPro>();
            pRestText = pRestBullet.gameObject.GetComponent<TextMeshPro>();

            sCurrentBullet = secondaryWeapon.transform.Find("currentBullet");
            sRestBullet = secondaryWeapon.transform.Find("restBullet");
            sCurrentText = sCurrentBullet.gameObject.GetComponent<TextMeshPro>();
            sRestText = sRestBullet.gameObject.GetComponent<TextMeshPro>();

            GameEntry.Event.Subscribe(WeaponOnBulletChangedEventArgs.EventId, ChangeBullet);
        }

        

        public void ChangeBullet(object sender, GameEventArgs e)
        {
            WeaponOnBulletChangedEventArgs weaponOnBulletChangedEventArgs = e as WeaponOnBulletChangedEventArgs;

            if (weaponOnBulletChangedEventArgs.WeaponTypeID == 30000)
            {
                pCurrentText.text = weaponOnBulletChangedEventArgs.CurrentBullet.ToString();
                pRestText.text = "/" + weaponOnBulletChangedEventArgs.RestBullet.ToString();

            }
            if (weaponOnBulletChangedEventArgs.WeaponTypeID == 30001)
            {
                sCurrentText.text = weaponOnBulletChangedEventArgs.CurrentBullet.ToString();
                sRestText.text = "/" + weaponOnBulletChangedEventArgs.RestBullet.ToString();
            }
        }
    }
}
