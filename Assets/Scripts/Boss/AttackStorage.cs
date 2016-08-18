using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// Stores various values for the boss's attack sequence and 
/// </summary>
public class AttackStorage : MonoBehaviour {

	public List<int> stageOneAttacks, stageTwoAttacks, stageThreeAttacks, 
	stageFourAttacks, stageFiveAttacks = new List<int>();

	public bool canAttack;
}
