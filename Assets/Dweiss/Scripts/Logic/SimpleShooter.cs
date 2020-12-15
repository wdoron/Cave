using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Dweiss
{
    public class SimpleShooter : MonoBehaviour, Shooter.IShooter
    {
        public bool canShoot = true;

        public Transform trans { get; private set; }


        public bool CanShoot { get { return canShoot; } }

        private void Awake()
        {
            trans = transform;
        }


        public void Shoot(GameObject bullet, Transform target) {

        }
    }
}