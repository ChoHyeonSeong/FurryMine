using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class EnforcePriceData : ScriptableObject
{
	public List<EnforcePriceEntity> Data; // Replace 'EntityType' to an actual type that is serializable.
}
