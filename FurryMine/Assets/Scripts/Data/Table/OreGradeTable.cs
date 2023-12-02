using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class OreGradeTable : ScriptableObject
{
	public List<OreGradeEntity> Table; // Replace 'EntityType' to an actual type that is serializable.
}
