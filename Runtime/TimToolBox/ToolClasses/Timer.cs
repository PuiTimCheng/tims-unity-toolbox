using System;

public class Timer {
    private float _duration;
    private float _startTime;
    private bool _isPaused;
    private float _pauseTime;

    public bool HasStarted { get; private set; }

    public Timer(float duration) {
        this._duration = duration;
        Reset();
    }

    protected virtual float CurrentTime => UnityEngine.Time.time;

    public float Duration {
        get => _duration;
        set => _duration = value;
    }

    public void Start() {
        HasStarted = true;
        _startTime = CurrentTime;
        _isPaused = false;
        _pauseTime = _startTime;
    }

    public void Reset() {
        HasStarted = false;
        _startTime = CurrentTime;
        _isPaused = false;
        _pauseTime = _startTime;
    }

    public void Start(float newDuration) {
        _duration = newDuration;
        Start();
    }

    public void Reset(float newDuration) {
        _duration = newDuration;
        Reset();
    }
    
    public bool IsPaused => _isPaused;

    public void Pause() {
        if (!HasStarted) return;
        if (!_isPaused) {
            _pauseTime = CurrentTime;
            _isPaused = true;
        }
    }

    public void Resume() {
        if (!HasStarted) return;
        if (_isPaused) {
            var passedTime = _pauseTime - _startTime;
            _startTime = CurrentTime - passedTime; //set start time to current time minus the time passed while paused
            _isPaused = false;
        }
    }

    public bool IsFinished() {
        if (!HasStarted || _isPaused) return false;
        return CurrentTime - _startTime >= _duration;
    }

    public float TimePassed => !HasStarted ? 0 : !_isPaused ? CurrentTime - _startTime : _pauseTime - _startTime;
    public float TimeLeft => Math.Max(0, _duration - TimePassed);

    public float TimePassedRatio => TimePassed / _duration;
    public float TimeLeftRatio => TimeLeft / _duration;
}

public class UnscaledTimer : Timer {
    public UnscaledTimer(float duration) : base(duration) { }
    protected override float CurrentTime => UnityEngine.Time.unscaledTime;
}