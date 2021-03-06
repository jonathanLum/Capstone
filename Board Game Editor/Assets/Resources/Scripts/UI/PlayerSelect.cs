using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelect : MonoBehaviour
{
    public GameDataController gameData;

    public TMPro.TextMeshProUGUI textPlayerCount;
    public int playerCount;
    public GameObject playerCountSelect;

    public List<GameObject> pieceSelectors;
    public List<GameObject> activePieceSelectors;
    public List<Material> pieceColors;

    public LinkedList<Material> availablePieceColors;

    public List<Material> selectedPieceColors;

    public enum DIRECTION { LEFT, RIGHT };

    public Dictionary<Color, Material> colorMapping;

    public Material defaultGamePiece;

    // Start is called before the first frame update
    void Start()
    {
        gameData = GameObject.FindGameObjectWithTag("GameDataController").GetComponent<GameDataController>();

        colorMapping = new Dictionary<Color, Material>();
        foreach (Material material in pieceColors)
        {
            colorMapping.Add(material.color, material);
        }

        availablePieceColors = new LinkedList<Material>(pieceColors);
        selectedPieceColors = new List<Material> { null, null, null, null };

        SetPlayerCount(4);
        InitButtonColorSelect();
    }


    // Update is called once per frame
    void Update()
    {

    }

    void InitButtonColorSelect()
    {
        foreach (GameObject pieceSelector in pieceSelectors)
        {

            Button[] buttons = pieceSelector.GetComponentsInChildren<Button>();
            foreach (Button button in buttons)
            {
                DIRECTION direction;

                if (button.name == "ButtonArrowLeft")
                {
                    direction = DIRECTION.LEFT;
                }

                else
                {
                    direction = DIRECTION.RIGHT;
                }

                button.onClick.AddListener(() => { ChangeColor(pieceSelector, direction); });

            }
        }
    }

    void ChangeDisplay(int value)
    {

        foreach (GameObject piece in pieceSelectors)
        {
            if (pieceSelectors.IndexOf(piece) < value)
            {

                piece.SetActive(true);
                InitColorToPiece(piece);

                if (!(activePieceSelectors.Contains(piece)))
                {
                    activePieceSelectors.Add(piece);
                }
            }
            else
            {
                piece.SetActive(false);
                RemoveColorFromPiece(piece);
                activePieceSelectors.Remove(piece);
            }
        }

        gameData.UpdatePieceColors(selectedPieceColors);
    }

    public void InitColorToPiece(GameObject piece)
    {
        SkinnedMeshRenderer mesh = piece.GetComponentInChildren<SkinnedMeshRenderer>();
        Material[] sharedPieceMaterials = mesh.sharedMaterials;


        if (sharedPieceMaterials[2] == defaultGamePiece)
        {
            Material[] pieceMaterials = mesh.materials;
            pieceMaterials = mesh.materials;
            pieceMaterials[2] = availablePieceColors.Last.Value;
            mesh.materials = pieceMaterials;

            mesh.sharedMaterial = availablePieceColors.Last.Value;


            selectedPieceColors[pieceSelectors.IndexOf(piece)] = availablePieceColors.Last.Value;
            availablePieceColors.RemoveLast();
        }
    }

    public void RemoveColorFromPiece(GameObject piece)
    {
        SkinnedMeshRenderer mesh = piece.GetComponentInChildren<SkinnedMeshRenderer>();
        Material[] sharedPieceMaterials = mesh.sharedMaterials;

        if (sharedPieceMaterials[2] != defaultGamePiece)
        {
            availablePieceColors.AddLast(sharedPieceMaterials[2]);
            selectedPieceColors[pieceSelectors.IndexOf(piece)] = defaultGamePiece;//colorMapping[image.color];
            mesh.sharedMaterial = defaultGamePiece;

            Material[] pieceMaterials = mesh.materials;
            pieceMaterials[2] = defaultGamePiece;
            mesh.materials = pieceMaterials;
        }

    }
    public void DecrementPlayerCount()
    {
        if (playerCount > 2)
        {
            SetPlayerCount(playerCount - 1);
        }
    }

    public void IncrementPlayerCount()
    {
        if (playerCount < 4)
        {
            SetPlayerCount(playerCount + 1);
        }
    }

    public void SetPlayerCount(int value)
    {
        playerCount = value;
        textPlayerCount.text = value.ToString();
        ChangeDisplay((int)value);
        gameData.SetPlayerCount(value);
    }

    public void ChangeColor(GameObject pieceSelector, DIRECTION direction)
    {
        Material nextMaterial = availablePieceColors.Last.Value;
        Material currentMaterial = pieceSelector.GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial; //colorMapping[pieceSelector.GetComponentInChildren<Image>().color];

        switch (direction)
        {
            case DIRECTION.LEFT:
                nextMaterial = availablePieceColors.First.Value;
                availablePieceColors.RemoveFirst();
                availablePieceColors.AddLast(currentMaterial);
                break;

            case DIRECTION.RIGHT:
                nextMaterial = availablePieceColors.Last.Value;
                availablePieceColors.RemoveLast();
                availablePieceColors.AddFirst(currentMaterial);
                break;
        }



        SkinnedMeshRenderer mesh = pieceSelector.GetComponentInChildren<SkinnedMeshRenderer>();
        mesh.sharedMaterial = nextMaterial;
        selectedPieceColors[pieceSelectors.IndexOf(pieceSelector)] = nextMaterial;

        Material[] pieceMaterials = mesh.materials;
        pieceMaterials[2] = nextMaterial;
        mesh.materials = pieceMaterials;

    }
}
