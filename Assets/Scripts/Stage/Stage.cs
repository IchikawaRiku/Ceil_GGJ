/*
 *  @file   StageManager.cs
 *  @brief  ステージクラス
 *  @author oorui
 */
using Cysharp.Threading.Tasks;


public class Stage : StageBase {

    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <returns></returns>
    public override async UniTask Initialize() {
        await base.Initialize();
        for (int i = 0, max = _gimmickBases.Length; i < max; i++) {
            if (_gimmickBases[i] == null) continue;
            _gimmickBases[i].Initialize();
        }
    }

    /// <summary>
    /// 初期化
    /// </summary>
    /// <returns></returns>
    public override async UniTask SetUp() {
        await base.SetUp();

        for (int i = 0, max = _gimmickBases.Length; i < max; i++) {
            if (_gimmickBases[i] == null) continue;
            _gimmickBases[i].SetUp();
        }

    }

    /// <summary>
    /// 片付け処理
    /// </summary>
    /// <returns></returns>
    public override async UniTask Teardown() {
        await base.Teardown();
        for (int i = 0, max = _gimmickBases.Length; i < max; i++) {
            if (_gimmickBases[i] == null) continue;
            _gimmickBases[i].Teardown();
        }
    }

    /// <summary>
    /// 実行処理
    /// </summary>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public override async UniTask Execute() {
        await UniTask.CompletedTask;
    }


}
