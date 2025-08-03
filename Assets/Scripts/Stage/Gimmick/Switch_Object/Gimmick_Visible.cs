using System.Collections;
using UnityEngine;

/// <summary>
/// スイッチなどで可視状態を切り替えるギミック（不透明→透明にフェード）
/// </summary>
[RequireComponent(typeof(Renderer))]
public class Gimmick_Visible : GimmickBase, IVisibleToggleable {
    [Header("フェード設定")]
    [Tooltip("透明になるまでのフェード時間（秒）")]
    [SerializeField] private float fadeDuration = 1f;

    private Renderer _renderer;
    private Material _material;
    private Coroutine _fadeCoroutine;

    private const float Transparent = 0f;
    private const float Opaque = 1f;

    /// <summary>
    /// 初期化時にRendererとMaterialを取得。不透明状態に設定。
    /// </summary>
    public override void SetUp() {
        _renderer = GetComponent<Renderer>();
        _material = _renderer.material;

        SetMaterialToTransparent(_material);

        SetAlpha(Transparent); // ← 初期状態を「透明」に変更！
    }

    /// <summary>
    /// 外部（スイッチなど）から呼ばれて、透明にフェードする。
    /// </summary>
    public void ToggleVisibility() {
        if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
        _fadeCoroutine = StartCoroutine(FadeToAlpha(Opaque)); // ← 切り替えを「不透明に」！
    }

    /// <summary>
    /// 指定したアルファ値まで徐々にフェードする処理。
    /// </summary>
    private IEnumerator FadeToAlpha(float targetAlpha) {
        if (!_material.HasProperty("_Color")) yield break;

        float time = 0f;
        Color color = _material.color;
        float startAlpha = color.a;

        while (time < fadeDuration) {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            color.a = alpha;
            _material.color = color;
            yield return null;
        }

        // 最終的な値をしっかり設定
        color.a = targetAlpha;
        _material.color = color;
    }

    /// <summary>
    /// アルファ値を即座に設定（初期化用）
    /// </summary>
    private void SetAlpha(float alpha) {
        if (_material.HasProperty("_Color")) {
            Color color = _material.color;
            color.a = alpha;
            _material.color = color;
        }
    }

    /// <summary>
    /// マテリアルが透明描画できるように設定変更（Standard Shader想定）
    /// </summary>
    private void SetMaterialToTransparent(Material mat) {
        if (mat == null) return;

        mat.SetFloat("_Mode", 3); // Transparent
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
    }

    /// <summary>
    /// 継続的な更新は不要
    /// </summary>
    protected override void OnUpdate() { }
}