using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int numberOfPlayers = 2;

    int currentTurn = 1;

    public List<Player> players;

    CameraController cam;
    
    
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i<numberOfPlayers; i++){
            players.Add(gameObject.AddComponent<Player>());
        }
        cam = GameObject.Find("Main Camera").GetComponent<CameraController>();
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)){
            Debug.Log("Pressed primary button.");
            players[currentTurn].Move(1);
        }
        cam.SetTarget(players[currentTurn].piece);
    }

}
