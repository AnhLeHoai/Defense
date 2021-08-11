//using UnityEngine;

//namespace DiePosers
//{
//    public class GunnerDiePoser : RagdollDiePoser
//    {
//        [SerializeField] private Collider head;
//        [SerializeField] private ParticleSystem bloodSplashFromHeadFx;

//        protected override void Initialize(Character killer, Character decedent)
//        {
//            base.Initialize(killer, decedent);
//            SetBloodSplashPosition(killer.transform);
//        }

//        private void SetBloodSplashPosition(Transform killerTransform)
//        {
//            Vector3 headHitPosition = head.ClosestPoint(killerTransform.position);
//            bloodSplashFromHeadFx.transform.position = headHitPosition;
//            bloodSplashFromHeadFx.transform.rotation = Quaternion.LookRotation(-killerTransform.forward);
//        }
//    }
//}