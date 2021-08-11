//using System;
//using DG.Tweening;
//using Sirenix.OdinInspector;
//using UnityEngine;

//namespace DiePosers
//{
//    public class DiePoser : MonoBehaviour
//    {
//        [SerializeField] private Animator animator;
//        [SerializeField] private SkinCharaterController[] skinCharacterControllers;
//        [SerializeField] private BloodController[] bloodControllers;
//        [SerializeField] private bool isFaceTowardsKiller;
//        [SerializeField] private bool isHatUseGravity;

//        private Character decendent;
//        private Joint[] joints;
//        private Vector3[] connectedAnchor;
//        private Vector3[] anchor;

//        public static DiePoser SpawnDiePoser(Character killer, Character decedent)
//        {
//            DiePoser diePoser;

//            if (killer.DataRole.DiePoser == null)
//            {
//                diePoser = Instantiate(PrefabStorage.Instance.DefaultDiePoser, decedent.transform.position, decedent.transform.rotation);
//            }
//            else
//            {
//                diePoser = Instantiate(killer.DataRole.DiePoser, decedent.transform.position, decedent.transform.rotation);
//            }

//            diePoser.decendent = decedent;
//            diePoser.transform.localScale = Vector3.one * decedent.ScaleRatio;

//            foreach (var skinCharacterController in diePoser.skinCharacterControllers)
//            {
//                skinCharacterController.Character = decedent;
//                skinCharacterController.CopyFrom(decedent.SkinCharater);
//            }

//            foreach (BloodController bloodController in diePoser.bloodControllers)
//            {
//                bloodController.character = decedent;
//                bloodController.SplashBlood();
//            }

//            if (diePoser.isFaceTowardsKiller)
//            {
//                diePoser.transform.rotation = Quaternion.LookRotation(-killer.transform.forward);
//            }
            
//            diePoser.gameObject.SetActive(true);

//            if (diePoser.animator)
//            {
//                diePoser.animator.Play(CharacterAction.Die.ToAnimatorHashedKey());
//            }

//            decedent.OnRenew += diePoser.OnDecedentRenew;
//            diePoser.Initialize(killer, decedent);

//            Time.timeScale = 1;
//            return diePoser;
//        }

//        void Awake()
//        {
//            joints = transform.GetComponentsInChildren<Joint>();
//            connectedAnchor = new Vector3[joints.Length];
//            anchor = new Vector3[joints.Length];
//            for (int i = 0; i < joints.Length; i++)
//            {
//                connectedAnchor[i] = joints[i].connectedAnchor;
//                anchor[i] = joints[i].anchor;
//                joints[i].autoConfigureConnectedAnchor = false;
//            }
//        }

//        protected virtual void Initialize(Character killer, Character decedent)
//        {
//            for (int i = 0; i < joints.Length; i++)
//            {
//                joints[i].connectedAnchor = connectedAnchor[i];
//                joints[i].anchor = anchor[i];
//            }
//        }
        
//        private void OnDecedentRenew()
//        {
//            Destroy(gameObject);
//        }
        
//        private void OnDestroy()
//        {
//            decendent.OnRenew -= OnDecedentRenew;
//        }
//    }
//}
