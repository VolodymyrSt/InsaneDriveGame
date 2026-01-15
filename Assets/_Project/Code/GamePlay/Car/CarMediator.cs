using System;
using System.Threading.Tasks;
using _Project.Code.GamePlay.Interaction;
using _Project.Code.Util;
using UnityEngine;

namespace _Project.Code.GamePlay.Car
{
    public class CarMediator : MonoBehaviour, ICar
    {
        [SerializeField] private InteractableObject _getOnCar;
        [SerializeField] private InteractableObject _getOutOfCar;
        
        private bool _isInCar;

        public Transform Transform => transform;

        public void Init()
        {
            _getOutOfCar.OnInteracted += ExitCar;
            _getOnCar.OnInteracted += EnterCar;

            _isInCar = false;
            UpdateState();
        }

        private void EnterCar()
        {
            if (_isInCar) return;
            _isInCar = true;
            UpdateState();
        }

        private void ExitCar()
        {
            if (!_isInCar) return;
            _isInCar = false;
            UpdateState();
        }

        private void UpdateState()
        {
            _getOnCar.Toggle(!_isInCar);
            _getOutOfCar.Toggle(_isInCar);
        }

        private void OnDestroy()
        {
            _getOutOfCar.OnInteracted -= ExitCar;
            _getOnCar.OnInteracted -= EnterCar;
        }
    }
}