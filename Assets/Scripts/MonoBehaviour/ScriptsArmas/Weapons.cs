using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    public Handgun handgun;
    public Shotgun shotgun;
    public AK47 aK47;
    [HideInInspector] public List<Weapon> WeaponList;
    public int ammountOfGuns;

    public void AddWeaponsToList()
    {
        foreach(Weapon weapon in GetComponentsInChildren<Weapon>())
        {
            WeaponList.Add(weapon);
        }
        ammountOfGuns = WeaponList.Count;
    }

    public Weapon GetWeapon(int GunType)
    {
        return WeaponList[GunType];
    }

    public int SelectNextWeapon(int GunNum)
    {
        GunNum++;
        //GunType %= (ammountOfGuns+1);
        if (GunNum < ammountOfGuns)
        {
            //print(GunNum);
            if (WeaponList[GunNum] != null && WeaponList[GunNum].isAvailable)
            {
                return GunNum;
            }
            else
                return 0;
        }
        else return 0;
    }

    public int SelectPreviousWeapon(int GunNum)
    {
        GunNum--;
        if (GunNum >= 0)
        {
            print(GunNum);
            if (WeaponList[GunNum] != null && WeaponList[GunNum].isAvailable)
                return GunNum;
            else
                return ammountOfGuns - 1;
        }
        else if (WeaponList[ammountOfGuns - 1].isAvailable)
            return ammountOfGuns - 1;
        return ++GunNum;
    }
}
