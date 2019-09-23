using System;
using UnityEngine;

namespace ProjectName.Utils.InputSystem
{
    public interface IInputController
    {
        void DetectInput();
        float GetAxis(string axisType);

        event Action<Vector2> TouchBegan;
        event Action<Vector2> PointerDown;
        event Action<Vector2> TouchEnd;
    }
}