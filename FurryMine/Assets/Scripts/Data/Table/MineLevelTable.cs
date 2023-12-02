using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class MineLevelTable : ScriptableObject
{
	public List<MineLevelEntity> Table; // Replace 'EntityType' to an actual type that is serializable.
}
