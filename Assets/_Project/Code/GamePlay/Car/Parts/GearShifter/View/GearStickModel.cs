using _Project.Code.Configs.Car;
using _Project.Code.Core.Services.StaticData;
using UnityEngine;

namespace _Project.Code.GamePlay.Car.Parts.GearShifter.View
{
    public class GearStickModel
    {
        private const float TransitionProgressMaxValue = 1f;
        private const float TransitionProgressMinValue = 0f;
        private const float ManualMoveSpeed = 0.008f;
        private const float ReturnSpeed = 4f;

        private readonly IStaticDataService _staticDataService;
        
        private GearShiftNodeSO _currentNode;
        private GearShiftNodeSO _targetNode;
        private Direction _lastDirection = Direction.None;
        private float _transitionProgress = 0f;
        
        public float TransitionProgress => _transitionProgress;
        public Direction Direction  => _lastDirection;
        public GearShiftNodeSO CurrentNode  => _currentNode;
        public GearShiftNodeSO TargetNode  => _targetNode;

        public GearStickModel(IStaticDataService staticDataService) => 
            _staticDataService = staticDataService;

        public void Init()
        {
            SetCurrentNodeToNeutral();
            ClearTargetNode().SetMinTransitionProgress().WithNoneDirection();
        }

        public GearStickModel ClearTargetNode()
        {
            _targetNode = null;
            return this;
        }
        
        public GearStickModel WithNoneDirection()
        {
            _lastDirection = Direction.None;
            return this;
        }

        public GearStickModel SetMaxTransitionProgress()
        {
            _transitionProgress = TransitionProgressMaxValue;
            return this;
        }

        public GearStickModel SetMinTransitionProgress()
        {
            _transitionProgress = TransitionProgressMinValue;
            return this;
        }

        public void PickTargetNodeByDirection(Direction direction)
        {
            if (!_currentNode.TryGetNeighborByDirection(direction, out GearShiftNodeSO neighbor)) 
                return;

            _lastDirection = direction;
            _targetNode = neighbor;
            SetMinTransitionProgress();
        }

        public void UpdateTransitionProgress(float projection) =>
            _transitionProgress = Mathf.Clamp(_transitionProgress + projection * ManualMoveSpeed,
                TransitionProgressMinValue, TransitionProgressMaxValue);
        
        public bool IsAtMinTransitionProgress() => 
            _transitionProgress <= TransitionProgressMinValue; 
        
        public bool IsAtMaxTransitionProgress() => 
            _transitionProgress >= TransitionProgressMaxValue;

        public Vector2 GetAngle() => _targetNode != null
                ? Vector2.Lerp(_currentNode.TiltAngles, _targetNode.TiltAngles, _transitionProgress)
                : _currentNode.TiltAngles;

        public bool IsOnConnectorNode() => 
            _currentNode.Mode == GearMode.Connector;

        public void PrepareReturnToNeutral()
        {
            _targetNode = _staticDataService.CarConfig.StartingNode;
            _lastDirection = _currentNode.GetDirectionToNeighbor(GearMode.Neutral);
            _transitionProgress = TransitionProgressMinValue;
        }
        
        public void MoveProgressToMax() => 
            _transitionProgress = Mathf.MoveTowards(_transitionProgress, TransitionProgressMaxValue, Time.deltaTime * ReturnSpeed);
        
        public void MoveProgressToMin() => 
            _transitionProgress = Mathf.MoveTowards(_transitionProgress, TransitionProgressMinValue, Time.deltaTime * ReturnSpeed);
        
        
        public bool HasTargetNode() => 
            _targetNode == null;
        
        public void SwapNodes() =>
            _currentNode = _targetNode;
        
        private void SetCurrentNodeToNeutral() =>
            _currentNode = _staticDataService.CarConfig.StartingNode;
    }
}
