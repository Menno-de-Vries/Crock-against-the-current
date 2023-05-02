using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = ("Player Data"), order = 0)]

public class PlayerData : ScriptableObject
{

    [Tooltip("The time the player has to move to the right lane")]
    public float TheTimeToMoveToLane = 0.25f;

    [Tooltip("The distance to check how close the player is to this pos on the x axis")]
    public float Distance = 0.3f;

    [Tooltip("The time it takes before the crocodile can move again")]
    public float WaitingTimeToMove = 0.5f;

}
