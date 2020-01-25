using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Rogue Tile Data", menuName = "Rogue Tile Data")]
public class RogueTileData : ScriptableObject
{
	public Tile spriteTile;
	public bool blocksMovement;
	public bool blocksVision;
}
