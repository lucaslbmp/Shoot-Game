using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Autor: Gilmar Correia
/// Data: 21/04/2021
/// Classe que gerencia a mudan�a de andar do player
/// </summary>

public class MoveToPos : MonoBehaviour
{
 
    public GameObject pos; // GameObject que vai ser a posi��o de deslocamento do jogador

    Color iluminado = new Color(0.73f, 0.73f, 0.73f, 1.0f);  // Cor da tilemap quando o ambiente � ilumin�vel
    Color apagado = new Color(0.142f, 0.142f, 0.142f, 1.0f); // Cor da tilemap quando o ambiente est� apagado

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))                                               // Se colidiu com o player...
        {
            GameObject hallSecondFloor = GameObject.Find("Layer_HallFloor(Second)");      // Encontra o layer do 2� andar
            hallSecondFloor.GetComponent<Tilemap>().color = iluminado;                    // Ilumina o 2� andar

            GameManager gameManager = GameObject.Find("RPGGameManager").GetComponent<GameManager>(); // Obtem o GameManager
            gameManager.cameraManager.cameraConfiner.enabled = false;            // Desabilita o confiner da camera
            
            collision.transform.position = new Vector3(pos.transform.position.x,pos.transform.position.y); // Muda a posi�o do jogador para a posi�ao de pos

            collision.GetComponent<PlayerMovement>().UpdatePlayerRotation();            // Atualiza a rota�ao do player

            gameManager.cameraManager.virtualCamera.Follow = collision.transform;       // Atribui a posi�ao do player ao atributo Follow da Virtual Camera
        }
    }
}
