using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss.Common
{
    public class Spawner : MonoBehaviour
    {
        public bool allowSpawnShiftInPhase = false;
        public int onStartSpawnAmount = 1;

        public AnimationCurve spawnIntervalCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(0, 1));
        public float interavl;
        public float destroyAfter = 0; 

        public GameObject[] toInstantiate;
        public BoxCollider spawnArea;
        public Transform parent;
        public AAfterSpawn afterSpawn;

        public interface IAfterSpawn
        {
            void AfterSpawn(GameObject go);
        }

        public abstract class AAfterSpawn : MonoBehaviour {
            public abstract void AfterSpawn(GameObject go);
        }

        public enum SpawnOption
        {
            Random,
            Sequence,
            All
        }

        public SpawnOption spawnOption;

        private int lastSpawn = -1;

        private float startTime = 0;
        private void OnEnable()
        {
            for (int i = 0; i < onStartSpawnAmount; i++)
            {
                Spawn();
            }
            StartCoroutine(CoroutineSpawn());
            startTime = Time.time;
            if (allowSpawnShiftInPhase)
            {
                startTime -= new System.Random().NextFloat() * 100;
            }
        }

        private void OnDisable()
        {
            
        }

        private IEnumerator CoroutineSpawn()
        {
            

            while (true)
            {
                yield return new WaitForSeconds(spawnIntervalCurve.Evaluate(Time.time - startTime) * interavl);
                Spawn();
            }
        }


        public List<GameObject> Spawn()
        {
            var ret = new List<GameObject>();
            switch (spawnOption)
            {
                case SpawnOption.All:
                    for (int i = 0; i < toInstantiate.Length; ++i) ret.Add(SpawnIt(toInstantiate[i]));
                    break;
                case SpawnOption.Sequence:
                    lastSpawn = (lastSpawn + 1) % toInstantiate.Length;
                    ret.Add(SpawnIt(toInstantiate[lastSpawn]));
                    break;
                case SpawnOption.Random:
                    lastSpawn = Random.Range(0, toInstantiate.Length);
                    ret.Add(SpawnIt(toInstantiate[lastSpawn]));
                    break;
            }

            return ret;
        }

        GameObject SpawnIt(GameObject go)
        {
            Vector3 rndPosWithin = transform.position;
            if (spawnArea != null)
            {
                rndPosWithin = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                rndPosWithin = spawnArea.bounds.center + spawnArea.bounds.extents.PointMul(rndPosWithin);
            }
            var newGO = Instantiate(go, rndPosWithin, parent == null?transform.rotation : parent.rotation);
            newGO.transform.SetParent(parent);
            if (destroyAfter > 0)
            {
                Destroy(newGO, destroyAfter);
            }
            if(afterSpawn != null) afterSpawn.AfterSpawn(newGO);
            return newGO;
        }

    }
}