using System;
using _Project.Code.GamePlay.CameraLogic;
using _Project.Code.GamePlay.Car.Parts.GearShifter.View;
using _Project.Code.Util;
using UnityEngine;

namespace _Project.Code.GamePlay.Car.Parts.GearShifter
{
    public class GearShifter : IDisposable
    {
        private readonly GearStickView _gearStickView;
        private readonly GearShifterHub _hub;
        private readonly ICamera _camera;
        private readonly Canvas _shiftingCanvas;
        
        private GearMode _gearMode;
        private bool _requestedClutch;
        
        private bool _isProcessing = false;
        
        public GearShifter(GearStickView gearStickView, ICamera camera, GearShifterHub hub)
        {
            _gearStickView = gearStickView;
            _camera = camera;
            _hub = hub;
        }

        public void Init() => 
            _gearStickView.OnGearModeChanged += OnGearModeChanged;

        private void OnGearModeChanged(GearMode newMode)
        {
            Debug.Log(newMode);
        }

        public void RequestInput(float requestedClutchInput) => 
            _requestedClutch = Mathf.Approximately(requestedClutchInput, 1) ? true : false;

        public void Update()
        {
            if (_requestedClutch)
                TryGrab();
            else
                TryRelease();
        }

        private void TryRelease()
        {
            if (!_isProcessing) return;
                        
            _isProcessing = false;
            _gearStickView.Release();
            _hub.Hide();
            _camera.UnblockCharacterLook();
        }

        private void TryGrab()
        {
            if (_isProcessing) return;
                        
            _isProcessing = true;
            _gearStickView.Grab();
            _camera.BlockCharacterLook();
            _hub.Show();
        }
        
        public void Dispose() => 
            _gearStickView.OnGearModeChanged -= OnGearModeChanged;
    }
}