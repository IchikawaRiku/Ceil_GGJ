/*
 *  @file   FadeManager.cs
 *  @brief  �t�F�[�h�̊Ǘ�
 *  @author Seki
 *  @date   2025/7/29
 */
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : SystemObject {
    public static FadeManager instance { get; private set; } = null;
    [SerializeField]
    private Image _fadeImage = null;

    private const float _DEFAULT_FADE_DURATION = 1.0f;
    public override async UniTask Initialize() {
        instance = this;
        await UniTask.CompletedTask;
    }
    /// <summary>
    /// �t�F�[�h�A�E�g
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    public async UniTask FadeOut(float duration = _DEFAULT_FADE_DURATION) {
        await FadeTargetAlpha(eFadeState.FadeOut, duration);
    }
    /// <summary>
    /// �t�F�[�h�C��
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    public async UniTask FadeIn(float duration = _DEFAULT_FADE_DURATION) {
        await FadeTargetAlpha(eFadeState.FadeIn, duration);
    }
    /// <summary>
    /// �t�F�[�h�摜���w��̕s�����x�ɕω�
    /// </summary>
    /// <param name="fadeState"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    private async UniTask FadeTargetAlpha(eFadeState fadeState, float duration) {
        _fadeImage.gameObject.SetActive(true);
        float elapsedTime = 0.0f;
        float startAlpha = _fadeImage.color.a;
        Color targetColor = _fadeImage.color;
        while(elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            // �⊮�����s�����x���t�F�[�h�摜�ɐݒ�
            float t = elapsedTime / duration;
            targetColor.a = Mathf.Lerp(startAlpha, (float)fadeState, t);
            _fadeImage.color = targetColor;
            // 1�t���[���҂�
            await UniTask.DelayFrame(1);
        }
        targetColor.a = (int)fadeState;
        _fadeImage.color = targetColor;
        _fadeImage.gameObject.SetActive(false);
    }
}
