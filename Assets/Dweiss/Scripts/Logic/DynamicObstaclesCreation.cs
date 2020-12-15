using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Dweiss;

namespace Dweiss
{
    public class DynamicObstaclesCreation : MonoBehaviour
    {
        public Transform positionOfObstacles;
        public Vector3 shiftValue = new Vector3(0, 0, 1);

        public int seed;
        public ObstacleToCreate startObstacle, endObstacle;
        public List<ObstacleToCreate> prefabs = new List<ObstacleToCreate>();


        public int createOnStart = 10;

        public class ObstaclesWithCreationProb
        {
            public ObstacleToCreate obstacle;
            public Bounds myBounds { get { return obstacle.gameObject.TotalMeshBounds(true); } }
            public ObstacleToCreate[] canBeConnected;

            public int totalCount;

            public ObstaclesWithCreationProb(ObstacleToCreate newObstacle, List<ObstacleToCreate> allList)
            {
                obstacle = newObstacle;
                canBeConnected = obstacle.ObstaclesAreToConnect ? obstacle.dependedObstacles.ToArray() :
                    allList.Where(a => obstacle.dependedObstacles.Contains(a) == false).ToArray();
                totalCount = canBeConnected.Sum(a => a.probabilityToAppear);

               // myBounds = obstacle.gameObject.TotalMeshBounds(true);
            }
            public ObstaclesWithCreationProb(ObstacleToCreate newObstacle, ObstacleToCreate[] newList)
            {
                obstacle = newObstacle;
                canBeConnected = newList;
                totalCount = canBeConnected.Sum(a => a.probabilityToAppear);
                //myBounds = obstacle.gameObject.TotalMeshBounds(true);
            }
            //public int probability;
        }


        private List<System.Tuple<GameObject, ObstaclesWithCreationProb>> currentList = new List<System.Tuple<GameObject, ObstaclesWithCreationProb>>();

        public ObstacleToCreate[] GetObstaclesArr
        {
            get { return currentList.Select(a => a.Item1.GetComponent<ObstacleToCreate>()).ToArray(); }
        }

        private ObstaclesWithCreationProb current;

        private Dictionary<ObstacleToCreate, ObstaclesWithCreationProb> obstaclesWithDependencies =
            new Dictionary<ObstacleToCreate, ObstaclesWithCreationProb>();

        private System.Random rand;
        void Awake()
        {
            rand = new System.Random(seed);
            InitSetup();
        }

        private void Reset()
        {
            positionOfObstacles = transform;
        }

        private void OnEnable()
        {

            Init();
        }
        private void OnDisable()
        {
            for (int i = 0; i < currentList.Count; ++i)
            {
                Destroy(currentList[i].Item1);
            }
            currentList.Clear();
        }


        void Init()
        {
            InitObstacles();
            //CreateShift(1);
        }

        //TODO remove this
        public void CreateShift(float distance)
        {
            ChangeShift(
                (index) => { return new Vector3(0, 0, 1); },
                (index) =>
                {
                    return index == 0 ? Vector3.zero :
          new Vector3(0, 0, Random.Range(distance * 0.8f, distance * 1.2f));
                });
        }

        private void InitObstacles()
        {
            for (int i = 0; i < createOnStart - 1; ++i)
            {
                GetNextObstacle(false);
            }
            if (createOnStart > 0)
                GetNextObstacle(true);
        }

        private void InitSetup()
        {
            //prefabs.Add(startObstacle);
            //prefabs.Add(endObstacle);

            var dependencyDic = new Dictionary<ObstacleToCreate, HashSet<ObstacleToCreate>>();
            for (int i = 0; i < prefabs.Count; ++i)
            {
                var obstacle = prefabs[i];
                dependencyDic.GetOrInit(obstacle).AddRange(
                    obstacle.ObstaclesAreToConnect ?
                    obstacle.dependedObstacles :
                    prefabs.Where(a => obstacle.dependedObstacles.Contains(a) == false));
            }

            for (int i = 0; i < prefabs.Count; ++i)
            {
                var obstacle = prefabs[i];
                if (obstacle.ObstaclesAreToConnect == false)
                {
                    for (int j = 0; j < obstacle.dependedObstacles.Length; ++j)
                    {
                        var v = obstacle.dependedObstacles[j];
                        dependencyDic.GetOrInit(v).Remove(obstacle);
                    }
                }
            }

            for (int i = 0; i < prefabs.Count; ++i)
            {
                var current = prefabs[i];
                var depended = dependencyDic[prefabs[i]].ToArray();
                obstaclesWithDependencies[current] = new ObstaclesWithCreationProb(current, depended);
            }

            obstaclesWithDependencies[endObstacle] = new ObstaclesWithCreationProb(endObstacle, prefabs);
            obstaclesWithDependencies[startObstacle] = new ObstaclesWithCreationProb(startObstacle, prefabs);
        }


        public void ChangeShift(System.Func<int, Vector3> shiftFactorGenerator, System.Func<int, Vector3> shiftAddedGenerator)
        {   
            GameObject prev = null;
            for (int i = 0; i < currentList.Count; ++i)
            {
                var t = prev == null ? positionOfObstacles : prev.transform;
                var newObs = currentList[i];
                newObs.Item1.transform.SetPositionAndRotation(t.position, t.rotation);

                //var meshBounds = newObs.Item2.myBounds;

                var meshBounds = newObs.Item1.TotalMeshBounds(true);

                if (prev != null)
                {

                    // var shift = t.rotation * (newObs.Item1.transform.rotation.Inverse() * meshBounds.size).PointMul(shiftValue);


                    //var shift = t.rotation * ( meshBounds.size).PointMul(shiftValue);

                    var shift = t.rotation * (newObs.Item1.transform.rotation.Inverse() *
                        (shiftAddedGenerator(i) + meshBounds.size.PointMul(shiftFactorGenerator(i)))
                        );
                    newObs.Item1.transform.position = t.position + shift;
                }

                prev = newObs.Item1;
            }
        }

        private void Create(ObstacleToCreate obst)
        {
            GameObject prev = null;
            if (currentList.Count != 0) prev = currentList[currentList.Count - 1].Item1;

            var newObs = Instantiate(obst.gameObject, positionOfObstacles);
            currentList.Add(System.Tuple.Create(newObs, obstaclesWithDependencies[obst]));

            var t = prev == null ? positionOfObstacles : prev.transform;
            newObs.transform.SetPositionAndRotation(t.position, t.rotation);

            var meshBounds = newObs.TotalMeshBounds(true);

            if (prev != null)
            {
                var shift = t.rotation * (newObs.transform.rotation.Inverse() * meshBounds.size).PointMul(shiftValue);
                newObs.transform.position += shift;
            }

            current = obstaclesWithDependencies[obst];
        }
        public ObstacleToCreate GetNextObstacle(bool end)
        {
            if (end)
            {
                Create(endObstacle);
            }
            else
            {
                if (current == null)
                {
                    Create(startObstacle);
                }
                else
                {
                    var probCount = rand.Next(0, current.totalCount);
                    for (int i = 0; i < current.canBeConnected.Length; ++i)
                    {
                        probCount -= current.canBeConnected[i].probabilityToAppear;
                        if (probCount < 0)
                        {
                            Create(current.canBeConnected[i]);
                            break;

                        }
                    }
                }
            }
            return current.obstacle;
        }

    }
}