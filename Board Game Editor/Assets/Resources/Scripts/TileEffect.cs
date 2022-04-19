using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class TileEffect : ScriptableObject {
    public abstract void Apply(Player target);
}
