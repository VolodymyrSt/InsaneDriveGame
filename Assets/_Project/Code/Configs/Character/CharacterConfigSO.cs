using UnityEngine;

namespace _Project.Code.Configs.Character
{
    [CreateAssetMenu(fileName = "Character Config", menuName = "Configs")]
    public class CharacterConfigSO : ScriptableObject
    {
        public float Speed;
        public float JumpHeight;
    }
}