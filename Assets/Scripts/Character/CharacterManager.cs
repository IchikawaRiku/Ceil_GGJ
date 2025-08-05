/*
 *  @file   CharacterManager.cs
 *  @brief  キャラクターの管理クラス
 *  @author Riku
 *  @date   2025/7/29
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

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
    public CharacterBase controlCharacter { get; private set; } = null;

    // InputSystem
    //private PlayerInput _playerInput;
    //private PlayerInput _spiritInput;

    public async UniTask Initialize() {
        instance = this;
        //プレイヤーの生成
        _unusePlayer = Instantiate(_playerOrigin, _unuseRoot);
        _unuseSpirit = Instantiate(_spiritOrigin, _unuseRoot);
        await _unuseSpirit.Initialize();
        await _unusePlayer.Initialize();
        
		await UniTask.CompletedTask;
    }

    /// <summary>
    /// 実行処理
    /// </summary>
    /// <returns></returns>
    public async UniTask Execute() {
        if (controlCharacter == null) return;
		// 操作キャラの実行処理
		await controlCharacter.Execute();
        if (controlCharacter != _useSpirit) {
            _unuseSpirit.ReturnPosition();
        }
		await UniTask.CompletedTask;
    }

    /// <summary>
    /// 操作キャラクターのチェンジ
    /// </summary>
    public async UniTask ChangeControlCharacter() {
        if (controlCharacter == _usePlayer) {
            //幽霊を生成
            UseSpirit();
            // プレイヤーの入力を切る
            controlCharacter.DisableInput();
			// コントロールを幽霊にする
			controlCharacter = _useSpirit;
            // 幽霊の入力をとる
            controlCharacter.EnableInput();
			// アニメーション再生
			_usePlayer.anim.SetBool("change", true);

		}
        else if (controlCharacter == _useSpirit) {
            //幽霊を未使用化
            UnuseSpirit();
            // 幽霊の入力を切る
            controlCharacter.DisableInput();
			// コントロールをプレイヤーにする
			controlCharacter = _usePlayer;
			// プレイヤーの入力をとる
			controlCharacter.EnableInput();
            // アニメーション終了
            _usePlayer.anim.SetBool("change", false);
		}
        await UniTask.CompletedTask;
    }

    /// <summary>
    /// プレイヤーの生成
    /// </summary>
    public void UsePlayer() {
        _usePlayer = _unusePlayer;
        _unusePlayer = null;
        _usePlayer.transform.SetParent(_useRoot);
        if (controlCharacter == _usePlayer) return;
		// コントロールをプレイヤーにする
		controlCharacter = _usePlayer;
	}
    /// <summary>
    /// プレイヤーの未使用化
    /// </summary>
    public void UnusePlayer() {
		if (_usePlayer == null) return;
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
        if (_useSpirit == null) return;
        _unuseSpirit = _useSpirit;
        _useSpirit = null;
        _unuseSpirit.transform.SetParent(_unuseRoot);
	}

    /// <summary>
    /// プレイヤーの位置取得
    /// </summary>
    public Vector3 GetPlayerPosition() {
        if (_usePlayer == null) return Vector3.zero;
        Vector3 position = _usePlayer.transform.position;
        return position;
    }

    /// <summary>
    /// プレイヤーの位置のセット
    /// </summary>
    /// <param name="positoin"></param>
    public void SetPlayerPosition(Vector3 positoin) {
        _usePlayer.transform.position = positoin;
    }

    /// <summary>
    /// 幽霊の位置取得
    /// </summary>
    public Vector3 GetSpiritPosition() {
        if (_useSpirit == null) return Vector3.zero;
        Vector3 position = _useSpirit.transform.position;
        return position;
    }
    
    /// <summary>
    /// 片付け
    /// </summary>
    public void Teardown() {
        _usePlayer.Teardown();
        if (_useSpirit == null) _unuseSpirit.Teardown();
        else _useSpirit.Teardown();
        UnusePlayer();
        UnuseSpirit();
        
    }

}
