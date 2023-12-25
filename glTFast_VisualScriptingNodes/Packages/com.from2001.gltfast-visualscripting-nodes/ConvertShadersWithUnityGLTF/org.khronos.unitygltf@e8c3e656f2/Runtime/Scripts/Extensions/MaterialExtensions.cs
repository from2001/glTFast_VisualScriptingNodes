using System.Collections.Generic;
using GLTF.Schema;
using UnityEngine;
using UnityGLTF.Extensions;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityGLTF
{
	public static class MaterialExtensions
	{
#if UNITY_EDITOR
		[InitializeOnLoadMethod]
#endif
		[RuntimeInitializeOnLoadMethod]
		static void InitExt()
		{
			GLTFSceneExporter.AfterMaterialExport += GLTFSceneExporterOnAfterMaterialExport;
		}

		private static readonly int thicknessTexture = Shader.PropertyToID("thicknessTexture");
		private static readonly int thicknessFactor = Shader.PropertyToID("thicknessFactor");
		private static readonly int attenuationDistance = Shader.PropertyToID("attenuationDistance");
		private static readonly int attenuationColor = Shader.PropertyToID("attenuationColor");
		private static readonly int ior = Shader.PropertyToID("ior");
		private static readonly int transmissionFactor = Shader.PropertyToID("transmissionFactor");
		private static readonly int transmissionTexture = Shader.PropertyToID("transmissionTexture");

		private static readonly int iridescenceFactor = Shader.PropertyToID("iridescenceFactor");
		private static readonly int iridescenceIor = Shader.PropertyToID("iridescenceIor");
		private static readonly int iridescenceThicknessMinimum = Shader.PropertyToID("iridescenceThicknessMinimum");
		private static readonly int iridescenceThicknessMaximum = Shader.PropertyToID("iridescenceThicknessMaximum");
		private static readonly int iridescenceTexture = Shader.PropertyToID("iridescenceTexture");
		private static readonly int iridescenceThicknessTexture = Shader.PropertyToID("iridescenceThicknessTexture");

		private static readonly int specularFactor = Shader.PropertyToID("specularFactor");
		private static readonly int specularColorFactor = Shader.PropertyToID("specularColorFactor");
		private static readonly int specularTexture = Shader.PropertyToID("specularTexture");
		private static readonly int specularColorTexture = Shader.PropertyToID("specularColorTexture");

		private static readonly int clearcoatFactor = Shader.PropertyToID("clearcoatFactor");
		private static readonly int clearcoatTexture = Shader.PropertyToID("clearcoatTexture");
		private static readonly int clearcoatRoughnessFactor = Shader.PropertyToID("clearcoatRoughnessFactor");
		private static readonly int clearcoatRoughnessTexture = Shader.PropertyToID("clearcoatRoughnessTexture");
		private static readonly int clearcoatNormalTexture = Shader.PropertyToID("clearcoatNormalTexture");


		private static void GLTFSceneExporterOnAfterMaterialExport(GLTFSceneExporter exporter, GLTFRoot gltfroot, Material material, GLTFMaterial materialnode)
		{
			if (!material) return;

			var usesTransmission = material.IsKeywordEnabled("_VOLUME_TRANSMISSION_ON");
			var usesVolume = material.HasProperty("_VOLUME_ON") && material.GetFloat("_VOLUME_ON") > 0.5f;
			var hasIor = material.HasProperty(ior) && !Mathf.Approximately(material.GetFloat(ior), KHR_materials_ior.DefaultIor);

			if (hasIor)
			{
				if (materialnode.Extensions == null)
					materialnode.Extensions = new Dictionary<string, IExtension>();

				var vi = new KHR_materials_ior();
				if (materialnode.Extensions.TryGetValue(KHR_materials_ior_Factory.EXTENSION_NAME, out var vv1))
					vi = (KHR_materials_ior) vv1;
				else
					materialnode.Extensions.Add(KHR_materials_ior_Factory.EXTENSION_NAME, vi);

				exporter.DeclareExtensionUsage(KHR_materials_ior_Factory.EXTENSION_NAME, false);

				if (material.HasProperty(ior))
					vi.ior = material.GetFloat(ior);
			}

			if (usesTransmission)
			{
				if (materialnode.Extensions == null)
					materialnode.Extensions = new Dictionary<string, IExtension>();

				exporter.DeclareExtensionUsage(KHR_materials_transmission_Factory.EXTENSION_NAME, false);

				// if the material already has an extension, we should get and modify that
				var vt = new KHR_materials_transmission();

				if (materialnode.Extensions.TryGetValue(KHR_materials_transmission_Factory.EXTENSION_NAME, out var vv2))
					vt = (KHR_materials_transmission) vv2;
				else
					materialnode.Extensions.Add(KHR_materials_transmission_Factory.EXTENSION_NAME, vt);

				if (material.HasProperty(transmissionFactor))
					vt.transmissionFactor = material.GetFloat(transmissionFactor);
				if (material.HasProperty(transmissionTexture) && material.GetTexture(transmissionTexture))
					vt.transmissionTexture = exporter.ExportTextureInfo(material.GetTexture(transmissionTexture), nameof(transmissionTexture));
			}

			if (usesVolume)
			{
				if (materialnode.Extensions == null)
					materialnode.Extensions = new Dictionary<string, IExtension>();

				exporter.DeclareExtensionUsage(KHR_materials_volume_Factory.EXTENSION_NAME, false);

				// if the material already has an extension, we should get and modify that
				var ve = new KHR_materials_volume();

				if (materialnode.Extensions.TryGetValue(KHR_materials_volume_Factory.EXTENSION_NAME, out var vv0))
					ve = (KHR_materials_volume)vv0;
				else
					materialnode.Extensions.Add(KHR_materials_volume_Factory.EXTENSION_NAME, ve);

				if (material.HasProperty(thicknessFactor))
					ve.thicknessFactor = material.GetFloat(thicknessFactor);
				if (material.HasProperty(thicknessTexture) && material.GetTexture(thicknessTexture))
					ve.thicknessTexture = exporter.ExportTextureInfo(material.GetTexture(thicknessTexture), nameof(thicknessTexture));
				if (material.HasProperty(attenuationDistance))
					ve.attenuationDistance = material.GetFloat(attenuationDistance);
				if (material.HasProperty(attenuationColor))
					ve.attenuationColor = material.GetColor(attenuationColor).ToNumericsColorRaw();
			}

			if (material.IsKeywordEnabled("_IRIDESCENCE_ON"))
			{
				exporter.DeclareExtensionUsage(KHR_materials_iridescence_Factory.EXTENSION_NAME, false);

				if (materialnode.Extensions == null)
					materialnode.Extensions = new Dictionary<string, IExtension>();

				var vir = new KHR_materials_iridescence();

				if (materialnode.Extensions.TryGetValue(KHR_materials_iridescence_Factory.EXTENSION_NAME, out var vv0))
					vir = (KHR_materials_iridescence) vv0;
				else
					materialnode.Extensions.Add(KHR_materials_iridescence_Factory.EXTENSION_NAME, vir);

				if (material.HasProperty(iridescenceFactor))
					vir.iridescenceFactor = material.GetFloat(iridescenceFactor);
				if (material.HasProperty(iridescenceIor))
					vir.iridescenceIor = material.GetFloat(iridescenceIor);
				if (material.HasProperty(iridescenceThicknessMinimum))
					vir.iridescenceThicknessMinimum = material.GetFloat(iridescenceThicknessMinimum);
				if (material.HasProperty(iridescenceThicknessMaximum))
					vir.iridescenceThicknessMaximum = material.GetFloat(iridescenceThicknessMaximum);
				if (material.HasProperty(iridescenceTexture) && material.GetTexture(iridescenceTexture))
					vir.iridescenceTexture = exporter.ExportTextureInfo(material.GetTexture(iridescenceTexture), nameof(iridescenceTexture));
				if (material.HasProperty(iridescenceThicknessTexture) && material.GetTexture(iridescenceThicknessTexture))
					vir.iridescenceThicknessTexture = exporter.ExportTextureInfo(material.GetTexture(iridescenceThicknessTexture), nameof(iridescenceThicknessTexture));
			}

			if (material.IsKeywordEnabled("_SPECULAR_ON"))
			{
				exporter.DeclareExtensionUsage(KHR_materials_specular_Factory.EXTENSION_NAME, false);

				if (materialnode.Extensions == null)
					materialnode.Extensions = new Dictionary<string, IExtension>();

				var vir = new KHR_materials_specular();

				if (materialnode.Extensions.TryGetValue(KHR_materials_specular_Factory.EXTENSION_NAME, out var vv0))
					vir = (KHR_materials_specular) vv0;
				else
					materialnode.Extensions.Add(KHR_materials_specular_Factory.EXTENSION_NAME, vir);

				if (material.HasProperty(specularFactor))
					vir.specularFactor = material.GetFloat(specularFactor);
				if (material.HasProperty(specularColorFactor))
					vir.specularColorFactor = material.GetColor(specularColorFactor).ToNumericsColorRaw();
				if (material.HasProperty(specularTexture) && material.GetTexture(specularTexture))
					vir.specularTexture = exporter.ExportTextureInfo(material.GetTexture(specularTexture), nameof(specularTexture));
				if (material.HasProperty(specularColorTexture) && material.GetTexture(specularColorTexture))
					vir.specularColorTexture = exporter.ExportTextureInfo(material.GetTexture(specularColorTexture), nameof(specularColorTexture));
			}

			if (material.IsKeywordEnabled("_CLEARCOAT_ON"))
			{
				exporter.DeclareExtensionUsage(KHR_materials_clearcoat_Factory.EXTENSION_NAME, false);

				if (materialnode.Extensions == null)
					materialnode.Extensions = new Dictionary<string, IExtension>();

				var cc = new KHR_materials_clearcoat();

				if (materialnode.Extensions.TryGetValue(KHR_materials_clearcoat_Factory.EXTENSION_NAME, out var vv0))
					cc = (KHR_materials_clearcoat) vv0;
				else
					materialnode.Extensions.Add(KHR_materials_clearcoat_Factory.EXTENSION_NAME, cc);

				if (material.HasProperty(clearcoatFactor))
					cc.clearcoatFactor = material.GetFloat(clearcoatFactor);
				if (material.HasProperty(clearcoatTexture))
					cc.clearcoatTexture = exporter.ExportTextureInfo(material.GetTexture(clearcoatTexture), nameof(clearcoatTexture));
				if (material.HasProperty(clearcoatRoughnessFactor))
					cc.clearcoatRoughnessFactor = material.GetFloat(clearcoatRoughnessFactor);
				if (material.HasProperty(clearcoatRoughnessTexture))
					cc.clearcoatRoughnessTexture = exporter.ExportTextureInfo(material.GetTexture(clearcoatRoughnessTexture), nameof(clearcoatRoughnessTexture));
				if (material.HasProperty(clearcoatNormalTexture))
					cc.clearcoatNormalTexture = exporter.ExportTextureInfo(material.GetTexture(clearcoatNormalTexture), nameof(clearcoatNormalTexture));


			}
		}
	}
}
