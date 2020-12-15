using UnityEngine;
using System.Collections;
using Dweiss;

namespace Common
{
    public class TrackTransform : MonoBehaviour
    {

        public enum Stick
        {
            StickToMe,
            StickToHim,
            Nothing
        }
        public Stick followRotate = Stick.StickToMe;
        public Stick followPosition = Stick.StickToMe;

        public Vector3 posFactor = new Vector3(1, 1, 1);
        public Vector4 rotFactor = new Vector4(1, 1, 1, 1);



        public bool useLocal = false;

        public Transform other;
        public Vector3 shift;
        //public Quaternion rotShift;

        public void StickNow()
        {
            StickNow(followRotate, followPosition);
        }

        private void Start2()
        {
            if(followPosition == Stick.StickToMe) shift = transform.position - other.position;
            else shift = other.position - transform.position;
        }

        //public void StickWithShift(Transform other)
        //{
        //    if (followRotate == Stick.StickToMe)
        //    {
        //        this.shift = other.position - transform.position;
        //        this.rotShift = Quaternion.Inverse(other.rotation) * transform.rotation;
        //    } else
        //    {
        //        this.shift = transform.position - other.position;
        //        this.rotShift = Quaternion.Inverse(transform.rotation) * other.rotation;
        //    }
        //    this.other = other;
        //    StickNow(followRotate, followPosition);
        //}

        public void StickNow(Stick r, Stick p)
        {

            if (useLocal == false)
            {

                switch (p)
                {
                    case Stick.StickToMe:
                        other.position = (transform.position + transform.rotation * shift).CombineByFactor(other.position, posFactor);
                        break;
                    case Stick.StickToHim:
                        transform.position = (other.position - other.rotation.Relative(transform.rotation) * shift).CombineByFactor(transform.position, posFactor);
                        //transform.position = other.position + shift;
                        break;
                    case Stick.Nothing:
                        break;
                    default:
                        throw new System.ArgumentOutOfRangeException("followPosition ins't supported " + followPosition);
                }
                switch (r)
                {
                    case Stick.StickToMe:
                        other.rotation = transform.rotation.CombineByFactor(other.rotation, rotFactor);
                        break;
                    case Stick.StickToHim:
                        transform.rotation = other.rotation.CombineByFactor(transform.rotation, rotFactor);
                        break;
                    case Stick.Nothing:
                        break;
                    default:
                        throw new System.ArgumentOutOfRangeException("Follow rotate ins't supported " + followRotate);
                }
            }
            else
            {

                switch (followPosition)
                {
                    case Stick.StickToMe:
                        other.localPosition = (transform.localPosition + transform.localRotation * shift).CombineByFactor(other.localPosition, posFactor);
                        break;
                    case Stick.StickToHim:
                        transform.localPosition = (other.localPosition - other.localRotation * shift).CombineByFactor(transform.localPosition, posFactor);
                        break;
                    case Stick.Nothing:
                        break;
                    default:
                        throw new System.ArgumentOutOfRangeException("followPosition ins't supported " + followPosition);
                }
                switch (followRotate)
                {
                    case Stick.StickToMe:
                        other.localRotation = transform.localRotation.CombineByFactor(other.localRotation, rotFactor);
                        break;
                    case Stick.StickToHim:
                        transform.localRotation = other.localRotation.CombineByFactor(transform.localRotation, rotFactor);
                        break;
                    case Stick.Nothing:
                        break;
                    default:
                        throw new System.ArgumentOutOfRangeException("Follow rotate ins't supported " + followRotate);
                }
            }

        }

        void Update()
        {
            StickNow();
        }
        //void LateUpdate()
        //{
        //    StickNow();
        //}
        //	void FixedUpdate () {
        //		StickNow ();
        //	}
        //	void OnGUI () {
        //		StickNow ();
        //	}
        //	void LateFixedUpdate () {
        //		StickNow ();
        //	}
    }
}