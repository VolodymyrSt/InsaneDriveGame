using System;
using _Project.Code.Util;
using UnityEngine;

namespace _Project.Code.GamePlay.Car.Parts.GearShifter.View
{
    public class GearShifterHub : MonoBehaviour
    {
        [SerializeField] private RectTransform _root;

        private void Start() => Hide();

        public void Show() => 
            _root.SetActive(true);
        
        public void Hide() => 
            _root.SetActive(false);
    }
}