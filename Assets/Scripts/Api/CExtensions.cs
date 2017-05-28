using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CExtensions
{
	public static Vector3 X0Z(this Vector3 v)
	{
		return new Vector3 (v.x, 0, v.z);
	}


}
