using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleToCreate : MonoBehaviour {

    public ObstacleToCreate[] dependedObstacles;
    public int probabilityToAppear = 1;
    [SerializeField] private bool obstaclesAreToConnect;

    public bool ObstaclesAreToConnect { get { return obstaclesAreToConnect; } }
    public bool ObstaclesAreNotToConnect { get { return obstaclesAreToConnect == false; } }



}
