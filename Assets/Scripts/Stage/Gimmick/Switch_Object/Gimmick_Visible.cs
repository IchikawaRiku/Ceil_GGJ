using System.Collections;
using UnityEngine;

/// <summary>
/// �X�C�b�`�Ȃǂŉ���Ԃ�؂�ւ���M�~�b�N�i�s�����������Ƀt�F�[�h�j
/// </summary>
[RequireComponent(typeof(Renderer))]
public class Gimmick_Visible : GimmickBase, IVisibleToggleable {
    [Header("�t�F�[�h�ݒ�")]
    [Tooltip("�����ɂȂ�܂ł̃t�F�[�h���ԁi�b�j")]
    [SerializeField] private float fadeDuration = 1f;

    private Renderer _renderer;
    private Material _material;
    private Coroutine _fadeCoroutine;

    private const float Transparent = 0f;
    private const float Opaque = 1f;

    /// <summary>
    /// ����������Renderer��Material���擾�B�s������Ԃɐݒ�B
    /// </summary>
    public override void SetUp() {
        _renderer = GetComponent<Renderer>();
        _material = _renderer.material;

        SetMaterialToTransparent(_material);

        SetAlpha(Transparent); // �� ������Ԃ��u�����v�ɕύX�I
    }

    /// <summary>
    /// �O���i�X�C�b�`�Ȃǁj����Ă΂�āA�����Ƀt�F�[�h����B
    /// </summary>
    public void ToggleVisibility() {
        if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
        _fadeCoroutine = StartCoroutine(FadeToAlpha(Opaque)); // �� �؂�ւ����u�s�����Ɂv�I
    }

    /// <summary>
    /// �w�肵���A���t�@�l�܂ŏ��X�Ƀt�F�[�h���鏈���B
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

        // �ŏI�I�Ȓl����������ݒ�
        color.a = targetAlpha;
        _material.color = color;
    }

    /// <summary>
    /// �A���t�@�l�𑦍��ɐݒ�i�������p�j
    /// </summary>
    private void SetAlpha(float alpha) {
        if (_material.HasProperty("_Color")) {
            Color color = _material.color;
            color.a = alpha;
            _material.color = color;
        }
    }

    /// <summary>
    /// �}�e���A���������`��ł���悤�ɐݒ�ύX�iStandard Shader�z��j
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
    /// �p���I�ȍX�V�͕s�v
    /// </summary>
    protected override void OnUpdate() { }
}