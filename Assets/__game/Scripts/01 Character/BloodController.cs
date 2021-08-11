//using DG.Tweening;
//using System.Collections;
//using System.Collections.Generic;
//using System.Runtime.CompilerServices;
//using UnityEngine;

//public class BloodController : MonoBehaviour
//{
//    [SerializeField] public Character character;
//    [SerializeField] private GameObject blood;
//    [SerializeField] private ParticleSystem bloodSplashFX;
//    public float delayBloodPourTime = 0;

//    public void SplashBlood()
//    {
//        StartCoroutine(SpreadBlood());
//        if (bloodSplashFX)
//        {
//            StartCoroutine(SplashBloodManyTimes());
//        }
        
//    }

//    private IEnumerator SpreadBlood()
//    {
//        if (!blood) yield break;
//        blood.transform.localScale = Vector3.zero;
        
//        yield return Yielders.Get(delayBloodPourTime);

//        // RaycastHit hit = new RaycastHit();
//        // foreach (RaycastHit raycastHit in Physics.RaycastAll(blood.transform.position, Vector3.down, 10))
//        // {
//        //     Debug.LogError(raycastHit.collider.name + " " + raycastHit.collider.gameObject.layer);
//        //     if (raycastHit.collider.gameObject.layer == 0)
//        //     {
//        //         hit = raycastHit;
//        //     }
//        // }

//        if (!Physics.Raycast(blood.transform.position, Vector3.down, out var hit, 10, 1 << 0))
//        {
//            yield break;
//        }

//        blood.transform.position = hit.point;
//        blood.transform.rotation = Quaternion.identity;
//        blood.transform.DOScale(new Vector3(1.5f, 1, 1.5f), 1.5f).SetDelay(0.38f)
//            .SetEase(Ease.InOutCubic);
//    }

//    IEnumerator SplashBloodManyTimes()
//    {
//        bloodSplashFX.time = 0.4f;
//        bloodSplashFX.transform.localScale = Vector3.one * (2f);
//        bloodSplashFX.Play();
//        yield return Yielders.Get(0.4f);
//        bloodSplashFX.Play();
//        bloodSplashFX.transform.localScale = Vector3.one * (1f);
//        yield return Yielders.Get(0.3f);
//        bloodSplashFX.Play();
//        bloodSplashFX.time *= 2;
//        bloodSplashFX.transform.localScale = Vector3.one * (0.9f);
//        yield return Yielders.Get(0.3f);
//        bloodSplashFX.Play();
//        bloodSplashFX.transform.localScale = Vector3.one * (0.7f);
//        yield return Yielders.Get(0.2f);
//        bloodSplashFX.Play();
//        yield return Yielders.Get(0.2f);
//        bloodSplashFX.Play();
//    }
//}
