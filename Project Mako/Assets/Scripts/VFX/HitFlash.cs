using Mako.HealthNamespace;
using PrimeTween;
using UnityEngine;

namespace Mako
{
    /// <summary>
    /// Flashes every material of every Renderer (MeshRenderer, SkinnedMeshRenderer, …)
    /// that has a colour property when the attached Health component reports damage or death.
    ///
    /// • Auto‑finds the Health component if you didn’t assign it.
    /// • Auto‑fills the _renderers array with all Renderers in this GameObject and its children.
    /// • Detects *any* Color property (e.g. _Color, _BaseColor, _Tint …).
    /// • If a material has no colour channel, it is simply skipped – you’ll see a warning in the console.
    /// </summary>
    [RequireComponent(typeof(Health))]
    public class HitFlash : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField] private Health _health;
        [SerializeField] private Renderer[] _renderers;

        [Header("Flash")]
        [SerializeField] private bool _enabled = true;
        [SerializeField] private Color _flashColor = Color.white;
        [SerializeField] private int _flashCount = 3;
        [SerializeField] private float _totalDuration = 0.2f;

        private struct MatInfo
        {
            public Material mat;
            public int colorPropId;
            public Color baseColor;
        }
        private readonly System.Collections.Generic.List<MatInfo> _matInfos = new();

        private void Awake()
        {
            if (_health == null)
                _health = GetComponent<Health>();

            if (_renderers == null || _renderers.Length == 0)
            {
                var rendList = new System.Collections.Generic.List<Renderer>();
                rendList.AddRange(GetComponentsInChildren<Renderer>(true));
                _renderers = rendList.ToArray();
            }
            else
            {
                var nonNull = new System.Collections.Generic.List<Renderer>();
                foreach (var r in _renderers)
                    if (r != null) nonNull.Add(r);
                _renderers = nonNull.ToArray();
            }

            foreach (var rend in _renderers)
            {
                if (rend == null) continue;

                var mats = rend.materials;
                foreach (var mat in mats)
                {
                    int propId = -1;

                    for (int p = 0; p < mat.shader.GetPropertyCount(); p++)
                    {
                        if (mat.shader.GetPropertyType(p) == UnityEngine.Rendering.ShaderPropertyType.Color)
                        {
                            propId = Shader.PropertyToID(mat.shader.GetPropertyName(p));
                            break;
                        }
                    }

                    var info = new MatInfo
                    {
                        mat = mat,
                        colorPropId = propId,
                        baseColor = (propId != -1) ? mat.GetColor(propId) : Color.white
                    };

                    _matInfos.Add(info);
                }
            }
        }

        private void OnEnable()
        {
            if (_health != null)
            {
                _health.OnGotDamaged += OnHit;
                _health.OnDead += OnHit;
            }
        }

        private void OnDisable()
        {
            if (_health != null)
            {
                _health.OnGotDamaged -= OnHit;
                _health.OnDead -= OnHit;
            }
        }
        private void OnHit()
        {
            if (!_enabled) return;

            int cycles = _flashCount * 2;
            float halfDur = _totalDuration / cycles;

            foreach (var info in _matInfos)
            {
                if (info.colorPropId == -1)
                {
                    Debug.LogWarning($"[HitFlash] Material '{info.mat.name}' has no colour property – it will not flash.");
                    continue;
                }

                Tween.StopAll(onTarget: info.mat);

                Tween.MaterialColor(
                    info.mat,
                    info.colorPropId,
                    info.baseColor,
                    _flashColor,
                    halfDur,
                    Ease.Linear,
                    cycles,
                    CycleMode.Yoyo,
                    useUnscaledTime: true);
            }
        }
    }
}
