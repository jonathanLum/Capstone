using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public int ID;
    public GameObject piece;
    public GameObject currTile;
    public bool skipNextTurn = false;

    public int placement = 0;

    public int escapeRoll = 0;
}
