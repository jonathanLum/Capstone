using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameBoard
{
    public string name = "default";
    public List<DataTransferObject> board = new List<DataTransferObject>();
}
