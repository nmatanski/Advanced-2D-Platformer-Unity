using UnityEngine;
using System.Collections.Generic;

namespace Anima2D
{
    public interface IRectManipulatorData
	{
		List<Vector3> normalizedVertices { get; set; }
	}
}
