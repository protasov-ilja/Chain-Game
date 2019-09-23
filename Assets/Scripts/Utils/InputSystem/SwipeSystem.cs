using System;
using UnityEngine;

namespace ProjectName.Utils.InputSystem
{
    public class SwipeSystem : IInputController
    {
        private float _movementRange = 100;

		private Vector2 _startPressPos;
		private Vector2 _lastPressPos;
		private Vector2 _currPressPos;
		private Vector2 _endPressPos;

		private Vector2 _currentSwipe;

		private bool _isRoundedField = false;
		private bool _isSwipeEnded;

		public event Action<Vector2> TouchBegan;
		public event Action<Vector2> PointerDown;
		public event Action<Vector2> TouchEnd;

		public float GetAxis(string axisType)
		{
			switch (axisType)
			{
				case "Horizontal":
					return _currentSwipe.x;
				case "Vertical":
					return _currentSwipe.y;
				default:
					return 0;
			}
		}

		public SwipeSystem(float maxSwipeRangeCoeff, bool isRoundedField)
		{
			_isRoundedField = isRoundedField;
			_movementRange = Mathf.Clamp01(maxSwipeRangeCoeff) * Screen.height;
		}

		private bool GetTouchInput()
		{
			if (Input.touches.Length > 0)
			{
				var touch = Input.GetTouch(0);
				switch (touch.phase)
				{
					case TouchPhase.Began:
						_startPressPos = touch.position;
						_currPressPos = _startPressPos;
						_isSwipeEnded = false;
						TouchBegan?.Invoke(touch.position);

						break;
					case TouchPhase.Ended:
					case TouchPhase.Canceled:
						_endPressPos = touch.position;
						_isSwipeEnded = true;

						TouchEnd?.Invoke(touch.position);

						return true;
					case TouchPhase.Moved:
					case TouchPhase.Stationary:
						_lastPressPos = _currPressPos;
						_currPressPos = touch.position;

						PointerDown?.Invoke(touch.position);

						return true;
				}
			}

			return false;
		}

		private bool GetMouseInput()
		{
			if (Input.GetMouseButtonDown(0))
			{
				_startPressPos = Input.mousePosition;
				_currPressPos = _startPressPos;
				_isSwipeEnded = false;
				TouchBegan?.Invoke(_startPressPos);
			}
			else if (Input.GetMouseButtonUp(0))
			{
				_endPressPos = Input.mousePosition;
				_isSwipeEnded = true;
				TouchEnd?.Invoke(_endPressPos);

				return true;
			}
			else if (Input.GetMouseButton(0))
			{
				_lastPressPos = _currPressPos;
				_currPressPos = Input.mousePosition;
				PointerDown?.Invoke(_currPressPos);

				return true;
			}

			return false;
		}

		public void DetectInput()
		{
			if (GetTouchInput() || GetMouseInput())
			{
				if (_isSwipeEnded)
				{
					_currentSwipe = Vector2.zero;
					_startPressPos = Vector2.zero;

					return;
				}

				_currentSwipe = _currPressPos - _startPressPos;
				if (_isRoundedField)
				{
					_currentSwipe = Vector3.ClampMagnitude(_currentSwipe, _movementRange);
				}
				else
				{
					var deltaX = Mathf.Clamp(_currentSwipe.x, -_movementRange, _movementRange);
					var deltaY = Mathf.Clamp(_currentSwipe.y, -_movementRange, _movementRange);
					_currentSwipe = new Vector2(deltaX, deltaY);
				}

				_currentSwipe = _currentSwipe / _movementRange;
			}
		}
    }
}