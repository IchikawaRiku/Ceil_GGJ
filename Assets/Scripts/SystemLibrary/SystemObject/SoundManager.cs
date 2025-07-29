/*
 *  @file   SoundManager.cs
 *  @brief  サウンドの管理
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
    //BGM再生用コンポーネント
    [SerializeField]
    private AudioSource _bgmAudioSource = null;
    //SE再生用コンポーネント
    [SerializeField]
    private AudioSource[] _seAudioSource = null;
    //BGMのリスト
    [SerializeField]
    private BGMAssign _bgmAssign = null;
    //SEのリスト
    [SerializeField]
    private SEAssign _seAssign = null;

    //入力受付タスク中断用トークン
    private CancellationToken _token;

    public override async UniTask Initialize() {
        instance = this;
        await UniTask.CompletedTask;
    }
    /// <summary>
    /// BGM再生
    /// </summary>
    /// <param name="bgmID"></param>
    public void PlayBGM(int bgmID) {
        if(!IsEnableIndex(_bgmAssign.bgmArray, bgmID)) return;

        _bgmAudioSource.clip = _bgmAssign.bgmArray[bgmID];
        _bgmAudioSource.Play();
    }
    /// <summary>
    /// BGM停止
    /// </summary>
    public void StopBGM() {
        _bgmAudioSource.Stop();
    }
    /// <summary>
    /// BGMの音量設定
    /// </summary>
    /// <param name="setValue"></param>
    public void SetBGMVolume(int setValue) {
        _bgmAudioSource.volume = setValue / 1.0f;
    }
    /// <summary>
    /// SEの再生
    /// </summary>
    /// <param name="seID"></param>
    /// <returns></returns>
    public async UniTask PlaySE(int seID) {
        _token = this.GetCancellationTokenOnDestroy();
        if(!IsEnableIndex(_seAssign.seArray, seID)) return;
        //再生中でないオーディオソースを探してそれを再生
        for (int i = 0, max = _seAudioSource.Length; i < max; i++) {
            if(_seAudioSource[i] == null || _seAudioSource[i].isPlaying) continue;
            //再生中でないオーディオソースが見つかったら再生
            _seAudioSource[i].clip = _seAssign.seArray[seID];
            _seAudioSource[i].Play();
            //SEの終了待ち
            while(_seAudioSource[i].isPlaying) await UniTask.DelayFrame(1, PlayerLoopTiming.Update, _token);

            return;
        }
    }
    /// <summary>
    /// SEの音量設定
    /// </summary>
    /// <param name="setValue"></param>
    public void SetSEVolume(int setValue) {
        for (int i = 0, max = _seAudioSource.Length; i < max; i++) {
            _seAudioSource[i].volume = setValue / 1.0f;
        }
    }
}
