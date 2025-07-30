/*
 *  @file   UserDataManager.cs
 *  @brief  ユーザーデータの管理
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
        //セーブデータのロード
        userData = LoadDataFromFile();
        //StringBuilderの宣言
        StringBuilder fileNameBuilder = new StringBuilder();
        fileNameBuilder.Append(Application.persistentDataPath);
        fileNameBuilder.Append(_userFileName);
        //連結したファイルパスを渡す
        _filePath = fileNameBuilder.ToString();
        await UniTask.CompletedTask;
    }
    /// <summary>
    /// Unityが終了した時に呼び出される処理
    /// </summary>
    private void OnApplicationQuit() {
        //セーブ
        SaveUserData();
    }
    /// <summary>
    /// ユーザーデータのセーブ
    /// </summary>
    public void SaveUserData() {
        UserDataToFile(userData);
    }
    /// <summary>
    /// ユーザーデータのロード
    /// </summary>
    public void LoadUserData() {
        userData = LoadDataFromFile();
    }
    /// <summary>
    /// セーブデータをファイルに渡す
    /// </summary>
    /// <param name="setData"></param>
    private void UserDataToFile(UserData setData) {
        //FileSteamの宣言
        FileStream fileStream = new FileStream(_filePath, FileMode.Create);
        //BinaryFormatterの宣言
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fileStream, setData);
        //ファイルを閉じる
        fileStream.Close();
    }
    /// <summary>
    /// ファイルの中身をセーブデータに渡す
    /// </summary>
    /// <returns></returns>
    private UserData LoadDataFromFile() {
        if(File.Exists(_filePath)) {
            //FileStreamの宣言
            FileStream fileStream = new FileStream(_filePath, FileMode.Open);
            //BinaryFormatterの宣言
            BinaryFormatter bf = new BinaryFormatter();
            UserData data = (UserData)bf.Deserialize(fileStream);
            //ファイルを閉じる
            fileStream.Close();
            return data;
        } else {
            return new UserData();
        }
    }
    /// <summary>
    /// BGM音量データの設定
    /// </summary>
    /// <param name="setValue"></param>
    public void SetBGMVolumeData(float setValue) {
        userData.bgmVolume = setValue;
    }
    /// <summary>
    /// SE音量データの設定
    /// </summary>
    /// <param name="setValue"></param>
    public void SetSEVolumeData(float setValue) {
        userData.seVolume = setValue;
    }
}
