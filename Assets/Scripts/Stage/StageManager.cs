using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステージの管理を行うクラス
/// </summary>
public class StageManager : MonoBehaviour {
    // 自身への参照
    public static StageManager instance { get; private set; } = null;
    // ステージのPrefab
    [SerializeField]
    private GameObject stagePrefab;

    public void Initialize() {
        instance = this;
        // ステージの初期生成
        Instantiate(stagePrefab);
    }


}
