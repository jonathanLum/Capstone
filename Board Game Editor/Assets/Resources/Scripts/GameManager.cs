using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public SaveController saveCtrl;
    public int numberOfPlayers;

    int currentTurn = 0;

    [SerializeField] public List<Player> players = new List<Player>();

    CameraController cam;

    public List<GameObject> allTiles;

    public Vector3[] spaceOffsets = new Vector3[]{new Vector3(.25f,0f,0f), new Vector3(-.25f,0f,0f),
                                                new Vector3(0f,0f,.25f), new Vector3(0f,0f,-.25f),
                                                new Vector3(0f, 0f, 0f)};


    // Start is called before the first frame update
    void Awake()
    {
        saveCtrl = GameObject.FindGameObjectWithTag("SaveController").GetComponent<SaveController>();
        numberOfPlayers = saveCtrl.playerCount;

        for (int i = 0; i < numberOfPlayers; i++)
        {
            Player player = new Player();
            players.Add(player);
            player.ID = i;
        }
        cam = GameObject.Find("Main Camera").GetComponent<CameraController>();

    }

    void Start()
    {
        Queue<Material> pieceColors = new Queue<Material>(saveCtrl.pieceColors);
        foreach (Player plr in players)
        {
            plr.currTile = allTiles[0];
            plr.piece = (GameObject)GameObject.Instantiate(
                Resources.Load("Prefabs/GamePiece"), plr.currTile.GetComponent<Tile>().GetLocation() + spaceOffsets[plr.ID],
                Quaternion.identity);

            plr.piece.GetComponentInChildren<MeshRenderer>().material = pieceColors.Dequeue();
        }
        //Debug.Log(players);
    }

    void Update()
    {
        // wait for player to roll dice

        // take turn
        //TakeTurn();

    }


    void TakeTurn()
    {
        // move piece
        MovePiece();
        // check for victory

        // add tile effect

        // incrementTurn
        IncrementTurn();
    }


    void MovePiece()
    {
        // get current player
        var target = players[currentTurn];
    }

    void IncrementTurn()
    {

        currentTurn += 1;
        if (currentTurn > players.Count - 1)
        {
            currentTurn = 0;
        }
        if (players[currentTurn].skipNextTurn == true)
        {
            IncrementTurn();
        }
        cam.SetTarget(players[currentTurn].piece);
    }
}
