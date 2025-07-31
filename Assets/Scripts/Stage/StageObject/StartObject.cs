using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの初期生成位置を決める
/// </summary>
public class StartObject : GimmickBase {

    public override void SetUp() {
        base.SetUp();
        CreatePlayer();
    }

    protected override void OnUpdate() {
    }

    /// <summary>
    /// プレイヤーを自身の位置に移動
    /// </summary>
    private void CreatePlayer() {
        CharacterUtility.UsePlayer();
        CharacterManager.instance.SetPlayerPosition(transform.position);
    }

}
