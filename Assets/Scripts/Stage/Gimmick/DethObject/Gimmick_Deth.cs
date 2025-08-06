using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using static MainGameProcessor;
/// <summary>
/// 落下時の死亡処理
/// </summary>
public class Gimmick_Deth : GimmickBase {

    /// <summary>
    /// 準備
    /// </summary>
    public override void SetUp() {
        base.SetUp();
    }

    /// <summary>
    /// 更新(使わない)
    /// </summary>
    protected override void OnUpdate() {
    }


    /// <summary>
    /// 死亡判定
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter(Collider collision) {
        // プレイヤーだったら
        if (collision.gameObject.layer == 6) {
            // SE再生
            UniTask task = SoundManager.instance.PlaySE(6);
            // 死亡判定をMainGameProcessorに送る
            EndGameReason(eEndReason.Dead);
        }

        // 押せる箱（PushBox）だったらリセット
        var pushBox = collision.GetComponent<Gimmick_PushBox>();
        if (pushBox != null) {
            pushBox.ResetBox();
        }

    }

}
