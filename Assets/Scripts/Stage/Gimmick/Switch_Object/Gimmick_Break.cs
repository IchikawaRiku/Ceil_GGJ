/*
 *  @file   Gimmick_Break.cs
 *  @brief  壊れるギミック
 *  @author oorui
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gimmick_Break : GimmickBase, IDestroyable {
    [SerializeField] private GameObject relatedObj = null;

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
        // 連携するオブジェクトが設定されていたら
        if (relatedObj != null) {
            // フラグを渡す先のコンポーネントを取得
            IBreakReceiver receiver = relatedObj.GetComponent<IBreakReceiver>();

            // 連携されていればフラグをTrueにする
            if (receiver != null) {
                // 通知を送る
                receiver.OnBreak();
            }
        }

        var pos = transform.position;
        pos.z -= 2f;
        // エフェクトを再生
        EffectManager.Instance.Play(EffectID._DESTROY, pos);
        // 消す
        gameObject.SetActive(false);
    }

}
