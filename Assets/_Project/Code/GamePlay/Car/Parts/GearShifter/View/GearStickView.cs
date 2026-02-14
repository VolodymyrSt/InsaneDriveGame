using System;
using _Project.Code.Configs.Car;
using _Project.Code.Core.Services.Input;
using _Project.Code.Core.Services.StaticData;
using UnityEngine;

namespace _Project.Code.GamePlay.Car.Parts.GearShifter.View
{
    public class GearStickView : MonoBehaviour
    {
        public event Action<GearMode> OnGearModeChanged;
        
        private IInputService _input;
        private GearStickModel _model;
        
        private GearReturnState _gearReturnState;
        private bool _isGrabbing = false;

        public void Init(IInputService inputService, GearStickModel model)
        {
            _input = inputService;
            _model = model;

            UpdateVisual();
        }

        public void Grab()
        {
            _isGrabbing = true;
            _gearReturnState = GearReturnState.None;
        }

        public void Release()
        {
            _isGrabbing = false;

            if (!_model.HasTargetNode())
                _gearReturnState = _model.TransitionProgress > 0.5f ? GearReturnState.ToTarget : GearReturnState.ToCurrent;
            else
            {
                if (_model.IsOnConnectorNode())
                {
                    _model.PrepareReturnToNeutral();
                    _gearReturnState = GearReturnState.ToTarget;
                }
                else
                    OnGearModeChanged?.Invoke(_model.CurrentNode.Mode);
            }
        }

        private void Update()
        {
            if (_isGrabbing)
            {
                var mouseDelta = _input.GetMouseDelta();
                if (_model.HasTargetNode())
                    TryPickDirection(mouseDelta);
                else
                    MoveAlongNodeTileAngle(mouseDelta);
            }
            else
            {
                switch (_gearReturnState)
                {
                    case GearReturnState.None:
                        return;
                    case GearReturnState.ToNeutral:
                        ReturnToNeutral();
                        break;
                    case GearReturnState.ToTarget:
                        ReturnToTargetNode();
                        break;
                    case GearReturnState.ToCurrent:
                        ReturnToCurrentNode();
                        break;
                }
            }
        }

        private void TryPickDirection(Vector2 mouseDelta)
        {
            var direction = GetMouseDirection(mouseDelta);
            if (direction == Direction.None) return;
            _model.PickTargetNodeByDirection(direction);
        }

        private void MoveAlongNodeTileAngle(Vector2 mouseDelta)
        {
            var projection = GetProjection(mouseDelta);
            _model.UpdateTransitionProgress(projection);
            
            if (_model.IsAtMaxTransitionProgress())
            {
                _model.SwapNodes();
                _model.ClearTargetNode().SetMaxTransitionProgress().WithNoneDirection();
            }
            else if (_model.IsAtMinTransitionProgress())
                _model.ClearTargetNode().SetMinTransitionProgress();

            UpdateVisual();
        }

        private void UpdateVisual()
        {
            Vector2 currentAngle = _model.GetAngle();
            transform.localRotation = Quaternion.Euler(currentAngle.x, 0f, currentAngle.y);
        }
        
        private void ReturnToNeutral()
        {
            _model.MoveProgressToMax();
            UpdateVisual();
            
            if (_model.IsAtMaxTransitionProgress())
            {
                _model.SwapNodes();
                _model.ClearTargetNode().SetMinTransitionProgress().WithNoneDirection();
                OnGearModeChanged?.Invoke(_model.CurrentNode.Mode);
                _gearReturnState = GearReturnState.None;
            }
        }

        private void ReturnToCurrentNode()
        {
            _model.MoveProgressToMin();
            UpdateVisual();

            if (_model.IsAtMinTransitionProgress())
            {
                if (_model.IsOnConnectorNode())
                {
                    _model.PrepareReturnToNeutral();
                    _gearReturnState = GearReturnState.ToTarget;
                }
                else
                {
                    _model.ClearTargetNode().SetMinTransitionProgress().WithNoneDirection();
                    _gearReturnState = GearReturnState.None;
                    OnGearModeChanged?.Invoke(_model.CurrentNode.Mode);
                }
            }
        }

        private void ReturnToTargetNode()
        {
            _model.MoveProgressToMax();
            UpdateVisual();

            if (_model.IsAtMaxTransitionProgress())
            {
                _model.SwapNodes();

                if (_model.IsOnConnectorNode())
                {
                    _model.PrepareReturnToNeutral();
                    _gearReturnState = GearReturnState.ToTarget;
                }
                else
                {
                    _model.ClearTargetNode().SetMinTransitionProgress().WithNoneDirection();
                    _gearReturnState = GearReturnState.None;
                    OnGearModeChanged?.Invoke(_model.CurrentNode.Mode);
                }
            }
        }

        private Direction GetMouseDirection(Vector2 delta)
        {
            const float threshold = 0.1f;
            if (delta.magnitude < threshold) return Direction.None;
            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                return delta.x > 0 ? Direction.Right : Direction.Left;
            return delta.y > 0 ? Direction.Up : Direction.Down;
        }
        
        private float GetProjection(Vector2 mouseDelta)
        {
            float projection = _model.Direction switch
            {
                Direction.Left  => -mouseDelta.x,
                Direction.Right =>  mouseDelta.x,
                Direction.Up    =>  mouseDelta.y,
                Direction.Down  => -mouseDelta.y,
                _               =>  0f
            };
            return projection;
        }
    }
}
