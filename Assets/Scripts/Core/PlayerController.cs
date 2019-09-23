using System;
using ProjectName.Utils.InputSystem;
using UnityEngine;

namespace ProjectName.Core
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private IInputController _inputController;

        public event Action<Vector3> TouchStart;
        public event Action<Vector3> TouchContinue;
        public event Action<Vector3> TouchEnd;

        private void Awake()
        {
            _inputController = new SwipeSystem(1, false);

            _inputController.TouchBegan += OnTouchStart;
            _inputController.TouchEnd += OnTouchEnd;
            _inputController.PointerDown += OnTouchContinue;
        }

        private void OnDisable()
        {
            _inputController.TouchBegan -= OnTouchStart;
            _inputController.TouchEnd -= OnTouchEnd;
            _inputController.PointerDown -= OnTouchContinue;
        }

        private void Update()
        {
            _inputController.DetectInput();
        }

        private void OnTouchStart(Vector2 position)
        {
            TouchStart?.Invoke(position);
        }

        private void OnTouchEnd(Vector2 position)
        {
            TouchEnd?.Invoke(position);
        }

        private void OnTouchContinue(Vector2 position)
        {
            TouchContinue?.Invoke(position);
        }
    }
}