using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BarUI : MonoBehaviour
{
    public TMP_Text text;
    public Slider mainSlider;
    public Slider diffSlider;
    public float diffMergeDelay;
    public float diffMergeTime;
    public Color addingDiffColor, deductingDiffColor;
    
    public float _maxValue;
    public float _currentValue;

    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

    private void OnDestroy()
    {
        cancellationTokenSource.Cancel();
    }
    
    public void SetValue(float currentValue)
    {
        var previousValue = _currentValue;
        this._currentValue = currentValue;
        if (!Mathf.Approximately(currentValue, previousValue))
        {
            cancellationTokenSource.Cancel();
            cancellationTokenSource = new CancellationTokenSource();
            if (currentValue > previousValue) {_ = MainBarMergeProcess(previousValue, currentValue, _maxValue,cancellationTokenSource.Token);}
            else if (currentValue < previousValue) _ = DiffBarMergeProcess(previousValue, currentValue, _maxValue,cancellationTokenSource.Token);
        }
        text.text = $"{currentValue}/{_maxValue}";
    }
    public void SetValue(float currentValue, float maxValue)
    {
        this._maxValue = maxValue;
        SetValue(currentValue);
    }
    public async UniTaskVoid MainBarMergeProcess(float startValue, float endValue, float maxValue, CancellationToken cancellationToken)
    {
        mainSlider.value = startValue / maxValue;
        diffSlider.value = endValue / maxValue;
        diffSlider.fillRect.GetComponent<Image>().color = addingDiffColor;
        await UniTask.Delay(TimeSpan.FromSeconds(diffMergeDelay), ignoreTimeScale: false, cancellationToken: cancellationToken);
        float counter = 0;
        while (counter < diffMergeTime)
        {
            cancellationToken.ThrowIfCancellationRequested();
            counter += Time.deltaTime;
            var timeRatio = counter / diffMergeTime;
            mainSlider.value = Mathf.Lerp(startValue, endValue,timeRatio) / maxValue;
            await UniTask.NextFrame();
        }
        mainSlider.value = endValue / maxValue;
    } 
    public async UniTaskVoid DiffBarMergeProcess(float startValue, float endValue, float maxValue, CancellationToken cancellationToken)
    {
        diffSlider.value = startValue / maxValue;
        mainSlider.value = endValue / maxValue;
        diffSlider.fillRect.GetComponent<Image>().color = deductingDiffColor;
        await UniTask.Delay(TimeSpan.FromSeconds(diffMergeDelay), ignoreTimeScale: false, cancellationToken: cancellationToken);
        float counter = 0;
        while (counter < diffMergeTime)
        {
            cancellationToken.ThrowIfCancellationRequested();
            counter += Time.deltaTime;
            var timeRatio = counter / diffMergeTime;
            diffSlider.value = Mathf.Lerp(startValue, endValue,timeRatio) / maxValue;
            await UniTask.NextFrame();
        }
        diffSlider.value = endValue / maxValue;
    } 
}