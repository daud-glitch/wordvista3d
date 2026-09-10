using System;
using System.Collections;
using UnityEngine;

namespace WordVista.CameraSystem
{
    public class CameraController : MonoBehaviour
    {
        [Header("Framing Transforms")]
        [SerializeField] private Vector3 introPosition = new Vector3(0, 15, -25);
        [SerializeField] private Vector3 gameplayPosition = new Vector3(0, 8, -14);
        [SerializeField] private Vector3 winPosition = new Vector3(0, 11, -19);

        [SerializeField] private Vector3 introRotation = new Vector3(25, 0, 0);
        [SerializeField] private Vector3 gameplayRotation = new Vector3(18, 0, 0);
        [SerializeField] private Vector3 winRotation = new Vector3(20, 0, 0);

        [Header("Smoothing")]
        [SerializeField] private float transitionDuration = 1.2f;
        [SerializeField] private float gentleSwaySpeed = 0.5f;
        [SerializeField] private float gentleSwayAmount = 0.15f;

        private Coroutine _activeTransition;
        private Vector3 _targetPos;
        private Quaternion _targetRot;
        private bool _isIdleGameplay;
        private float _swayTimer;

        private void Awake()
        {
            transform.position = introPosition;
            transform.rotation = Quaternion.Euler(introRotation);
            _targetPos = gameplayPosition;
            _targetRot = Quaternion.Euler(gameplayRotation);
        }

        private void Update()
        {
            if (_isIdleGameplay)
            {
                _swayTimer += Time.deltaTime * gentleSwaySpeed;
                float swayX = Mathf.Sin(_swayTimer) * gentleSwayAmount;
                float swayY = Mathf.Cos(_swayTimer * 0.7f) * (gentleSwayAmount * 0.5f);
                transform.position = _targetPos + new Vector3(swayX, swayY, 0);
            }
        }

        public void PlayIntroSequence(Action onComplete = null)
        {
            _isIdleGameplay = false;
            StartTransition(introPosition, Quaternion.Euler(introRotation),
                            gameplayPosition, Quaternion.Euler(gameplayRotation),
                            transitionDuration, () =>
                            {
                                _isIdleGameplay = true;
                                onComplete?.Invoke();
                            });
        }

        public void PlayLevelCompleteMotion(Action onComplete = null)
        {
            _isIdleGameplay = false;
            StartTransition(transform.position, transform.rotation,
                            winPosition, Quaternion.Euler(winRotation),
                            1.5f, onComplete);
        }

        public void ResetToGameplay()
        {
            _isIdleGameplay = false;
            StartTransition(transform.position, transform.rotation,
                            gameplayPosition, Quaternion.Euler(gameplayRotation),
                            0.8f, () => _isIdleGameplay = true);
        }

        private void StartTransition(Vector3 fromPos, Quaternion fromRot, Vector3 toPos, Quaternion toRot, float duration, Action callback)
        {
            if (_activeTransition != null)
            {
                StopCoroutine(_activeTransition);
            }
            _activeTransition = StartCoroutine(TransitionRoutine(fromPos, fromRot, toPos, toRot, duration, callback));
        }

        private IEnumerator TransitionRoutine(Vector3 fromPos, Quaternion fromRot, Vector3 toPos, Quaternion toRot, float duration, Action callback)
        {
            float elapsed = 0f;
            _targetPos = toPos;
            _targetRot = toRot;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
                transform.position = Vector3.Lerp(fromPos, toPos, t);
                transform.rotation = Quaternion.Slerp(fromRot, toRot, t);
                yield return null;
            }

            transform.position = toPos;
            transform.rotation = toRot;
            _activeTransition = null;
            callback?.Invoke();
        }
    }
}
