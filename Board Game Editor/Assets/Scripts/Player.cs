using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject piece;

    public GameObject currTile;

    private void Start() {
        currTile = GameObject.Find("StartTile");
        piece = GameObject.Find("GamePiece");
    }

    public void Move(int spaces){
        Debug.Log("Move " + spaces + " spaces");
        currTile = currTile.GetComponent<Tile>().children[0];
        
    }
}
