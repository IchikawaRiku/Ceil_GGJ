/*
 *  @file   CameraManager.cs
 *  @brief  �J�����̊Ǘ��N���X
 *  @author Riku
 *  @date   2025/7/30
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : SystemObject {
	// ���g�ւ̎Q��
	public static CameraManager instance { get; private set; } = null;
	// �Ǘ����̃J����
	private Camera _camera = null;
	// �J�����̗���鋗��
	[SerializeField]
	private float _cameraDistance = -10f;
	// �J�����̖��O
	private const string _CAMERA_NAME = "Main Camera";

	/// <summary>
	/// ������
	/// </summary>
	public override async UniTask Initialize() {
		instance = this;
		// �V�[����̃J�������L���b�V��
		_camera = GameObject.Find(_CAMERA_NAME).GetComponent<Camera>();
		await UniTask.DelayFrame(1);
	}

	/// <summary>
	/// �J�����̈ʒu���Z�b�g
	/// </summary>
	/// <param name="setPosition"></param>
	public void SetPosition(Vector3 setPosition) {
		setPosition.z = _cameraDistance;
		_camera.transform.position = setPosition;
	}

}
