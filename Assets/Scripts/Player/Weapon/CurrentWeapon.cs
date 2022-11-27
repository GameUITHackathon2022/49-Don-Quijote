using LittleFoxLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;

public class CurrentWeapon: MonoBehaviour
{
    int maxBullet;
    public int currentBullet;
    public int CurrentBulletinRound;
    public WeaponBase weapon;
    public bool ReadyToShoot;
    public bool onReload = false;
    private void Start()
    {
        maxBullet = weapon.BullperRound * weapon.MaxRound;
        currentBullet = maxBullet - weapon.BullperRound;
        CurrentBulletinRound = weapon.BullperRound;
        ReadyToShoot = true;
        PlayerController.Instance.playerUI.ChangeBullet(CurrentBulletinRound, currentBullet);

    }
    public void ShootBulletCount()
    {
       CurrentBulletinRound -= 1;
        PlayerController.Instance.playerUI.ChangeBullet(CurrentBulletinRound, currentBullet);
        if (CurrentBulletinRound <=0)
        {
            if (currentBullet > 0)
            {
                PlayerController.Instance.AudioReloadSound();
                PlayerController.Instance.ReloadAction(1);
                Invoke(nameof(Reload), weapon.ReloadTime);
                ReadyToShoot = false;
                onReload = true ;
            }
            else
            {
                ReadyToShoot = false;
            }
        } 
    }
    public void GrabBullet(int number)
    {
        currentBullet += number;
        Reload();
    }
    public void Reload()
    {
        CurrentBulletinRound = currentBullet > weapon.BullperRound ? weapon.BullperRound : currentBullet;
        currentBullet -= CurrentBulletinRound;
        PlayerController.Instance.playerUI.ChangeBullet(CurrentBulletinRound, currentBullet);

        ReadyToShoot = true;
        onReload = false;
        PlayerController.Instance.ReloadAction(0);

    }
    public void OnReload()
    {
        if (currentBullet > 0)
        {
            PlayerController.Instance.AudioReloadSound();
            PlayerController.Instance.ReloadAction(1);
            Invoke(nameof(Reload), weapon.ReloadTime);
            ReadyToShoot = false;
            onReload = true;
        }
    }
}
