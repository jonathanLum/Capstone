using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameDataController : MonoBehaviour
{

    public int playerCount = 4;
    public List<Material> pieceColors;


    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {

    }

    public void SetPlayerCount(int count)
    {
        playerCount = count;
    }

    public void UpdatePieceColors(List<Material> newPieceColors)
    {
        pieceColors = newPieceColors;
    }
}
