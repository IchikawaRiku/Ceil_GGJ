using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �X�e�[�W�̊Ǘ����s���N���X
/// </summary>
public class StageManager : MonoBehaviour {
    // ���g�ւ̎Q��
    public static StageManager instance { get; private set; } = null;
    // �X�e�[�W��Prefab
    [SerializeField]
    private GameObject stagePrefab;

    public void Initialize() {
        instance = this;
        // �X�e�[�W�̏�������
        Instantiate(stagePrefab);
    }


}
