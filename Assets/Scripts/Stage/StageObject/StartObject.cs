using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���C���[�̏��������ʒu�����߂�
/// </summary>
public class StartObject : GimmickBase {

    public override void SetUp() {
        base.SetUp();
        CreatePlayer();
    }

    protected override void OnUpdate() {
    }

    /// <summary>
    /// �v���C���[�����g�̈ʒu�Ɉړ�
    /// </summary>
    private void CreatePlayer() {
        CharacterUtility.UsePlayer();
        CharacterManager.instance.SetPlayerPosition(transform.position);
    }

}
