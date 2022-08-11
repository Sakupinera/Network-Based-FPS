using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 投掷物的类型
/// </summary>
public enum E_ThrowType
{
    Antitank,
    Smoke,
    Flash
}

/// <summary>
/// 投掷武器脚本
/// </summary>
public class ThrowWeapon : BaseWeapon
{
    //存储投掷物的数量
    public Dictionary<E_ThrowType, GameObject> throws2 = new Dictionary<E_ThrowType, GameObject>();

    //当前投掷物脚本 和 对象
    public ThrowBullet grenade;
    public GameObject throwObj;

    private void Awake()
    {
        //设置武器类型为 投掷物
        weaponType = E_WeaponType.Throw;

        //测试用的，在unity场景中直接拖入了预设体，以后商店购买再更新
        AddThrow(throwObj, throwObj.GetComponent<ThrowBullet>().ThrowType);

        Debug.Log(throws2.Count);

        //初始化投掷物
        if (throws2.Count != 0)
        {
            if (throws2.ContainsKey(E_ThrowType.Antitank))
            {
                throwObj = throws2[E_ThrowType.Antitank];
                grenade = throws2[E_ThrowType.Antitank].GetComponent<ThrowBullet>();
            }
            else if (throws2.ContainsKey(E_ThrowType.Smoke))
            {
                throwObj = throws2[E_ThrowType.Smoke];
                grenade = throws2[E_ThrowType.Smoke].GetComponent<ThrowBullet>();
            }
            else if (throws2.ContainsKey(E_ThrowType.Flash))
            {
                throwObj = throws2[E_ThrowType.Flash];
                grenade = throws2[E_ThrowType.Flash].GetComponent<ThrowBullet>();
            }
        }
    }

    /// <summary>
    /// 切换投掷物
    /// </summary>
    public void ChangeTrow()
    {

    }

    public void AddThrow(GameObject gameObject, E_ThrowType e_ThrowType)
    {
        throws2.Add(e_ThrowType, gameObject);
    }

    public override void Fire()
    {
        //判断投掷物字典中是否还有投掷物        
        if (throws2.Count == 0)
        {
            return;
        }
        //播放投掷动画

        //用于存储实例化子弹对象
        GameObject obj;
        shootDirection = shootPoint.forward;
        obj = Instantiate(grenade.gameObject, shootPoint.position, Quaternion.LookRotation(shootDirection));
        obj.GetComponent<ThrowBullet>().ShootDirction = Quaternion.LookRotation(shootDirection);
        obj.GetComponent<ThrowBullet>().SetFather(fatherObj);
        throws2.Remove(grenade.ThrowType);

        if (throws2.Count != 0)
        {
            //自动切换武器投掷武器
            if (throws2.ContainsKey(E_ThrowType.Antitank))
            {
                throwObj = throws2[E_ThrowType.Antitank];
                grenade = throws2[E_ThrowType.Antitank].GetComponent<ThrowBullet>();
                //可能播放切换手雷动画

            }
            else if (throws2.ContainsKey(E_ThrowType.Smoke))
            {
                throwObj = throws2[E_ThrowType.Smoke];
                grenade = throws2[E_ThrowType.Smoke].GetComponent<ThrowBullet>();
                //可能播放切换手雷动画

            }
            else if (throws2.ContainsKey(E_ThrowType.Flash))
            {
                throwObj = throws2[E_ThrowType.Flash];
                grenade = throws2[E_ThrowType.Flash].GetComponent<ThrowBullet>();
                //可能播放切换手雷动画

            }

        }
        //投掷物扔完之后自动切换武器
        else if (throws2.Count == 0)
        {
            if (fatherObj.WeaponDic.ContainsKey(E_WeaponType.Primary))
            {
                fatherObj.WeaponSwap(KeyCode.Alpha1);

            }
            else if (fatherObj.WeaponDic.ContainsKey(E_WeaponType.Secondary))
            {
                fatherObj.WeaponSwap(KeyCode.Alpha2);

            }
            else if (fatherObj.WeaponDic.ContainsKey(E_WeaponType.Close))
            {
                fatherObj.WeaponSwap(KeyCode.Alpha3);

            }
        }



    }
}
