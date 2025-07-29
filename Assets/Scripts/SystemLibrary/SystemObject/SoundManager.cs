/*
 *  @file   SoundManager.cs
 *  @brief  �T�E���h�̊Ǘ�
 *  @author Seki
 *  @date   2025/7/29
 */
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

using static CommonModule;

public class SoundManager : SystemObject {
    public static SoundManager instance { get; private set; } = null;
    //BGM�Đ��p�R���|�[�l���g
    [SerializeField]
    private AudioSource _bgmAudioSource = null;
    //SE�Đ��p�R���|�[�l���g
    [SerializeField]
    private AudioSource[] _seAudioSource = null;
    //BGM�̃��X�g
    [SerializeField]
    private BGMAssign _bgmAssign = null;
    //SE�̃��X�g
    [SerializeField]
    private SEAssign _seAssign = null;

    //���͎�t�^�X�N���f�p�g�[�N��
    private CancellationToken _token;

    public override async UniTask Initialize() {
        instance = this;
        await UniTask.CompletedTask;
    }
    /// <summary>
    /// BGM�Đ�
    /// </summary>
    /// <param name="bgmID"></param>
    public void PlayBGM(int bgmID) {
        if(!IsEnableIndex(_bgmAssign.bgmArray, bgmID)) return;

        _bgmAudioSource.clip = _bgmAssign.bgmArray[bgmID];
        _bgmAudioSource.Play();
    }
    /// <summary>
    /// BGM��~
    /// </summary>
    public void StopBGM() {
        _bgmAudioSource.Stop();
    }
    /// <summary>
    /// SE�̍Đ�
    /// </summary>
    /// <param name="seID"></param>
    /// <returns></returns>
    public async UniTask PlaySE(int seID) {
        _token = this.GetCancellationTokenOnDestroy();
        if(!IsEnableIndex(_seAssign.seArray, seID)) return;
        //�Đ����łȂ��I�[�f�B�I�\�[�X��T���Ă�����Đ�
        for (int i = 0, max = _seAudioSource.Length; i < max; i++) {
            AudioSource audioSource = _seAudioSource[i];
            if(audioSource == null || audioSource.isPlaying) continue;
            //�Đ����łȂ��I�[�f�B�I�\�[�X������������Đ�
            audioSource.clip = _seAssign.seArray[seID];
            audioSource.Play();
            //SE�̏I���҂�
            while(audioSource.isPlaying) await UniTask.DelayFrame(1, PlayerLoopTiming.Update, _token);

            return;
        }
    }
}
