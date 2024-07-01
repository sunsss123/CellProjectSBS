using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractiveObject : MonoBehaviour
{
    protected direction direct;
    public abstract void Active(direction direct);

    }
