﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Entity Data", menuName = "Entity Data")]

public class EntityData : ScriptableObject
{
	public Tile spriteTile;
	public new string name;
	public bool blocksMovement;
	public Stats stats;
	public RogueAI ai;
}
