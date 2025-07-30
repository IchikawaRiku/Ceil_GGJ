using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterManager : MonoBehaviour {
	// 自身への参照
	public static CharacterManager instance { get; private set; } = null;
	[SerializeField]
	private Transform _useRoot = null;
	[SerializeField]
	private Transform _unuseRoot = null;
	// 生成するオブジェクトのオリジナル
	[SerializeField]
	private PlayerCharacter _playerOrigin = null;
	[SerializeField]
	private SpiritCharacter _spiritOrigin = null;

	// 生成したオブジェクト
	private PlayerCharacter _usePlayer = null;
	private SpiritCharacter _useSpirit = null;
	//未使用状態のプレイヤー
	private PlayerCharacter _unusePlayer = null;
	//未使用状態の幽霊
	private SpiritCharacter _unuseSpirit = null;
	// 操作中のキャラ
	public CharacterBase controlCharacter = null;

	private void Start () {
		instance = this;
		//プレイヤーの生成
		_unusePlayer = Instantiate(_playerOrigin, _unuseRoot);
		_unuseSpirit = Instantiate(_spiritOrigin, _unuseRoot);
		// 初期操作はプレイヤー
		controlCharacter = _usePlayer;
		// 幽体の入力はとらない
		_unuseSpirit.enabled = false;
		_unuseSpirit.Initialize();
	}

	private void Update () {
		// 操作キャラの実行処理
		controlCharacter.Execute();
		if (controlCharacter != _useSpirit) {
			_unuseSpirit.ReturnPosition();
		}
	}

	/// <summary>
	/// 操作キャラクターのチェンジ
	/// </summary>
	public void ChangeControlCharacter() {
		if (controlCharacter == _usePlayer) {
			//幽霊を生成
			UseSpirit();
			// コントロールを幽体にする
			controlCharacter = _useSpirit;
			// プレイヤーの入力を切る
			_usePlayer.enabled = false;
			// 幽体の入力をとる
			_useSpirit.enabled = true;
		}
		else if (controlCharacter == _useSpirit) {
			//幽霊を未使用化
			UnuseSpirit();
			// コントロールをプレイヤーにする
			controlCharacter = _usePlayer;
			// 幽体の入力を切る
			_unuseSpirit.enabled = false;
			// プレイヤーの入力をとる
			_usePlayer.enabled = true;
		}
	}
	/// <summary>
	/// プレイヤーの生成
	/// </summary>
	public void UsePlayer() {
		_usePlayer = _unusePlayer;
		_unusePlayer = null;
		_usePlayer.transform.SetParent(_useRoot);
	}
	/// <summary>
	/// プレイヤーの未使用化
	/// </summary>
	public void UnusePlayer() {
		_unusePlayer = _usePlayer;
		_usePlayer = null;
		_unusePlayer.transform.SetParent(_unuseRoot);
	}
	/// <summary>
	/// 幽霊の生成
	/// </summary>
	public void UseSpirit() {
		_useSpirit = _unuseSpirit;
		_unuseSpirit = null;
		_useSpirit.transform.SetParent(_useRoot);
	}
	/// <summary>
	/// 幽霊の未使用化
	/// </summary>
	public void UnuseSpirit() {
		_unuseSpirit = _useSpirit;
		_useSpirit = null;
		_unuseSpirit.transform.SetParent(_unuseRoot);
	}
}
