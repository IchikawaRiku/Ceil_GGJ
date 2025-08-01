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
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static GameConst;

public class MenuSetting : MenuBase {
    // BGM音量のテキスト
    [SerializeField]
    private TextMeshProUGUI _bgmVolumeText = null;
    // SE音量のテキスト
    [SerializeField]
    private TextMeshProUGUI _seVolumeText = null;
    // 最初に選択されるボタン
    [SerializeField]
    private Button _initSelectButton = null;
    // ボタンの入力受付
    private AcceptSettingsButtonInput _acceptButtonInput = null;
    // InputAction
    private MyInput _inputAction = null;
    // メニューを閉じるか判別するフラグ
    private bool _isClose = false;
    // BGMの音量データ
    private float _bgmVolumeData = -1;
    // SEの音量データ
    private float _seVolumeData = -1;

    public override async UniTask Initialize() {
        await base.Initialize();
        _inputAction = MyInputManager.inputAction;
        _acceptButtonInput = new AcceptSettingsButtonInput();
        _isClose = false;
        SetupData();
    }

    private void SetupData() {
        UserData userData = UserDataManager.userData;
        SetBGMVolume(userData.bgmVolume);
        SetSEVolume(userData.seVolume);
        SetBGMVolumeData(_bgmVolumeData);
        SetSEVolumeData(_seVolumeData);
    }
    public override async UniTask Open() {
        await base.Open();
        await FadeManager.instance.FadeIn();
        _inputAction.Player.Pause.Enable();
        await _acceptButtonInput.Setup(_initSelectButton);
        while (true) {
            await _acceptButtonInput.AcceptInput();
            if(_isClose || _inputAction.Player.Pause.WasPressedThisFrame()) break;

            await UniTask.DelayFrame(1);
        }
        _isClose = false;
        _inputAction.Player.Pause.Disable();
        await _acceptButtonInput.Teardown();
        await FadeManager.instance.FadeOut();
        await Close();
    }
    /// <summary>
    /// メニュー開閉フラグの変更
    /// </summary>
    public void MenuClose() {
        _isClose = true;
    }
    /// <summary>
    /// BGM音量の設定
    /// </summary>
    /// <param name="setValue"></param>
    public void SetBGMVolume(float setValue) {
        _bgmVolumeData = Mathf.Clamp(setValue, 0, _DEVIDE_TEN_VOLUME);
        // テキストの表示（10段階の整数にする）
        _bgmVolumeText.text = (_bgmVolumeData).ToString();
    }
    /// <summary>
    /// SE音量の設定
    /// </summary>
    /// <param name="setValue"></param>
    public void SetSEVolume(float setValue) {
        _seVolumeData = Mathf.Clamp(setValue, 0, _DEVIDE_TEN_VOLUME);
        _seVolumeText.text = (_seVolumeData).ToString();
    }
    /// <summary>
    /// BGM音量を上げる
    /// </summary>
    public void AddBGMVolume() {
        _bgmVolumeData++;
        SetBGMVolume(_bgmVolumeData);
        SetBGMVolumeData(_bgmVolumeData);
    }
    /// <summary>
    /// BGM音量を下げる
    /// </summary>
    public void SubBGMVolume() {
        _bgmVolumeData--;
        SetBGMVolume(_bgmVolumeData);
        SetBGMVolumeData(_bgmVolumeData);
    }
    /// <summary>
    /// SE音量を上げる
    /// </summary>
    public void AddSEVolume() {
        _seVolumeData++;
        SetSEVolume(_seVolumeData);
        SetSEVolumeData(_seVolumeData);
    }
    /// <summary>
    /// SE音量を下げる
    /// </summary>
    public void SubSEVolume() {
        _seVolumeData--;
        SetSEVolume(_seVolumeData);
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
