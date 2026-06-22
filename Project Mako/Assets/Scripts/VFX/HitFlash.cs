using Mako.HealthNamespace;
using PrimeTween;
using UnityEngine;

namespace Mako
{
    public class HitFlash : MonoBehaviour
    {
        private static readonly int _baseColorId = Shader.PropertyToID("_BaseColor");

        [Header("Setup")]
        [SerializeField] private Health _health;
        [SerializeField] private Renderer[] _renderers;

        [Header("Flash")]
        [SerializeField] private bool _enabled;
        [SerializeField] private Color _flashColor = Color.white;
        [SerializeField] private int _flashCount = 3;
        [SerializeField] private float _totalDuration = 0.2f;

        private Material[] _materials;
        private Color[] _baseColors;

        private void Awake()
        {
            _materials = new Material[_renderers.Length];
            _baseColors = new Color[_renderers.Length];

            for (int i = 0; i < _renderers.Length; i++)
            {
                _materials[i] = _renderers[i].material;
                _baseColors[i] = _materials[i].GetColor(_baseColorId);
            }
        }

        private void OnEnable()
        {
            _health.OnGotDamaged += OnHit;
            _health.OnDead += OnHit;
        }

        private void OnDisable()
        {
            _health.OnGotDamaged -= OnHit;
            _health.OnDead -= OnHit;
        }

        private void OnHit()
        {
            if (_enabled == false)
                return;

            int cycles = _flashCount * 2;
            float halfDuration = _totalDuration / cycles;

            for (int i = 0; i < _materials.Length; i++)
            {
                Material material = _materials[i];
                Color baseColor = _baseColors[i];

                Tween.StopAll(onTarget: material);

                Tween.MaterialColor(material, _baseColorId, baseColor, _flashColor, halfDuration,
                    Ease.Linear, cycles, CycleMode.Yoyo, useUnscaledTime: true);
            }
        }
    }
}
