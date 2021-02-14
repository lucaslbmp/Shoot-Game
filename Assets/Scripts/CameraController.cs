using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;

    public GameObject playerObj;
    public GameObject ammoTxtObj;

    void Update()
    {
        transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, offset.z); // Camera follows the player with specified offset position
        UpdateAmmo();
    }

    void UpdateAmmo()
    {
        //GameObject.Find("scoreUI").GetComponent<Text>().text = "Score: " + score;
    }
}
