using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuCredit : MenuBase {
    //�ړ�����N���W�b�g�摜
    [SerializeField]
    private RectTransform _moveImage = null;
    //����{�^��
    [SerializeField]
    private Button _closeButton = null;
    [SerializeField]
    private float _moveTime = 10.0f;
    private bool _isClose = false;
    private int maxMove = 1410;
    //���O�ړ��̃^�X�N�𒆒f���邽�߂̃g�[�N��
    private CancellationToken _token;

    public override async UniTask Initialize() {
        await base.Initialize();
        _moveImage.gameObject.transform.position = Vector3.zero;
    }
    public override async UniTask Open() {
        await base.Open();
        _token = this.GetCancellationTokenOnDestroy();
        _closeButton.gameObject.SetActive(false);
        _isClose = false;
        await FadeManager.instance.FadeIn();
        Vector3 startPos = Vector3.zero;
        Vector3 goalPos = new Vector3(0, maxMove, 0);
        float elapseTime = 0.0f;
        _moveImage.gameObject.transform.position = Vector3.zero;
        while (elapseTime < _moveTime) {
            elapseTime += Time.deltaTime;
            float t = elapseTime / _moveTime;
            _moveImage.gameObject.transform.position = Vector3.Lerp(startPos, goalPos, t);
            await UniTask.DelayFrame(1, PlayerLoopTiming.Update, _token);
        }
        _moveImage.gameObject.transform.position = goalPos;
        _closeButton.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_closeButton.gameObject);
        while (!_isClose) {
            await UniTask.DelayFrame(1,PlayerLoopTiming.Update, _token);
        }
        await FadeManager.instance.FadeOut();
        await Close();
    }
    public void CloseMenu() {
        _isClose = true;
    }
}
