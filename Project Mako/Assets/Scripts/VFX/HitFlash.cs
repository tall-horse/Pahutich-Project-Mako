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
        /* ------------------------------------------------------------------ */
        /*  Inspector fields                                                   */
        /* ------------------------------------------------------------------ */

        [Header("Setup")]
        [SerializeField] private Health _health;          // optional – will be auto‑found if null
        [SerializeField] private Renderer[] _renderers;   // optional – will be auto‑filled

        [Header("Flash")]
        [SerializeField] private bool _enabled = true;
        [SerializeField] private Color _flashColor = Color.white;
        [SerializeField] private int _flashCount = 3;     // number of flashes (Yoyo → *2 steps)
        [SerializeField] private float _totalDuration = 0.2f;

        /* ------------------------------------------------------------------ */
        /*  Internal data                                                      */
        /* ------------------------------------------------------------------ */

        /// <summary>Information for a single material that has a colour channel.</summary>
        private struct MatInfo
        {
            public Material mat;
            public int colorPropId;   // -1 if no colour property found
            public Color baseColor;     // original colour to tween from
        }

        /// <summary>All materials that will be animated.</summary>
        private readonly System.Collections.Generic.List<MatInfo> _matInfos = new();

        /* ------------------------------------------------------------------ */
        /*  Unity callbacks                                                    */
        /* ------------------------------------------------------------------ */

        private void Awake()
        {
            // --------------------------------------------------------------
            // 1️⃣ Auto‑find Health if you didn’t assign it.
            // --------------------------------------------------------------
            if (_health == null)
                _health = GetComponent<Health>();

            // --------------------------------------------------------------
            // 2️⃣ Auto‑fill the renderer array (all Renderers in this GameObject & children).
            // --------------------------------------------------------------
            if (_renderers == null || _renderers.Length == 0)
            {
                var rendList = new System.Collections.Generic.List<Renderer>();
                rendList.AddRange(GetComponentsInChildren<Renderer>(true));
                _renderers = rendList.ToArray();
            }
            else
            {
                // Remove any accidental null entries.
                var nonNull = new System.Collections.Generic.List<Renderer>();
                foreach (var r in _renderers)
                    if (r != null) nonNull.Add(r);
                _renderers = nonNull.ToArray();
            }

            // --------------------------------------------------------------
            // 3️⃣ Build the material‑info list.
            // --------------------------------------------------------------
            foreach (var rend in _renderers)
            {
                if (rend == null) continue;

                var mats = rend.materials;   // creates *instances* for all sub‑materials
                foreach (var mat in mats)
                {
                    int propId = -1;

                    // Find the first Color property in this shader.
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

        /* ------------------------------------------------------------------ */
        /*  Event handler                                                      */
        /* ------------------------------------------------------------------ */

        private void OnHit()
        {
            if (!_enabled) return;
            Debug.Log("hit");
            int cycles = _flashCount * 2;          // Yoyo → two steps per flash
            float halfDur = _totalDuration / cycles; // duration of one step

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
                    info.baseColor,   // start from the original colour
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
