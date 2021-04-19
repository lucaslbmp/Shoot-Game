using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DoorFeatures")]

public class DoorFeatures : ScriptableObject
{
    public Item keyItem;
    public float doorSpeed;
    public bool locked;
    public AudioClip doorOpenSound;
    public AudioClip doorCloseSound;
    public AudioClip lockSound;
    public AudioClip unlockSound;
}
