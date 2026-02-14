using System;
using System.Threading.Tasks;
using _Project.Code.Core.Services.Input;
using _Project.Code.Core.Services.StaticData;
using _Project.Code.GamePlay.CameraLogic;
using _Project.Code.GamePlay.Car.Parts.BreakSystem;
using _Project.Code.GamePlay.Car.Parts.CarAxle;
using _Project.Code.GamePlay.Car.Parts.Engine;
using _Project.Code.GamePlay.Car.Parts.GearShifter;
using _Project.Code.GamePlay.Car.Parts.GearShifter.View;
using _Project.Code.GamePlay.Interaction;
using _Project.Code.Util;
using Unity.Cinemachine;
using UnityEngine;
using VContainer;

namespace _Project.Code.GamePlay.Car
{
    public class CarMediator : MonoBehaviour, ICar
    {
        [Header("Base")]
        [SerializeField] private Rigidbody _rigidbody;
        
        [Header("Wheels")]
        [SerializeField] private WheelCollider _wheelFR;
        [SerializeField] private WheelCollider _wheelFL;
        [SerializeField] private WheelCollider _wheelRR;
        [SerializeField] private WheelCollider _wheelRL;
        
        [Header("Shifter")]
        [SerializeField] private GearStickView _gearStick;
        [SerializeField] private GearShifterHub _shifterHub;
        
        private IInputService _input;
        private IStaticDataService _staticDataService;
        
        private Engine _engine;
        private BreakSystem _breakSystem;
        private Axle _rearAxle;
        private FrontAxle _frontAxle;
        private GearShifter _shifter;
        private GearStickModel _gearStickModel;
        
        private bool _isInCar;
        private bool _isCarIgnited;

        public Transform Transform => transform;

        [Inject]
        private void Construct(IInputService inputService, IStaticDataService staticDataService)
        {
            _input = inputService;
            _staticDataService = staticDataService;
        }

        private void OnValidate() => 
            _rigidbody ??= GetComponent<Rigidbody>();

        public void Init(ICamera cameraHandler)
        {
            _rigidbody.centerOfMass = new Vector3(0, -0.5f, 0);
            
            _engine = new Engine(_rigidbody);
            _frontAxle = new FrontAxle(_wheelFR,  _wheelFL);
            _rearAxle = new Axle(_wheelRR, _wheelRL);
            _breakSystem = new BreakSystem(_frontAxle, _rearAxle, DrivetrainType.BOTH);
            _gearStickModel = new GearStickModel(_staticDataService);
            _shifter = new GearShifter(_gearStick, cameraHandler, _shifterHub);
            
            _gearStickModel.Init();
            _gearStick.Init(_input, _gearStickModel);
            _shifter.Init();
            
            _isInCar = false;
            _isCarIgnited = false;
        }
        
        public void SetCarIgnited(bool isCarIgnited) => 
            _isCarIgnited = isCarIgnited;
        
        public void SetInCar(bool isInCar) => 
            _isInCar = isInCar;

        private void Update()
        {
            _engine.RequestInput(_input.GetCarGasInput());
            _shifter.RequestInput(_input.GetCarClutchInput());
        }

        private void FixedUpdate()
        {
            if (!_isInCar || !_isCarIgnited) return;

            if (IsBraking())
                _breakSystem.Break();
            else
            {
                _breakSystem.Release();
                float motorTorque = _engine.GetMotorTorque();
                _rearAxle.SetMotorTorque(motorTorque);
            }
            
            TurnWheels();
            _shifter.Update();
        }

        private void TurnWheels() => 
            _frontAxle.PerformWheelsTurn(_input.GetCarSteeringWheelTurnAxis());

        private bool IsBraking() => 
            _input.GetCarBreakInput() > 0.05f;

        private void OnDestroy() => 
            _shifter.Dispose();
    }
}