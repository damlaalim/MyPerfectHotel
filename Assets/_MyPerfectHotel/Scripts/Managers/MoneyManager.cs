using System.Collections.Generic;
using _MyPerfectHotel.Scripts.Controller;
using _MyPerfectHotel.Scripts.Data;
using _MyPerfectHotel.Scripts.Player;
using DG.Tweening;
using UnityEngine;

namespace _MyPerfectHotel.Scripts.Managers
{
    public class MoneyManager : MonoBehaviour
    {
        [SerializeField] private List<MoneyStackController> moneyStackControllerList;
        [SerializeField] private GameObject moneyPrefab;
        [SerializeField] private Vector3 moneyEndScale;
        [SerializeField] private float completionRadius, moneyJumpDuration, moneyYMoveDur, moneyTopPointMin, moneyTopPointMax,
            playerFollowDur, rotateDur;

        private Dictionary<Tween, MoneyStackType> _moneyMoveWorkerTweenDict = new();

        public void MoveToMoney(Vector3 initPos, MoneyStackType moneyStackType)
        {
            var moneyObject = Instantiate(moneyPrefab);
            moneyObject.transform.position = initPos;

            MoneyStackController currentController = null;

            foreach (var mSController in moneyStackControllerList)
            {
                if (mSController.type == moneyStackType)
                    currentController = mSController;
            }

            if (!currentController)
                return;

            var endPos = currentController.GetNextMoneyPos(moneyObject);
            var newTw = moneyObject.transform.DOJump(endPos, 1, 1, moneyJumpDuration);
            _moneyMoveWorkerTweenDict.Add(newTw, moneyStackType);
            newTw.OnComplete(() => _moneyMoveWorkerTweenDict.Remove(newTw));
        }

        public void MoveToMoney(GameObject moneyObject, PlayerController playerController, MoneyStackType stackType)
        {
            DOTween.SetTweensCapacity(2000, 100);
            
            foreach (var (key, value) in _moneyMoveWorkerTweenDict)
                if (value == stackType)
                    key.Kill();

            var moneyPos = moneyObject.transform.position;
            var movePos = moneyPos.y + Random.Range(moneyTopPointMin, moneyTopPointMax);
            var rotateDegree = new Vector2(Random.Range(130, 350), Random.Range(130, 350));

            var doRotateTw = moneyObject.transform.DORotate(rotateDegree, rotateDur);

            doRotateTw.OnComplete(() =>
            {
                var doMoveTw = moneyObject.transform.DOMoveY(movePos, moneyYMoveDur);

                doMoveTw.OnComplete(() =>
                {
                    moneyObject.transform.localScale = moneyEndScale;
                    
                    var followPlayerTween =
                        moneyObject.transform.DOMove(playerController.transform.position, playerFollowDur);
                    var isMoneyFollowingTarget = true;

                    followPlayerTween.OnUpdate(delegate
                    {
                        if (Vector3.Distance(moneyObject.transform.position, playerController.transform.position) >
                            completionRadius && isMoneyFollowingTarget)
                            followPlayerTween.ChangeEndValue(playerController.transform.position, true);
                        else
                        {
                            isMoneyFollowingTarget = false;
                            followPlayerTween.Kill();
                            Destroy(moneyObject);
                        }
                    });
                });
            });
        }
    }
}