using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static CommonModule;
public abstract class StageBase : MonoBehaviour {

    [SerializeField]
    private GimmickBase[] gimmickBases = null;

    /// <summary>
    /// ‰Šú‰»ˆ—
    /// </summary>
    /// <returns></returns>
    public virtual async UniTask Initialize() {
        gameObject.SetActive(false);
        await UniTask.CompletedTask;
    }
    
    public virtual async UniTask SetUp() {
        gameObject.SetActive(true);
        await UniTask.CompletedTask;
    }

}
