/*
 *  @file   goalObject.cs
 *  @brief  ゴールオブジェクト
 *  @author oorui
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static MainGameProcessor;
public class goalObject : GimmickBase {
    // ゴールしたかどうか
    public bool isGoal { get; private set; } = false;

    /// <summary>
    /// 準備
    /// </summary>
    public override void SetUp() {
        base.SetUp();
        isGoal = false;
    }


    /// <summary>
    /// 更新処理
    /// </summary>
    protected override void OnUpdate() {
    }

    /// <summary>
    /// 触れたとき
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter(Collider collision) {
        if (collision.gameObject.layer == 6) {
            isGoal = true;
            UniTask task = SoundManager.instance.PlaySE(7);
            EffectManager.Instance.Play(EffectID._GOAL, transform.position);
            EndGameReason(eEndReason.Clear);

        }
    }
}
