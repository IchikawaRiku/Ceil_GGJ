/*
 *  @file   PartMainGame.cs
 *  @brief  メインゲームパート
 *  @author Seki
 *  @date   2025/7/29
 */
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartMainGame : PartBase {
    [SerializeField]
    private CharacterManager _characterManager = null;
    //入力受付
    private AcceptMenu _acceptMenu = null;
    public override async UniTask Initialize() {
        await base.Initialize();
        _acceptMenu = new AcceptMenu();
    }

    public override async UniTask Execute() {
        _characterManager.Initialize();
        await _acceptMenu.AcceptInput();
    }
}
