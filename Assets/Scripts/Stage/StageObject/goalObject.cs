using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static MainGameProcessor;
public class goalObject : GimmickBase {
    // ƒS[ƒ‹‚µ‚½‚©‚Ç‚¤‚©
    public bool isGoal { get; private set; } = false;

    /// <summary>
    /// €”õ
    /// </summary>
    public override void SetUp() {
        base.SetUp();
        isGoal = false;
    }


    /// <summary>
    /// XVˆ—
    /// </summary>
    protected override void OnUpdate() {
    }


    private void OnTriggerEnter(Collider collision) {
        if (collision.gameObject.layer == 6) {
            isGoal = true;
            EndGameReason(eEndReason.Clear);

        }
    }
}
