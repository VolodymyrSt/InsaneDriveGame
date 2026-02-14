using UnityEngine;

namespace _Project.Code.Configs.Character
{
    [CreateAssetMenu(fileName = "Character Config", menuName = "Configs/Character")]
    public class CharacterConfigSO : ScriptableObject
    {
        [Header("Speed")]
        public float WalkSpeed;
        public float SprintSpeed = 9;
        public float CrouchSpeed;
        public float AirSpeed = 15f;
        
        [Header("Response")]
        public float WalkResponse = 20f;
        public float SprintResponse = 30f;
        public float CrouchResponse = 20f;
        public float CrouchHeightResponse = 15f;
        
        [Header("Jump")]
        public float JumpForce;
        
        [Header("StanceHeight")]
        public float StandHeight = 2f;
        public float CrouchHeight = 1f;
        
        [Header("CameraStanceHeight")]
        public float StandCameraTargetHeight = 0.9f;
        public float CrouchCameraTargetHeight = 0.7f;
        
        [Header("Air")]
        public float AirAcceleration = 70f;
        
        [Header("Interaction")]
        public float InteractionDistance = 5f;
    }
}