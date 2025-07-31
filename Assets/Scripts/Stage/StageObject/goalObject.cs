using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goalObject : GimmickBase {
    // �S�[���������ǂ���
    public bool isGoal { get; private set; } = false;

    /// <summary>
    /// ����
    /// </summary>
    public override void SetUp() {
        base.SetUp();
        isGoal = false;
    }


    /// <summary>
    /// �X�V����
    /// </summary>
    protected override void OnUpdate() {
    }


    private void OnTriggerEnter(Collider collision) {
        if (collision.gameObject.layer == 6) {
            isGoal = true;
            // �t�F�[�h����H
            UniTask task = PartManager.instance.TransitionPart(eGamePart.Title);
            //GameManager.instance.StageChange();
        }
    }
}
