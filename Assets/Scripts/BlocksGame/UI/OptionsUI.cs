using System;
using MessagePipe;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace BlocksGame.UI
{
    public class OptionsUI : MonoBehaviour
    {
        [SerializeField] private SerializedDictionary<VolumeType, Slider> volumeSliders = new  SerializedDictionary<VolumeType, Slider>();
        
        private IDisposable disposable;
            
        private void Start()
        {
            var disposableBuilder = Disposable.CreateBuilder();
            foreach (var slider in volumeSliders)
            {
                if (AudioManager.VolumeParameterNames.TryGetValue(slider.Key, out var paramName) && PlayerPrefs.HasKey(paramName))
                {
                    slider.Value.value = PlayerPrefs.GetFloat(paramName);
                }
                slider.Value.onValueChanged.AsObservable().Subscribe(value =>
                {
                    OnSliderValueChanged(slider.Key, value);
                }).AddTo(ref disposableBuilder);
            }
            disposable = disposableBuilder.Build();
        }

        private void OnDestroy()
        {
            disposable?.Dispose();
        }

        private void OnSliderValueChanged(VolumeType type, float value)
        {
            GlobalMessagePipe.GetPublisher<MSetVolume>().Publish(new MSetVolume() {Type = type, Percentage = value});
        }
    }
}
