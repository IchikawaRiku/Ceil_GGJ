/*
 *  @file   UserDataManager.cs
 *  @brief  ���[�U�[�f�[�^�̊Ǘ�
 *  @author Seki
 *  @date   2025/7/30
 */
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UserDataManager : SystemObject {
    public static UserDataManager instance { get; private set; } = null;
    public static UserData userData { get; private set; } = null;
    private string _userFileName = "/GGJ.U";
    private string _filePath = null;

    public override async UniTask Initialize() {
        instance = this;
        //�Z�[�u�f�[�^�̃��[�h
        userData = LoadDataFromFile();
        //StringBuilder�̐錾
        StringBuilder fileNameBuilder = new StringBuilder();
        fileNameBuilder.Append(Application.persistentDataPath);
        fileNameBuilder.Append(_userFileName);
        //�A�������t�@�C���p�X��n��
        _filePath = fileNameBuilder.ToString();
        await UniTask.CompletedTask;
    }
    /// <summary>
    /// Unity���I���������ɌĂяo����鏈��
    /// </summary>
    private void OnApplicationQuit() {
        //�Z�[�u
        SaveUserData();
    }
    /// <summary>
    /// ���[�U�[�f�[�^�̃Z�[�u
    /// </summary>
    public void SaveUserData() {
        UserDataToFile(userData);
    }
    /// <summary>
    /// ���[�U�[�f�[�^�̃��[�h
    /// </summary>
    public void LoadUserData() {
        userData = LoadDataFromFile();
    }
    /// <summary>
    /// �Z�[�u�f�[�^���t�@�C���ɓn��
    /// </summary>
    /// <param name="setData"></param>
    private void UserDataToFile(UserData setData) {
        //FileSteam�̐錾
        FileStream fileStream = new FileStream(_filePath, FileMode.Create);
        //BinaryFormatter�̐錾
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fileStream, setData);
        //�t�@�C�������
        fileStream.Close();
    }
    /// <summary>
    /// �t�@�C���̒��g���Z�[�u�f�[�^�ɓn��
    /// </summary>
    /// <returns></returns>
    private UserData LoadDataFromFile() {
        if(File.Exists(_filePath)) {
            //FileStream�̐錾
            FileStream fileStream = new FileStream(_filePath, FileMode.Open);
            //BinaryFormatter�̐錾
            BinaryFormatter bf = new BinaryFormatter();
            UserData data = (UserData)bf.Deserialize(fileStream);
            //�t�@�C�������
            fileStream.Close();
            return data;
        } else {
            return new UserData();
        }
    }
    /// <summary>
    /// BGM���ʃf�[�^�̐ݒ�
    /// </summary>
    /// <param name="setValue"></param>
    public void SetBGMVolumeData(float setValue) {
        userData.bgmVolume = setValue;
    }
    /// <summary>
    /// SE���ʃf�[�^�̐ݒ�
    /// </summary>
    /// <param name="setValue"></param>
    public void SetSEVolumeData(float setValue) {
        userData.seVolume = setValue;
    }
}
