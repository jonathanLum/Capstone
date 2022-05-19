using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;


[System.Serializable]
public class MyIntEvent : UnityEvent<int>{}

public class GameManager : MonoBehaviour
{
    public UnityEvent changeTurn;
    public MyIntEvent gameOverEvent;

    public SaveController saveCtrl;
    public GameDataController gameData;
    public int numberOfPlayers;

    public bool gameOver = false;
    public bool attacking = false;
    public bool choosingDirection = false;
    public int currentTurn = 0;
    int dir = 0;

    public GameObject dice;

    int roll = 0;
    public int spacesToMove = 0;
    [SerializeField] float speed = 1f;

    [SerializeField] public List<Player> players = new List<Player>();
    [SerializeField] List<GameObject> arrows;
    [SerializeField] List<GameObject> placementSpawns;

    public CameraController cameraController;
    public GameObject diceCamera;

    public List<GameObject> allTiles;

    public Vector3[] spaceOffsets = new Vector3[]{new Vector3(.25f,0f,0f), new Vector3(-.25f,0f,0f),
                                                new Vector3(0f,0f,.25f), new Vector3(0f,0f,-.25f),
                                                new Vector3(0f, 0f, 0f)};
    public bool gamePaused;
    public NotificationManager notifications;
    // Start is called before the first frame update
    void Awake()
    {
        // notificationQueue = new Queue<string>();
        notifications = gameObject.GetComponent<NotificationManager>();
        gamePaused = false;
        saveCtrl = GameObject.FindGameObjectWithTag("SaveController").GetComponent<SaveController>();
        gameData = GameObject.FindGameObjectWithTag("GameDataController").GetComponent<GameDataController>();
        numberOfPlayers = gameData.playerCount;

        for (int i = 0; i < numberOfPlayers; i++)
        {
            Player player = new Player();
            players.Add(player);
            player.ID = i;
        }
    }

    void Start()
    {
        StartCoroutine(notifications.NotifyLoop());
        notifications.Notify("Player " + (players[currentTurn].ID + 1).ToString() + " Turn");
        
        Queue<Material> pieceColors = new Queue<Material>(gameData.pieceColors);
        foreach (Player plr in players)
        {
            plr.currTile = allTiles[0];
            plr.piece = (GameObject)GameObject.Instantiate(
                Resources.Load("Prefabs/GamePiece"), plr.currTile.GetComponent<Tile>().GetLocation() + spaceOffsets[plr.ID],
                Quaternion.Euler(0, 180, 0));

            plr.piece.GetComponentInChildren<SkinnedMeshRenderer>().material = pieceColors.Dequeue();
        }

        cameraController = GameObject.FindGameObjectWithTag("CameraController").GetComponent<CameraController>();
        cameraController.playerTarget = players[currentTurn].piece.transform;
    }

    void Update()
    {
        // wait for player to roll dice
        if (dice.GetComponent<Dice>().rolled && !gameOver)
        {
            //Hide dice
            diceCamera.SetActive(false);
            spacesToMove = dice.GetComponent<Dice>().number;
            roll = spacesToMove;
            var player = players[currentTurn];
            if (player.escapeRoll == 0 || roll == player.escapeRoll)
            {
                player.escapeRoll = 0;
                StartCoroutine(TakeTurn());
            }
            else
            {
                IncrementTurn();
            }
            dice.GetComponent<Dice>().Reset();
        }
    }


    IEnumerator TakeTurn()
    {
        // get current player
        var player = players[currentTurn];
        player.piece.GetComponent<GamePiece>().moving = true;

        var inTransit = false;
        var targetPos = new Vector3();
        while (spacesToMove != 0 || choosingDirection)
        {
            if (inTransit)
            {
                var step = speed * Time.deltaTime;
                player.piece.transform.position = Vector3.MoveTowards(player.piece.transform.position, targetPos, step);
                if (Vector3.Distance(player.piece.transform.position, targetPos) < 0.001f)
                {
                    inTransit = false;

                    spacesToMove -= (int)Mathf.Sign((float)spacesToMove);
                    if (player.currTile.GetComponent<Tile>().children.Count == 0)
                    {
                        spacesToMove = 0;
                        // win game
                        GameOver();
                        break;
                    }
                    if (!gameOver && spacesToMove == 0)
                        ApplyEffect();
                }
            }
            else
            {
                if (Mathf.Sign(spacesToMove) == 1)
                {
                    if (player.currTile.GetComponent<Tile>().children.Count > 1)
                    {
                        choosingDirection = true;
                        // show UI
                        PlaceArrows();
                        //wait for choice
                        while (choosingDirection)
                        {
                            //Debug.Log("waiting for choice");
                            yield return null;
                        }
                        HideArrows();
                        //set currtile to choice
                        player.currTile = player.currTile.GetComponent<Tile>().children[dir];
                    }
                    else
                    {
                        player.currTile = player.currTile.GetComponent<Tile>().children[0];
                    }
                }
                else
                {
                    player.currTile = player.currTile.GetComponent<Tile>().parent;
                }
                targetPos = player.currTile.GetComponent<Tile>().GetLocation() + spaceOffsets[player.ID];
                player.piece.transform.LookAt(targetPos);
                inTransit = true;
            }
            yield return null;
        }

        player.piece.GetComponent<GamePiece>().moving = false;

        // Wait for player to choose during attack
        while (attacking)
        {
            yield return null;
        }

        if (!gameOver)
            yield return new WaitForSeconds(1.5f);
            IncrementTurn();
    }

    void ApplyEffect()
    {
        var player = players[currentTurn];
        player.currTile.GetComponent<Tile>().LandedOn(gameObject.GetComponent<GameManager>());

        string effectText = player.currTile.GetComponent<Tile>().GetTileMessage();
        if (effectText != null)
        {
            notifications.Notify(effectText);
        }
    }

    void IncrementTurn()
    {
        if (gameOver)
            return;

        diceCamera.SetActive(true);
        currentTurn += 1;
        changeTurn.Invoke();

        if (currentTurn > players.Count - 1)
        {
            currentTurn = 0;
        }
        if (players[currentTurn].skipNextTurn == true)
        {
            players[currentTurn].skipNextTurn = false;
            notifications.Notify("Player " + (players[currentTurn].ID + 1).ToString() + " Skipped");
            IncrementTurn();
            return;
        }

        var player = players[currentTurn];
        cameraController.playerTarget = players[currentTurn].piece.transform;

        notifications.Notify("Player " + (players[currentTurn].ID + 1).ToString() + " Turn");
        if (player.escapeRoll > 0)
        {
            notifications.Notify("Roll " + player.escapeRoll + " to escape");
        }
    }

    void GameOver(){
        gameOver = true;
        gameOverEvent.Invoke(currentTurn+1);
        diceCamera.SetActive(false);

        CalculatePlacements();

        // log all placements
        foreach(Player player in players){
            Debug.Log("Player " + (player.ID + 1) + ": Placecement - " + player.placement);
        }

        // display placement screen
        List<int> taken = new List<int>();
        foreach(Player player in players){
            switch(player.placement){
                case 1:
                    player.piece.transform.position = placementSpawns[player.placement-1].transform.position;
                    player.piece.transform.rotation = placementSpawns[player.placement-1].transform.rotation;
                    break;
                case 2:
                    player.piece.transform.position = new Vector3(placementSpawns[player.placement-1].transform.position.x - (taken.FindAll(p => p == 2).Count()*0.3f),
                                                                  placementSpawns[player.placement-1].transform.position.y, 
                                                                  placementSpawns[player.placement-1].transform.position.z);
                    player.piece.transform.rotation = placementSpawns[player.placement-1].transform.rotation;
                    taken.Add(2);
                    break;
                case 3:
                    player.piece.transform.position = new Vector3(placementSpawns[player.placement-1].transform.position.x + (taken.FindAll(p => p == 3).Count()*0.3f),
                                                                  placementSpawns[player.placement-1].transform.position.y, 
                                                                  placementSpawns[player.placement-1].transform.position.z);
                    player.piece.transform.rotation = placementSpawns[player.placement-1].transform.rotation;
                    taken.Add(3);
                    break;
                case 4:
                    player.piece.transform.position = placementSpawns[player.placement-1].transform.position;
                    player.piece.transform.rotation = placementSpawns[player.placement-1].transform.rotation;
                    break;
            }
        }
    }

    void CalculatePlacements(){
        List<int> distances = new List<int>();
        foreach(Player player in players){
            distances.Add(Step(player.currTile));
        }

        int i = 0;
        int place = 1;
        int last = 0;
        while(i < numberOfPlayers){
            if(distances.Min() != last){
                place += 1;
            }
            var index = distances.FindIndex(d => d == distances.Min());
            players[index].placement = place;
            last = distances.Min();
            distances[index] = int.MaxValue;
            i++;
        }
    }

    int Step(GameObject tile){
        Tile tileCode = tile.GetComponent<Tile>();
        int count = 1;
        if(tileCode.children.Count == 0){
            return 0;
        }
        int childCount = int.MaxValue;
        foreach(GameObject child in tileCode.children){
            childCount = Mathf.Min(childCount, Step(child));
        }
        count += childCount;
        return count;
    }

    void PlaceArrows()
    {
        var children = players[currentTurn].currTile.GetComponent<Tile>().children;
        var max = children.Count;
        for (int i = 0; i < max; i++)
        {
            arrows[i].transform.position =
                children[i].GetComponent<Tile>().GetLocation() + new Vector3(0f, 0.25f, 0f);
            arrows[i].transform.LookAt(players[currentTurn].currTile.GetComponent<Tile>().GetLocation(), Vector3.up);
            arrows[i].transform.eulerAngles = new Vector3(0, arrows[i].transform.eulerAngles.y, 0);
            arrows[i].SetActive(true);
        }
    }

    void HideArrows()
    {
        foreach (GameObject arrow in arrows)
        {
            arrow.SetActive(false);
        }
    }

    public void DirectionChosen(int childID)
    {
        dir = childID;
        choosingDirection = false;
        //Debug.Log("chosen");
    }

    public void FireLaser(GameObject target){
        float heightOffset = 0.531f;
        Vector3 pos = new Vector3(players[currentTurn].piece.transform.position.x, players[currentTurn].piece.transform.position.y+heightOffset, players[currentTurn].piece.transform.position.z);
        GameObject laser = (GameObject)Instantiate(Resources.Load("Prefabs/Laser"), pos, Quaternion.identity);
        laser.GetComponent<Laser>().target = target;
        cameraController.playerTarget = laser.transform;
    }
}

