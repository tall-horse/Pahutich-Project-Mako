using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Mako
{
    public class CollectableAnimationController : MonoBehaviour
    {
        [SerializeField] private Vector3 _rotationVector = new Vector3(0f, 360f, 0f);
        [SerializeField] private float _rotationSpeed = 1f;
        // Start is called before the first frame update
        void Start()
        {
            transform.DORotate(_rotationVector, _rotationSpeed, RotateMode.WorldAxisAdd).SetLoops(-1).SetEase(Ease.Linear);
        }
    }
}
