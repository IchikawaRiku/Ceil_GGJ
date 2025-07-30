using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {
	// 自身への参照
	public static CameraManager instance { get; private set; } = null;
	// 管理中のカメラ
	private Camera _camera = null;
	// カメラの離れる距離
	[SerializeField]
	private float _cameraDistance = -10f;
	// カメラの名前
	private const string _CAMERA_NAME = "Main Camera";

	/// <summary>
	/// 初期化
	/// </summary>
	public void Initialize() {
		instance = this;
		// シーン上のカメラをキャッシュ
		_camera = GameObject.Find(_CAMERA_NAME).GetComponent<Camera>();
	}

	/// <summary>
	/// カメラの位置をセット
	/// </summary>
	/// <param name="setPosition"></param>
	public void SetPosition(Vector3 setPosition) {
		setPosition.z = _cameraDistance;
		_camera.transform.position = setPosition;
	}

}
