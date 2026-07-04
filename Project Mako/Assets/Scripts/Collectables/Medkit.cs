using DG.Tweening;
using Mako.Movement;
using UnityEngine;

namespace Mako.Collectables
{
    public class Medkit : MonoBehaviour, ICollectable
    {
        [Header("Healing")]
        [SerializeField] private int _amountToHeal = 25;

        [Header("Magnet effect")]
        [SerializeField] private float _collectDuration = 0.5f;
        [SerializeField] private Ease _collectEase = Ease.InOutQuad;
        [SerializeField] private Vector3 _scaleToZero = Vector3.zero;

        private MeshRenderer _meshRenderer;
        private Collider _hitBox;
        private AudioSource _audioSource;
        private HealthNamespace.Health _playerHealth;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _hitBox = GetComponent<Collider>();
            _audioSource = GetComponent<AudioSource>();
            var pc = FindObjectOfType<PlayerController>();
            if (pc != null)
                _playerHealth = pc.GetComponent<HealthNamespace.Health>();
        }
        public void Collect()
        {
            if (_playerHealth == null) return;

            _playerHealth.Heal(_amountToHeal);

            _audioSource?.Play();

            _meshRenderer.enabled = false;
            _hitBox.enabled = false;

            transform.DOKill();

            Sequence seq = DOTween.Sequence();

            seq.Append(transform.DOMove(_playerHealth.transform.position,
                                        _collectDuration).SetEase(_collectEase));

            seq.Join(transform.DOScale(_scaleToZero, _collectDuration / 2f)
                           .SetEase(Ease.InQuad));

            seq.OnComplete(() => Destroy(gameObject));
        }

        private void OnTriggerEnter(Collider other)
        {
            var health = other.GetComponent<HealthNamespace.Health>();
            if (health == _playerHealth)
                Collect();
        }
    }
}
