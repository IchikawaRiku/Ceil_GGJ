using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gimmick_Break : GimmickBase, IDestroyable {

    /// <summary>
    /// ������
    /// </summary>
    public override void Initialize() {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// ����
    /// </summary>
    public override void SetUp() {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// �X�V����
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    protected override void OnUpdate() {
    }



    /// <summary>
    /// �I�u�W�F�N�g���A�N�e�B�u�ɂ���
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    public void DestroyGimmick() {
        // ����
        gameObject.SetActive(false);
    }

}
