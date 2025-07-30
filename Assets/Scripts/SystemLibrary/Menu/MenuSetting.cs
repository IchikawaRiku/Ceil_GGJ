/*
 *  @file   MenuSetting.cs
 *  @brief  �ݒ胁�j���[
 *  @author Seki
 *  @date   2025/7/29
 */
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuSetting : MenuBase {
    //BGM���ʂ̃e�L�X�g
    [SerializeField]
    private TextMeshProUGUI _bgmVolumeText = null;
    //SE���ʂ̃e�L�X�g
    [SerializeField]
    private TextMeshProUGUI _seVolumeText = null;
    //���j���[����邩���ʂ���t���O
    private bool _isClose = false;
    //BGM�̉��ʃf�[�^
    private float _bgmVolumeData = -1;
    //SE�̉��ʃf�[�^
    private float _seVolumeData = -1;

    //10�i�K�̉��������p�萔
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
    /// ���j���[�J�t���O�̕ύX
    /// </summary>
    public void MenuClose() {
        _isClose = true;
    }
    /// <summary>
    /// BGM���ʂ��グ��
    /// </summary>
    public void AddBGMVolume() {
        _bgmVolumeData += _DEVIDE_TEN_VOLUME;
        SetBGMVolumeData(_bgmVolumeData);
    }
    /// <summary>
    /// BGM���ʂ�������
    /// </summary>
    public void SubBGMVolume() {
        _bgmVolumeData -= _DEVIDE_TEN_VOLUME;
        SetBGMVolumeData(_bgmVolumeData);
    }
    /// <summary>
    /// SE���ʂ��グ��
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
    /// BGM���ʃf�[�^�̐ݒ�
    /// </summary>
    /// <param name="setValue"></param>
    private void SetBGMVolumeData(float setValue) {
        SoundManager.instance.SetBGMVolume(setValue);
        UserDataManager.instance.SetBGMVolumeData(setValue);
    }
    /// <summary>
    /// SE���ʃf�[�^�̐ݒ�
    /// </summary>
    /// <param name="setValue"></param>
    private void SetSEVolumeData(float setValue) {
        SoundManager.instance.SetSEVolume(setValue);
        UserDataManager.instance.SetSEVolumeData(setValue);
    }
}
