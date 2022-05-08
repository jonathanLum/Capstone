using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public UnityEvent changeTurn;

    public SaveController saveCtrl;
    public GameDataController gameData;
    public int numberOfPlayers;

    [SerializeField] bool gameOver = false;
    public bool attacking = false;
    public bool choosingDirection = false;
    public int currentTurn = 0;
    int dir = 0;

    public GameObject dice;

    [SerializeField] int roll = 0;
    public int spacesToMove = 0;
    [SerializeField] float speed = 1f;

    [SerializeField] public List<Player> players = new List<Player>();
    [SerializeField] List<GameObject> arrows;

    public CameraController cameraController;
    public GameObject diceCamera;

    public List<GameObject> allTiles;

    public Vector3[] spaceOffsets = new Vector3[]{new Vector3(.25f,0f,0f), new Vector3(-.25f,0f,0f),
                                                new Vector3(0f,0f,.25f), new Vector3(0f,0f,-.25f),
                                                new Vector3(0f, 0f, 0f)};
    public bool gamePaused;
    public Queue<string> notificationQueue;
    // Start is called before the first frame update
    void Awake()
    {
        notificationQueue = new Queue<string>();
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
        StartCoroutine(NotifyLoop());
        Notify("Player " + (players[currentTurn].ID + 1).ToString() + " Turn");

        Queue<Material> pieceColors = new Queue<Material>(gameData.pieceColors);
        foreach (Player plr in players)
        {
            plr.currTile = allTiles[0];
            plr.piece = (GameObject)GameObject.Instantiate(
                Resources.Load("Prefabs/GamePiece"), plr.currTile.GetComponent<Tile>().GetLocation() + spaceOffsets[plr.ID],
                Quaternion.identity);

            plr.piece.GetComponentInChildren<MeshRenderer>().material = pieceColors.Dequeue();
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
                        gameOver = true;
                        Debug.Log("Player " + player.ID + 1 + " Wins!");
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

        // Wait for player to choose during attack
        while (attacking)
        {
            yield return null;
        }

        player.piece.GetComponent<GamePiece>().moving = false;
        if (!gameOver)
            IncrementTurn();
    }

    void ApplyEffect()
    {
        var player = players[currentTurn];
        player.currTile.GetComponent<Tile>().LandedOn(gameObject.GetComponent<GameManager>());

        string effectText = player.currTile.GetComponent<Tile>().GetTileMessage();
        if (effectText != null)
        {
            Notify(effectText);
        }
    }

    IEnumerator NotifyLoop()
    {
        Transform parent = GameObject.Find("Notifications").transform;

        while (true)
        {
            Vector3 position = new Vector3(0, 0, 0);

            while (notificationQueue.Count > 0)
            {
                string text = notificationQueue.Dequeue();
                GameObject notification = (GameObject)GameObject.Instantiate(
                                           Resources.Load("UI/Notification"), new Vector3(0, 0, 0),
                                           Quaternion.identity, parent);
                position.y = -((parent.childCount - 1) * notification.GetComponent<RectTransform>().sizeDelta.y + (parent.childCount * 10));
                notification.GetComponent<RectTransform>().localPosition = position;
                notification.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = text;
                StartCoroutine(DisplayNotification(notification));
            }

            position.y = 0;
            foreach (Transform child in parent)
            {
                position.y -= child.gameObject.GetComponent<RectTransform>().sizeDelta.y + 10;
                child.gameObject.GetComponent<RectTransform>().localPosition = position;
            }
            yield return null;
        }
    }

    IEnumerator DisplayNotification(GameObject notification)
    {
        notification.SetActive(true);
        yield return new WaitForSeconds(2);
        notification.SetActive(false);
        DestroyImmediate(notification);
    }
    void Notify(string text)
    {
        notificationQueue.Enqueue(text);
    }

    void IncrementTurn()
    {
        diceCamera.SetActive(true);
        currentTurn += 1;
        if (currentTurn > players.Count - 1)
        {
            currentTurn = 0;
        }
        if (players[currentTurn].skipNextTurn == true)
        {
            players[currentTurn].skipNextTurn = false;
            Notify("Player " + (players[currentTurn].ID + 1).ToString() + " Skipped");
            IncrementTurn();
            return;
        }

        var player = players[currentTurn];
        cameraController.playerTarget = players[currentTurn].piece.transform;
        changeTurn.Invoke();

        Notify("Player " + (players[currentTurn].ID + 1).ToString() + " Turn");
        if (player.escapeRoll > 0)
        {
            Notify(player.currTile.GetComponent<Tile>().GetTileMessage());
        }
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
}

