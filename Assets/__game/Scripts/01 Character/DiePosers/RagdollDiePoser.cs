//using UnityEngine;

//namespace DiePosers
//{
//    public class RagdollDiePoser : DiePoser
//    {
//        public Collider[] hitColliders;
//        public float hitForce;

//        protected override void Initialize(Character killer, Character decedent)
//        {
//            base.Initialize(killer, decedent);
//            foreach (Collider collider in hitColliders)
//            {
//                Vector3 attackPosition = GetHitPosition(killer, collider);
//                collider.attachedRigidbody.AddExplosionForce(hitForce + decedent.Size * 2000, attackPosition, 20);
//                if (collider.CompareTag("Head"))
//                {
//                    collider.attachedRigidbody.AddTorque(hitForce * Vector3.one);
//                }
//            }
//        }

//        private Vector3 GetHitPosition(Character killer, Collider collider)
//        {
//            Vector3 closestPointOnBound = collider.ClosestPointOnBounds(killer.transform.position);
//            return closestPointOnBound;
//        }
//    }
//}