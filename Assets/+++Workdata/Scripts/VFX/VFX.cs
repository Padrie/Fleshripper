using System;
using UnityEngine;

[Serializable]
public class VFX
{
    public string name;

    public GameObject[] vfx;

    public bool playOnStart = true;

    public bool loop = false;
}
