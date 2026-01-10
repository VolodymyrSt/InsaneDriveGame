using _Project.Code.Configs.Character;
using _Project.Code.Util;
using KinematicCharacterController;
using UnityEngine;

namespace _Project.Code.GamePlay.Character
{
    public class KinematicCharacterModule : ICharacterController
    {
        private readonly KinematicCharacterMotor _motor;
        private readonly Transform _cameraTarget;
        
        private readonly float _jumpForce;
        
        private readonly float _walkSpeed;
        private readonly float _runSpeed;
        private readonly float _crouchSpeed;
        
        private readonly float _walkResponse;
        private readonly float _runResponse;
        private readonly float _crouchResponse;
        
        private readonly float _crouchCameraTargetHeight;
        private readonly float _standCameraTargetHeight;
        
        private readonly float _crouchHeightResponse;
        
        private readonly float _crouchHeight;
        private readonly float _standHeight;
        
        private readonly float _airAcceleration;
        private readonly float _airSpeed;

        private readonly Collider[] _uncrouchOverlapResults;

        private Vector3 _acceleration;
        
        private Vector3 _requestedMoveDirection;
        private Quaternion _requestedRotation;
        private bool _requestedJumpInput;
        private bool _requestedCrouchInput;
        private bool _requestedRunInput;
        
        private CharacterStance _characterStance;

        public KinematicCharacterModule(KinematicCharacterMotor motor, CharacterConfigSO config, Transform cameraTarget)
        {
            _motor = motor;
            _cameraTarget = cameraTarget;

            _uncrouchOverlapResults = new Collider[8];
            
            _walkSpeed                = config.WalkSpeed;
            _runSpeed                 = config.RunSpeed;
            _crouchSpeed              = config.CrouchSpeed;
            _jumpForce                = config.JumpForce;
            _crouchCameraTargetHeight = config.CrouchCameraTargetHeight;
            _standCameraTargetHeight  = config.StandCameraTargetHeight;
            _standHeight              = config.StandHeight;
            _crouchHeight             = config.CrouchHeight;
            _crouchHeightResponse     = config.CrouchHeightResponse;
            _walkResponse             = config.WalkResponse;
            _crouchResponse           = config.CrouchResponse;
            _runResponse              = config.RunResponse;
            _airAcceleration          = config.AirAcceleration;
            _airSpeed                 = config.AirSpeed;
        }

        public void Init()
        {
            _motor.CharacterController = this;
            _characterStance = CharacterStance.Stand;
        }

        public void UpdateInput(Vector3 moveInput, Quaternion rotation, bool jumpInput, bool crouchInput
            , bool runInput)
        {
            _requestedRotation      = rotation;
            _requestedMoveDirection = rotation * new Vector3(moveInput.x, 0f, moveInput.y).normalized;
            _requestedCrouchInput   = crouchInput;
            _requestedJumpInput     = jumpInput;
            _requestedRunInput      = runInput;
        }

        public void UpdateBody()
        {
            var targetCameraHeight = GetTargetCameraHeight();
            
            _cameraTarget.localPosition = Vector3.Lerp(
                _cameraTarget.localPosition,
                new Vector3(_cameraTarget.localPosition.x, targetCameraHeight, _cameraTarget.localPosition.z),
                1f - Mathf.Exp(-_crouchHeightResponse * Time.deltaTime)
            );
        }

        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            var lookDirection = Vector3.ProjectOnPlane(_requestedRotation * Vector3.forward, _motor.CharacterUp); 
            
            if (lookDirection != Vector3.zero)
                currentRotation = Quaternion.LookRotation(lookDirection, _motor.CharacterUp);
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            _acceleration = Vector3.zero;
            
            if (IsOnGround())
                PerformMove(ref currentVelocity, deltaTime);
            else
                ApplyGravity(ref currentVelocity, deltaTime);

            if (CanJump())
            {
                if (!_motor.GroundingStatus.IsStableOnGround) return;
                PerformJump(ref currentVelocity);
            }
        }

        public void BeforeCharacterUpdate(float deltaTime)
        {
            if (!_requestedCrouchInput || !IsOnGround() || _characterStance != CharacterStance.Stand) return;
            
            _characterStance = CharacterStance.Crouch;
            SetCapsuleDimensionsByStance(CharacterStance.Crouch);
        }

        public void AfterCharacterUpdate(float deltaTime)
        {
            if (_requestedCrouchInput || !IsOnGround() || _characterStance != CharacterStance.Crouch) return;
            
            SetCapsuleDimensionsByStance(CharacterStance.Stand);

            if (_motor.CharacterOverlap(_motor.TransientPosition, _motor.TransientRotation,
                    _uncrouchOverlapResults, _motor.CollidableLayers, QueryTriggerInteraction.Ignore) > 0)
                SetCapsuleDimensionsByStance(CharacterStance.Crouch);
            else
                _characterStance = CharacterStance.Stand;
        }
        
        public Vector3 GetAcceleration() => _acceleration;
        
        private void ApplyGravity(ref Vector3 currentVelocity, float deltaTime)
        {
            if (_requestedMoveDirection.magnitude > 0)
            {
                var planerMovement = Vector3.ProjectOnPlane(
                    _requestedMoveDirection
                    , _motor.CharacterUp
                ).normalized;
                    
                var currentPlanerVelocity = Vector3.ProjectOnPlane(
                    currentVelocity
                    , _motor.CharacterUp
                );
                    
                var movementForce = planerMovement * _airAcceleration *  deltaTime;
                var targetPlanerVelocity = currentPlanerVelocity + movementForce;
                targetPlanerVelocity = Vector3.ClampMagnitude(targetPlanerVelocity, _airSpeed);
                    
                currentVelocity += targetPlanerVelocity - currentPlanerVelocity;
            }

            currentVelocity += _motor.CharacterUp * Constants.Gravity * deltaTime;
        }
        
        private void PerformMove(ref Vector3 currentVelocity, float deltaTime)
        {
            var groundedMovement = _motor.GetDirectionTangentToSurface
                    (_requestedMoveDirection, _motor.GroundingStatus.GroundNormal)
                    .normalized;

            var speed = _characterStance == CharacterStance.Stand ? _requestedRunInput ? _runSpeed : _walkSpeed : _crouchSpeed;
            var response = _characterStance == CharacterStance.Stand ? _requestedRunInput ? _runResponse : _walkResponse : _crouchResponse;
            
            var targetVelocity = groundedMovement * speed;
                
            var moveVelocity = Vector3.Lerp(currentVelocity, targetVelocity,  1f - Mathf.Exp(-response * Time.deltaTime));
            _acceleration = (moveVelocity - currentVelocity) / deltaTime;
            currentVelocity =  moveVelocity;
        }

        private void PerformJump(ref Vector3 currentVelocity)
        {
            _motor.ForceUnground(0f);
            var currentVerticalVelocity = Vector3.Dot(currentVelocity, _motor.CharacterUp);
            var targetVerticalVelocity = Mathf.Max(currentVerticalVelocity, _jumpForce);
            currentVelocity += _motor.CharacterUp * (targetVerticalVelocity -  currentVerticalVelocity);
        }
        
        private void SetCapsuleDimensionsByStance(CharacterStance stance)
        {
            var height = stance == CharacterStance.Stand ? _standHeight : _crouchHeight;
            _motor.SetCapsuleDimensions(_motor.Capsule.radius, height, height * 0.5f);
        }
        
        private float GetTargetCameraHeight()
        {
            var currentHeight = _motor.Capsule.height;
            return currentHeight * (_characterStance == CharacterStance.Stand 
                ? _standCameraTargetHeight : _crouchCameraTargetHeight);
        }
        
        private bool CanJump() => _requestedJumpInput && !_requestedCrouchInput;
        
        private bool IsOnGround() => _motor.GroundingStatus.IsStableOnGround;
        
        public bool IsColliderValidForCollisions(Collider coll) { return true; }
        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport) {  }
        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
            ref HitStabilityReport hitStabilityReport) { }
        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition,
            Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport) { }
        public void OnDiscreteCollisionDetected(Collider hitCollider) { }
        public void PostGroundingUpdate(float deltaTime) { }
    }
}
