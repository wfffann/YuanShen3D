using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YuanShenImpactMovementSystem
{
    [CreateAssetMenu(fileName = "Player", menuName = "Custom/Characters/Player")]
    public class PlayerSO : ScriptableObject
    {
        [field: SerializeField] public PlayerGroundedData playerGroundedData {  get; private set; }
        [field: SerializeField] public PlayerAirborneData playerAirborneData {  get; private set; }

    }
}
