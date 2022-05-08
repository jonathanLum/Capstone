using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public Texture2D icon;
    public GameObject iconPlane;
    public TileEffect effect;
    public float effectValue;
    public GameObject parent;
    public List<GameObject> children;

    private void Start()
    {
        iconPlane.GetComponent<Renderer>().material.SetTexture("_Texture2D", icon);
    }

    public void LandedOn(GameManager gameManager)
    {
        if (effect != null)
        {
            effect.Apply(gameManager, effectValue);
        }
    }

    public Vector3 GetLocation()
    {
        return transform.position + new Vector3(0.5f, 0, 0.5f);
    }

    public string GetTileText()
    {
        if (effect != null)
        {
            return effect.text;
        }

        return null;
    }

}
