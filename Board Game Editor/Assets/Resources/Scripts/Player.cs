using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public int ID;
    public GameObject piece;

    public Color color;
    public GameObject currTile;
    public bool skipNextTurn = false;
    public int trapNumber = 0;
}
