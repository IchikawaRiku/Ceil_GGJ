using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���C���[�̏��������ʒu�����߂�
/// </summary>
public class StartObject : StageObject {

    public override void Setup() {
        base.Setup();
        CreatePlayer();
    }

    /// <summary>
    /// �v���C���[�����g�̈ʒu�Ɉړ�
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
