using DG.Tweening;
using Mako.Movement;
using Mako.Shooting;
using UnityEngine;

namespace Mako.Collectables
{
    public class WeaponCooler : MonoBehaviour, ICollectable
    {
        [Header("Cooling")]
        [SerializeField] private int _amountToCool = 15;

        [Header("Magnet effect")]
        [SerializeField] private float _collectDuration = 0.5f;
        [SerializeField] private Ease _collectEase = Ease.InOutQuad;
        [SerializeField] private Vector3 _scaleToZero = Vector3.zero;

        private MeshRenderer _meshRenderer;
        private Collider _hitBox;
        private AudioSource _audioSource;
        private Shooter _playerGun;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _hitBox = GetComponent<Collider>();
            _audioSource = GetComponent<AudioSource>();

            var pc = FindObjectOfType<PlayerController>();
            if (pc != null)
                _playerGun = pc.GetComponentInChildren<Shooter>();
        }

        public void Collect()
        {
            if (_playerGun == null) return;

            _playerGun.currentOverheat -= _amountToCool;

            _audioSource?.Play();

            _meshRenderer.enabled = false;
            _hitBox.enabled = false;

            transform.DOKill();

            Sequence seq = DOTween.Sequence();

            seq.Append(transform.DOMove(_playerGun.transform.position,
                                        _collectDuration).SetEase(_collectEase));

            seq.Join(transform.DOScale(_scaleToZero, _collectDuration / 2f)
                           .SetEase(Ease.InQuad));

            seq.OnComplete(() => Destroy(gameObject));
        }

        private void OnTriggerEnter(Collider other)
        {
            var encounteredGunOwner = other.GetComponentInChildren<Shooter>();
            if (encounteredGunOwner == _playerGun)
                Collect();
        }
    }
}
