using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterManager : MonoBehaviour {
	// 自身への参照
	public static CharacterManager instance { get; private set; } = null;
	// 生成するオブジェクトのオリジナル
	[SerializeField]
	private GameObject _playerOrigin = null;
	[SerializeField]
	private GameObject _spiritOrigin = null;

	// 生成したオブジェクト
	private GameObject _player = null;
	private GameObject _spirit = null;
	
	// 操作中のキャラ
	public GameObject controlCharacter = null;

	private void Start () {
		instance = this;
		_player = Instantiate(_playerOrigin);
		_spirit = Instantiate(_spiritOrigin, _player.transform);
		// 初期操作はプレイヤー
		controlCharacter = _player;
		// 幽体の入力はとらない
		_spirit.GetComponent<PlayerInput>().enabled = false;
	}

	private void Update () {
		// 操作キャラの実行処理
		controlCharacter.GetComponent<CharacterBase>().Execute();
	}

	/// <summary>
	/// 操作キャラクターのチェンジ
	/// </summary>
	public void ChangeControlCharacter() {
		if (controlCharacter == _player) {
			// プレイヤーの入力を切る
			_player.GetComponent<PlayerInput>().enabled = false;
			// コントロールを幽体にする
			controlCharacter = _spirit;
			// 幽体の入力をとる
			_spirit.GetComponent<PlayerInput>().enabled = true;
		}
		else if (controlCharacter == _spirit) {
			// 幽体の入力を切る
			_spirit.GetComponent<PlayerInput>().enabled = false;
			// コントロールをプレイヤーにする
			controlCharacter = _player;
			// プレイヤーの入力をとる
			_player.GetComponent<PlayerInput>().enabled = true;
		}
	}
}
