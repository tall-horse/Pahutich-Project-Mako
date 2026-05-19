using UnityEngine;

namespace Mako.Miscellaneous
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset;
        [SerializeField] private float smoothness;

        // Update is called once per frame
        public void Update()
        {
            if (target != null)
                transform.position = Vector3.Lerp(transform.position, target.position + offset, smoothness);
        }
    }
}