/*
 *  @file   MenuSetting.cs
 *  @brief  設定メニュー
 *  @author Seki
 *  @date   2025/7/29
 */
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuSetting : MenuBase {
    //BGM音量のテキスト
    [SerializeField]
    private TextMeshProUGUI _bgmVolumeText = null;
    //SE音量のテキスト
    [SerializeField]
    private TextMeshProUGUI _seVolumeText = null;
    //メニューを閉じるか判別するフラグ
    private bool _isClose = false;
    //BGMの音量データ
    private float _bgmVolumeData = -1;
    //SEの音量データ
    private float _seVolumeData = -1;

    //10段階の音調調整用定数
    private const float _DEVIDE_TEN_VOLUME = 0.1f;

    public override async UniTask Initialize() {
        await base.Initialize();
        _isClose = false;
        SetupData();
    }

    private void SetupData() {
        UserData userData = UserDataManager.userData;
        _bgmVolumeData = userData.bgmVolume;
        _seVolumeData = userData.seVolume;
    }
    public override async UniTask Open() {
        await base.Open();
        await FadeManager.instance.FadeIn();
        while (true) { 
            if(_isClose || Input.GetKeyDown(KeyCode.Escape)) break;

            await UniTask.DelayFrame(1);
        }
        await FadeManager.instance.FadeOut();
        await Close();
    }
    public override async UniTask Close() {
        await base.Close();
        _isClose = false;
    }
    /// <summary>
    /// メニュー開閉フラグの変更
    /// </summary>
    public void MenuClose() {
        _isClose = true;
    }
    /// <summary>
    /// BGM音量を上げる
    /// </summary>
    public void AddBGMVolume() {
        _bgmVolumeData += _DEVIDE_TEN_VOLUME;
        SetBGMVolumeData(_bgmVolumeData);
    }
    /// <summary>
    /// BGM音量を下げる
    /// </summary>
    public void SubBGMVolume() {
        _bgmVolumeData -= _DEVIDE_TEN_VOLUME;
        SetBGMVolumeData(_bgmVolumeData);
    }
    /// <summary>
    /// SE音量を上げる
    /// </summary>
    public void AddSEVolume() {
        _seVolumeData += _DEVIDE_TEN_VOLUME;
        SetSEVolumeData(_seVolumeData);
    }

    public void SubSEVolume() {
        _seVolumeData -= _DEVIDE_TEN_VOLUME;
        SetSEVolumeData(_seVolumeData);
    }
    /// <summary>
    /// BGM音量データの設定
    /// </summary>
    /// <param name="setValue"></param>
    private void SetBGMVolumeData(float setValue) {
        SoundManager.instance.SetBGMVolume(setValue);
        UserDataManager.instance.SetBGMVolumeData(setValue);
    }
    /// <summary>
    /// SE音量データの設定
    /// </summary>
    /// <param name="setValue"></param>
    private void SetSEVolumeData(float setValue) {
        SoundManager.instance.SetSEVolume(setValue);
        UserDataManager.instance.SetSEVolumeData(setValue);
    }
}
