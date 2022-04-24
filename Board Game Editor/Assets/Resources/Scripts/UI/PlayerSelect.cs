using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelect : MonoBehaviour
{
    public TMPro.TextMeshProUGUI textPlayerCount;
    public int playerCount;
    public List<GameObject> pieceSelectors;
    public List<GameObject> activePieceSelectors;
    public GameObject playerCountSelect;
    public List<Material> pieceColors;

    [SerializeField] LinkedList<Material> availablePieceColors;

    public List<Material> selectedPieceColors;

    public enum DIRECTION { LEFT, RIGHT };

    // Start is called before the first frame update
    void Start()
    {
        availablePieceColors = new LinkedList<Material>(pieceColors);
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
    }

    public void InitColorToPiece(GameObject piece)
    {
        Image image = piece.GetComponentInChildren<Image>();
        // Debug.Log(image.material.ToString());
        // Debug.Log(image.defaultMaterial);
        if (image.material == image.defaultMaterial)
        {
            image.material = availablePieceColors.Last.Value;
            availablePieceColors.RemoveLast();
            selectedPieceColors.Add(image.material);
        }

    }

    public void RemoveColorFromPiece(GameObject piece)
    {
        Image image = piece.GetComponentInChildren<Image>();

        if (image.material != image.defaultMaterial)
        {
            availablePieceColors.AddLast(image.material);
            selectedPieceColors.Remove(image.material);
            image.material = image.defaultMaterial;

        }

    }
    public void DecrementPlayerCount()
    {
        if (playerCount > 1)
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
    }

    public void ChangeColor(GameObject pieceSelector, DIRECTION direction)
    {
        Material nextMaterial = availablePieceColors.Last.Value;
        Material currentMaterial = pieceSelector.GetComponentInChildren<Image>().material;

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


        pieceSelector.GetComponentInChildren<Image>().material = nextMaterial;

    }
}
