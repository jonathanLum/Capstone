using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public SaveController saveCtrl;
    public int numberOfPlayers;

    [SerializeField] bool gameOver = false;
    public int currentTurn = 0;

    [SerializeField]int roll = 0;
    public int spacesToMove = 0;
    [SerializeField]float speed = 1f;

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
        if(Input.GetMouseButtonDown(0) && !gameOver){
            spacesToMove = Random.Range(1,7);
            roll = spacesToMove;
            var player = players[currentTurn];
            if(player.escapeRoll == 0 || roll == player.escapeRoll){
                player.escapeRoll = 0;
                StartCoroutine(MovePiece());
            }else{
                IncrementTurn();
            } 
        }
    }


    IEnumerator MovePiece()
    {
        // get current player
        var player = players[currentTurn];
        player.piece.GetComponent<GamePiece>().moving = true;

        var inTransit = false;
        var targetPos = new Vector3();
        while(spacesToMove != 0){
            if(inTransit){
                var step = speed * Time.deltaTime;
                player.piece.transform.position = Vector3.MoveTowards(player.piece.transform.position, targetPos, step);
                if(Vector3.Distance(player.piece.transform.position, targetPos) < 0.001f){
                    inTransit = false;

                    spacesToMove -= (int)Mathf.Sign((float)spacesToMove);
                    if(player.currTile.GetComponent<Tile>().children.Count == 0){
                        spacesToMove = 0;
                        // win game
                        gameOver = true;
                        Debug.Log("Player " + player.ID + " Wins!");
                        break;
                    }
                    if(!gameOver && spacesToMove == 0)
                        ApplyEffect();
                }
            }else{
                if(Mathf.Sign(spacesToMove) == 1){
                    player.currTile = player.currTile.GetComponent<Tile>().children[0];
                }else{
                    player.currTile = player.currTile.GetComponent<Tile>().parent;
                }
                targetPos = player.currTile.GetComponent<Tile>().GetLocation() + spaceOffsets[player.ID];
                player.piece.transform.LookAt(targetPos);
                inTransit = true;
            }
            yield return null;
        }
        player.piece.GetComponent<GamePiece>().moving = false;
        if(!gameOver)
            IncrementTurn();
    }

    void ApplyEffect(){
        var player = players[currentTurn];
        player.currTile.GetComponent<Tile>().LandedOn(gameObject.GetComponent<GameManager>());
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
        //cam.SetTarget(players[currentTurn].piece);
    }
}
