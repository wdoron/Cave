using UnityEngine;
using System.Collections;
using Dweiss;



namespace Common
{
    public class RigidbodyFollow : MonoBehaviour
    {
        public Transform other;
        public Vector3 shift;
        public Quaternion shiftRot;
        public Quaternion rotShift;

        public float maxMovePerSec = float.PositiveInfinity;
        
        [SerializeField] private Rigidbody _rg;
        private void Reset()
        {
            _rg = GetComponent<Rigidbody>();
        }

        public void StickWithShift(Transform other)
        {
            shift = transform.position - other.position;
            shiftRot = Quaternion.FromToRotation(other.forward, shift);
            //Debug.LogFormat("StickWithShift: Shift{0} R{1} O{2} Mul{3} ^Mul{4}", 
            //    shift, 
            //    shiftRot.eulerAngles, 
            //    other.rotation.eulerAngles,
            //    (shiftRot*other.rotation* shift)
            //    , (shiftRot * other.forward * shift.magnitude));
            rotShift = transform.rotation.Relative(other.rotation);
            this.other = other;
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            //transform.position = (other.position - other.rotation * shift).CombineByFactor(transform.position, posFactor);
            //transform.position = (other.position - other.rotation * shift).CombineByFactor(transform.position, posFactor);

            var targetPos = other.position + shiftRot * other.forward * shift.magnitude;
            var moveVec = targetPos - _rg.position;
            moveVec = moveVec.normalized * Mathf.Min(moveVec.magnitude, maxMovePerSec * Time.fixedDeltaTime);

            _rg.MovePosition(_rg.position + moveVec);
            _rg.MoveRotation(rotShift * other.rotation);
        }
    }
}