using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MoveToPos : MonoBehaviour
{
 
    public GameObject pos; // GameObject que vai ser a posição de deslocamento do jogador

    Color iluminado = new Color(0.73f, 0.73f, 0.73f, 1.0f);  // Cor da tilemap quando o ambiente é iluminável
    Color apagado = new Color(0.142f, 0.142f, 0.142f, 1.0f); // Cor da tilemap quando o ambiente está apagado

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameObject hallSecondFloor = GameObject.Find("Layer_HallFloor(Second)");
            hallSecondFloor.GetComponent<Tilemap>().color = iluminado;

            GameManager gameManager = GameObject.Find("RPGGameManager").GetComponent<GameManager>();
            gameManager.cameraManager.cameraConfiner.enabled = false;
            
            collision.transform.position = new Vector3(pos.transform.position.x,pos.transform.position.y);

            collision.GetComponent<PlayerMovement>().UpdatePlayerRotation();

            gameManager.cameraManager.virtualCamera.Follow = collision.transform;
        }
    }
}
