using System;

namespace TimToolBox {
public class Timer {
    private float _duration;
    private float _startTime;
    private float _pauseTime;

    public Action OnTimerStart = delegate { };
    public Action OnTimerStop = delegate { };
    public bool IsRunning { get; protected set; }

    public Timer(float duration) {
        this._duration = duration;
        Start();
    }
    protected virtual float CurrentTime => UnityEngine.Time.time;
    public float Duration {
        get => _duration;
        set => _duration = value;
    }

    public void Start() {
        _startTime = CurrentTime;
        _pauseTime = _startTime;
        if (!IsRunning) {
            IsRunning = true;
            OnTimerStart.Invoke();
        }
    }
    public void Start(float newDuration) {
        _duration = newDuration;
        Start();
    }
    public void Stop() {
        if (IsRunning) {
            IsRunning = false;
            OnTimerStop.Invoke();
        }
    }
    public void Pause() {
        if (IsRunning) {
            IsRunning = false;
            _pauseTime = CurrentTime;
        }
    }
    public void Resume() {
        if (!IsRunning) {
            IsRunning = true;
            var passedTime = _pauseTime - _startTime;
            _startTime = CurrentTime - passedTime; //set start time to current time minus the time passed while paused
        }
    }
    public bool IsPaused => !IsRunning && TimePassedRatio < 1;
    public bool IsFinished =>  TimePassed >= _duration;

    public float TimePassed => IsRunning ? CurrentTime - _startTime : _pauseTime - _startTime;
    public float TimeLeft => Math.Max(0, _duration - TimePassed);

    public float TimePassedRatio => TimePassed / _duration;
    public float TimeLeftRatio => TimeLeft / _duration;
}

public class UnscaledTimer : Timer {
    public UnscaledTimer(float duration) : base(duration) { }
    protected override float CurrentTime => UnityEngine.Time.unscaledTime;
} }
