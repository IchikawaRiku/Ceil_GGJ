using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gimmick_Break : GimmickBase, IDestroyable {

    /// <summary>
    /// 初期化
    /// </summary>
    public override void Initialize() {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 準備
    /// </summary>
    public override void SetUp() {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    protected override void OnUpdate() {
    }



    /// <summary>
    /// オブジェクトを非アクティブにする
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    public void DestroyGimmick() {
        // 消す
        gameObject.SetActive(false);
    }

}
