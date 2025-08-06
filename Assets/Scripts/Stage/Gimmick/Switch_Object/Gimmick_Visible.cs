using System.Collections;
using UnityEngine;

/// <summary>
/// スイッチなどで可視状態を切り替えるギミック
/// 透明から不透明に変化させる
/// </summary>
[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Collider))]
public class Gimmick_Visible : GimmickBase, IVisibleToggleable {
    [Tooltip("透明になるまでのフェード時間（秒）")]
    [SerializeField] private float fadeDuration = 1f;

    private Renderer _renderer;
    private Material _material;
    private Coroutine _fadeCoroutine;
    private Collider _collider;

    private const float Transparent = 0f;
    private const float Opaque = 1f;

    /// <summary>
    /// 準備
    /// </summary>
    public override void SetUp() {
        _renderer = GetComponent<Renderer>();
        _material = _renderer.material;
        _collider = GetComponent<Collider>();

        SetMaterialToTransparent(_material);
        // 初期状態を透明に設定
        SetAlpha(Transparent);
        _collider.isTrigger = true;
        Debug.Log("SetUp called");
    }

    /// <summary>
    /// 不透明にする
    /// </summary>
    public void ToggleVisibility() {
        if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
        _fadeCoroutine = StartCoroutine(FadeToAlpha(Opaque));
        _collider.isTrigger = false;
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