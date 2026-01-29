using System;
using System.Threading.Tasks;
using _Project.Code.Core.Services.Input;
using _Project.Code.GamePlay.Interaction;
using _Project.Code.Util;
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
        
        private IInputService _input;
        private CarDriveModule _carDriveModule;
        
        private bool _isInCar;
        private bool _isCarIgnited;

        public Transform Transform => transform;

        [Inject]
        private void Construct(IInputService inputService) => 
            _input = inputService;

        private void OnValidate() => 
            _rigidbody ??= GetComponent<Rigidbody>();

        public void Init()
        {
            _carDriveModule = new CarDriveModule(_wheelFR,  _wheelFL, _wheelRR, _wheelRL, _rigidbody);
            _carDriveModule.Init();
            
            _isInCar = false;
            _isCarIgnited = false;
        }
        
        public void SetCarIgnited(bool isCarIgnited) => 
            _isCarIgnited = isCarIgnited;
        
        public void SetInCar(bool isInCar) => 
            _isInCar = isInCar;

        private void FixedUpdate()
        {
            if (!_isInCar || !_isCarIgnited) return;
            
            _carDriveModule.RequestInput(_input.GetCarDriveVector());
            _carDriveModule.Drive();
        }
    }
}