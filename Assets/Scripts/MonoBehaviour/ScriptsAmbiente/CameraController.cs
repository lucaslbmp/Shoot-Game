using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;

    public GameObject playerObj;

    void Update()
    {
        //transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, offset.z); // Camera follows the player with specified offset position
        //UpdateAmmo();
    }

    void UpdateAmmo()
    {
        //int loaded_ammo = GameObject.Find("Player").GetComponent<Shooting>().loaded_ammo;
        //int remaining_ammo = GameObject.Find("Player").GetComponent<Shooting>().remaining_ammo;
        //GameObject.Find("ammoUI").GetComponent<Text>().text = loaded_ammo + "-" + remaining_ammo;
    }
}
