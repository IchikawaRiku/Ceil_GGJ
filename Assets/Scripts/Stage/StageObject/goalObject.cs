using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    private void OnTriggerEnter(Collider collision) {
        if (collision.gameObject.layer == 6) {
            isGoal = true;
            // フェードいる？
            UniTask task = PartManager.instance.TransitionPart(eGamePart.Title);
            //GameManager.instance.StageChange();
        }
    }
}
