using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの初期生成位置を決める
/// </summary>
public class StartObject : StageObject {

    public override void Setup() {
        base.Setup();
        CreatePlayer();
    }

    /// <summary>
    /// プレイヤーを自身の位置に移動
    /// </summary>
    private void CreatePlayer() {
        CharacterManager.instance.UsePlayer();
    }


    public override Vector3 GetPosition() {
        return position;
    }

    public override Quaternion GetRotation() {
        return rotation;
    }

    public override void SetPosition(Vector3 _position) {
        position = _position;
        transform.position = position;
    }

    public override void SetRotation(Quaternion _rotation) {
        rotation = _rotation;
        transform.rotation = rotation;
    }
}
