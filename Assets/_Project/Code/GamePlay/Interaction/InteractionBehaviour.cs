using System;
using _Project.Code.Core.Services.ContentProviderService;
using _Project.Code.GamePlay.CameraLogic;
using _Project.Code.GamePlay.CameraLogic.Factory;
using _Project.Code.GamePlay.Car;
using _Project.Code.GamePlay.Character;
using _Project.Code.GamePlay.Character.Factory;
using DG.Tweening;
using UnityEngine;
using VContainer;

namespace _Project.Code.GamePlay.Interaction
{
    [Serializable]
    public abstract class InteractionBehaviour
    {
        protected InteractableInfo Info;
        protected IInteractable Interactable;
        
        public void Initialize(IInteractable interactable) => Interactable = interactable;
        public virtual void InitInfo( InteractableInfo info) => Info = info;
        public virtual void Initialize(IObjectResolver resolver){}
        public abstract void Execute(Action onExecuted = null);
    }
    
    [Serializable]
    public class RotationBehaviour : InteractionBehaviour
    {
        [Header("Target")]
        public Transform Target;
        
        [Header("Target Rotations")]
        public Vector3 ExecuteEuler;
        public Vector3 RevertEuler;
        
        private bool _isExecuted = false;

        public override void InitInfo(InteractableInfo info)
        {
            base.InitInfo( info);
            Info.Name = "Open";
        }

        public override void Execute(Action onExecuted = null)
        {
            if (!_isExecuted)
            {
                _isExecuted = true;
                Info.Name = "Close";
                Rotate(Target, ExecuteEuler);
            }
            else
            {
                _isExecuted = false;
                Info.Name = "Open";
                Rotate(Target, RevertEuler);
            }
            
            onExecuted?.Invoke();
        }
        
        private void Rotate(Transform target, Vector3 euler)
        {
            var targetRotation = Quaternion.Euler(euler);
            target.localRotation = targetRotation;
        }
    }

    [Serializable]
    public class GetOnCarBehaviour : InteractionBehaviour
    {
        public Transform Car;
        
        [Header("Camera")]
        public Transform HeadPoint;
        
        [Header("CharacterHolder")]
        public Transform Holder;
        
        [Header("Pitch And Yaw")]
        public Vector2 Pitch;
        public Vector2 Yaw;
        
        private IContentProvider _contentProvider;

        public override void Initialize(IObjectResolver resolver) => 
            _contentProvider = resolver.Resolve<IContentProvider>();
        
        public override void Execute(Action onExecuted = null)
        {
            var character = _contentProvider.Character;
            var camera = _contentProvider.Camera;
            var car = _contentProvider.Car;

            GetOnCar(character, camera, car);
            
            onExecuted?.Invoke();
        }

        private void GetOnCar(ICharacter character, ICamera camera, ICar car)
        {
            car.SetInCar(true);
            character.Deactivate();
            character.Transform.SetParent(Holder);
            character.Head.position = HeadPoint.position;
            
            camera.Transform.SetParent(Holder, false);
            camera.LookModule.SetVehicleLook(Car, HeadPoint.forward , Pitch, Yaw);
        }
    }
    
    [Serializable]
    public class GetOutOfCarBehaviour : InteractionBehaviour
    {
        [Header("Position")]
        public Transform OutPoint;
        
        private IContentProvider _contentProvider;

        public override void Initialize(IObjectResolver resolver) => 
            _contentProvider = resolver.Resolve<IContentProvider>();

        public override void Execute(Action onExecuted = null)
        {
            var character = _contentProvider.Character;
            var camera = _contentProvider.Camera;
            var car = _contentProvider.Car;
            
            GetOutOfCar(character, camera, car);
            
            onExecuted?.Invoke();
        }
        
        private void GetOutOfCar(ICharacter character, ICamera camera, ICar car)
        {
            car.SetInCar(false);
            character.Transform.SetParent(null);
            character.Transform.localScale = Vector3.one;
            character.Motor.SetPosition(OutPoint.position + Vector3.up);
            character.ResetHeadPosition();
            
            camera.Transform.SetParent(character.CameraHolder, false);
            camera.LookModule.SetMode(CameraLookMode.Free);
            camera.WithModifiers(true);
            character.Activate();
        }
    }
    
    [Serializable]
    public class IgnitionCarBehaviour : InteractionBehaviour
    {
        [Header("Time")]
        public float IgnitionTime = 1.5f;
        public float ShutDownTime = 1f;
        
        private IContentProvider _contentProvider;
        private bool _isIgnited = false;
        
        public override void Initialize(IObjectResolver resolver) => 
            _contentProvider = resolver.Resolve<IContentProvider>();

        public override void InitInfo(InteractableInfo info)
        {
            base.InitInfo( info);
            Info.Name = "Ignite";
        }

        public override void Execute(Action onExecuted = null)
        {
            var camera = _contentProvider.Camera;
            var car = _contentProvider.Car;
            
            if (!_isIgnited)
                Ignite(camera, car);
            else
                ShutDown(car);
            
            onExecuted?.Invoke();
        }

        private void ShutDown(ICar car)
        {
            _isIgnited = false;
            Info.Name = "Ignite";
            
            Interactable.DisableAndEnableColliderAfterTime(ShutDownTime, () => {
                car.SetCarIgnited(false);
            });
        }

        private void Ignite(ICamera camera, ICar car)
        {
            _isIgnited = true;
            Info.Name = "Shut Down";
                
            Interactable.DisableAndEnableColliderAfterTime(IgnitionTime, () =>
            {
                camera.WithModifiers(false);
                car.SetCarIgnited(true);
            });
        }
    }
}