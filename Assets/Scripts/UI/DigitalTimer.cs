using ProjectName.Core;
using TMPro;
using UnityEngine;

namespace ProjectName.UI
{
    public class DigitalTimer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textValue;
        [SerializeField] private Color _alertColor;
        [Tooltip("Time in seconds")]
        [SerializeField] private int _startTimeOfAlert;
        
        private Color _defaultColor;

        private bool _isAlertOn = false;

        private const int FAULT = 1;

        private void Awake()
        {
            _defaultColor = _textValue.color;
        }
        
        public void Initialize(Timer timer)
        {
            timer
                .OnUpdate(UpdateTime)
                .OnPause(UpdateTime)
                .OnRestart(SetDefaultColor)
                .OnTimerEnd(EndTime);
        }

        private void SetDefaultColor()
        {
            _isAlertOn = false;
            _textValue.color = _defaultColor;
        }

        private void UpdateTime(TimerVO timerVO)
        {
            var time = timerVO.Time;
            
            SetColor((float)time.TotalSeconds);
            var hour = LeadingZero(time.Hours);
            var minute = LeadingZero( time.Minutes);
            var second = LeadingZero(time.Seconds + FAULT);
            _textValue.text = $"{ minute }:{ second }";
        }
        
        private void EndTime()
        {
            var minute = LeadingZero(0);
            var second = LeadingZero(0);
            _textValue.text = $"{ minute }:{ second }";
        }

        private static string LeadingZero(int number)
        {
            if (number < 0) number = 0;
            
            return number.ToString().PadLeft(2, '0');
        }

        private void SetColor(float currentTime)
        {
            if (!_isAlertOn && ((int)currentTime <= _startTimeOfAlert))
            {
                _isAlertOn = true;
                _textValue.color = _alertColor;
            }
        }

    }
}