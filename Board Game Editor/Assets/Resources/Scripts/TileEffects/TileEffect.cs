using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class TileEffect : ScriptableObject
{
    public abstract void Apply(GameManager gameManager, float effectValue);

    public abstract string message { get; }
}
