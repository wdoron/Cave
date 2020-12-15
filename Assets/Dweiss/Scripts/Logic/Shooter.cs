using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dweiss;

namespace Dweiss {
    public class Shooter : MonoBehaviour
    {

        public interface IShooter
        {
            Transform trans{ get; }
            bool CanShoot { get; }
            void Shoot(GameObject bullet, Transform target);
        }

        [Header("Spawn")]
        public float animationCurveLength = 100;
        public AnimationCurve spawnCurve;
        public float minSpawnTime = 1.5f, spawnFactorDeviation = .3f;

        [Header("Prefab")]
        public float destroyAfter = 10;
        public GameObject toInstantiate;

        [Header("Positions")]
        public Transform[] allShooters;
        public Transform[] targets;
        public Transform bulletParent;
        private IShooter[] shooters;

        [Header("difficulty")]
        public float meterPerSec;
        public float accuracyInMetersRadius = .5f;//my accuracy 

        [Header("Work area")]
        [Tooltip("Set 0,0,0 for none")]
        public Vector3 planeOfReferenceForFieldOfView = Vector3.up;
        public float activeOnFieldOfView = 80;
        public float minDistToSpawn = 2;

        [Header("Utils")]
        public bool shouldSpawn = true;
        public int randomSeed;
        public bool shootFromRandomSources = false;


        private System.Random numGenerator;
        private void Awake()
        {
            numGenerator = new System.Random(randomSeed);
            if (targets == null || targets.Length == 0)
            {
                targets = new Transform[] { Camera.main.transform };
            }

            shooters = new IShooter[allShooters.Length];
            for (int i = 0; i < allShooters.Length; i++)
            {
                var iShooter = allShooters[i].GetComponent<IShooter>();
                if (iShooter == null)
                {
                    iShooter = allShooters[i].gameObject.AddComponent<SimpleShooter>();
                }
                shooters[i] = iShooter;

            }
        }

        private Transform NextTarget { get { return targets[numGenerator.Next(0, targets.Length)]; } }

        private void OnEnable()
        {
            StartCoroutine(ContinousSpawn()); 
        }

        private bool IsActive(Transform spawner, Transform target)
        {

            var relativeTo = spawner.position - target.position;
            var targetFor = target.forward;
            if (planeOfReferenceForFieldOfView.sqrMagnitude != 0) {
                targetFor = Vector3.ProjectOnPlane(target.forward, planeOfReferenceForFieldOfView);
                relativeTo = Vector3.ProjectOnPlane(relativeTo, planeOfReferenceForFieldOfView);
            }
            var angle = Vector3.Angle(targetFor, relativeTo);
            //Debug.LogFormat("{2} Angle {0} <= {1}", angle, activeOnFieldOfView, spawner);
            return angle <= activeOnFieldOfView;
        }
        private bool IsInRange(Transform spawner, Transform target)
        {
            var distance = (target.position - spawner.position).sqrMagnitude;
            var isClose = distance < (minDistToSpawn* minDistToSpawn);
            //Debug.LogFormat("{2} IsInRange {0} <= {1}", distance, (minDistToSpawn * minDistToSpawn), spawner);
            return isClose;
        }
        
        private float GetWaitTime(float count)
        {
            //averageSpawnTime, spawnDeviation
            var deviation = minSpawnTime * spawnFactorDeviation;
            //var min = averageSpawnTime - .5f * deviation;
            //var changed = spawnCurve.Evaluate((count % animationCurveLength) / animationCurveLength) * deviation;

            var waitNow = minSpawnTime + spawnCurve.Evaluate((count % animationCurveLength) / animationCurveLength) * deviation;
            //var waitNow = (averageSpawnTime * spawnFactorDeviation) + 
            //    spawnCurve.Evaluate((count % animationCurveLength) / animationCurveLength) * 
            //    (averageSpawnTime * 2 * (1- spawnFactorDeviation));
            return waitNow;
        }

        IEnumerator ContinousSpawn()
        {

            var count = 0;
            Transform target = NextTarget;
            while (true) {

                if (shouldSpawn)
                {
                    int startIndex = 0;
                    if (shootFromRandomSources)
                    {
                        startIndex = numGenerator.Next(shooters.Length);
                    }
                    for (int s = 0; s < shooters.Length; ++s)
                    {
                        var shooter = shooters[(s+ startIndex)% shooters.Length];
                        if (shooter.CanShoot && IsActive(shooter.trans, target) && IsInRange(shooter.trans, target))
                        {
                            var go = Spawn(shooter.trans, target);
                            shooter.Shoot(go, target);
                            yield return new WaitForSeconds(GetWaitTime(count++));
                            target = NextTarget;

                        }
                    }
                }
                yield return new WaitForSeconds(GetWaitTime(count++));
            }
        }

        private GameObject Spawn(Transform spawner, Transform target)
        {
            var newGO = Instantiate(toInstantiate, spawner.position, spawner.rotation);

            newGO.transform.SetParent(bulletParent == null? spawner : bulletParent);
            if (destroyAfter > 0)
            {
                Destroy(newGO, destroyAfter);
            }
            AfterSpawn(newGO, target);
            return newGO;
        }

        private void AfterSpawn(GameObject go, Transform target)
        {
            var mover = go.GetComponentInChildren<MoveByCurve>();
            var posTarget = target.position;

            float radius = (float)numGenerator.NextDouble() * accuracyInMetersRadius;
            Vector3 dir = new Vector3((float)numGenerator.NextDouble() * 2 - 1,
                (float)numGenerator.NextDouble() * 2 - 1,
                (float)numGenerator.NextDouble() * 2 - 1);//Not fullyRandom
            posTarget += dir.normalized * radius;

            mover.meterPerSec = meterPerSec;
            mover.target = posTarget;
        }

    }
}