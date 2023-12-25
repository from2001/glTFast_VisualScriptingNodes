﻿using GLTF.Schema;
using GLTF.Schema.KHR_lights_punctual;
using UnityEngine;
using LightType = UnityEngine.LightType;

namespace UnityGLTF
{
	public partial class GLTFSceneImporter
	{
		private void ConstructLights(GameObject nodeObj, Node node)
		{
			// TODO this should be handled by the lights extension directly, not here
			const string lightExt = KHR_lights_punctualExtensionFactory.EXTENSION_NAME;
			KHR_LightsPunctualNodeExtension lightsExtension = null;
			if (_gltfRoot.ExtensionsUsed != null && _gltfRoot.ExtensionsUsed.Contains(lightExt) && node.Extensions != null && node.Extensions.ContainsKey(lightExt))
			{
				lightsExtension = node.Extensions[lightExt] as KHR_LightsPunctualNodeExtension;
				var l = lightsExtension.LightId;

				var light = l.Value;

				var newLight = nodeObj.AddComponent<Light>();
				switch (light.Type)
				{
					case GLTF.Schema.KHR_lights_punctual.LightType.spot:
						newLight.type = LightType.Spot;
						break;
					case GLTF.Schema.KHR_lights_punctual.LightType.directional:
						newLight.type = LightType.Directional;
						break;
					case GLTF.Schema.KHR_lights_punctual.LightType.point:
						newLight.type = LightType.Point;
						break;
				}

				newLight.name = light.Name;
				newLight.intensity = (float) light.Intensity / Mathf.PI;
				newLight.color = new Color(light.Color.R, light.Color.G, light.Color.B, light.Color.A);
				newLight.range = (float) light.Range;
				if (light.Spot != null)
				{
#if UNITY_2019_1_OR_NEWER
					newLight.innerSpotAngle = (float) light.Spot.InnerConeAngle * 2 / (Mathf.Deg2Rad * 0.8f);
#endif
					newLight.spotAngle = (float) light.Spot.OuterConeAngle * 2 / Mathf.Deg2Rad;
				}

				// flip?
				nodeObj.transform.localRotation *= Quaternion.Euler(0, 180, 0);
			}
		}
	}
}
