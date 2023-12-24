using UnityEngine;
using Unity.VisualScripting;
using Cysharp.Threading.Tasks;
using System.Collections;
using GLTFast;
using VisualScriptingNodes;
using UnityEngine.Networking;
using System;
using UnityEditor;
using System.IO;

namespace GltfastVisualScriptingNodes
{
    [UnitShortTitle("Load glTF/glb")]
    [UnitTitle("Load glTF/glb")]
    [UnitCategory("Gltf")]
    [UnitSubtitle("Load glTF/glb with URL")]
    public class LoadGltf : Unit
    {
        [DoNotSerialize]
        public ControlInput inputTrigger;

        [DoNotSerialize]
        public ControlOutput outputTrigger;

        [DoNotSerialize]
        public ValueInput glTF_URL;

        [DoNotSerialize]
        public ValueOutput result;

        private GameObject resultValue;
        protected override void Definition()
        {
            inputTrigger = ControlInputCoroutine("inputTrigger", Enter);
            outputTrigger = ControlOutput("outputTrigger");

            glTF_URL = ValueInput<string>("glTF/glb URL", "");
            result = ValueOutput<GameObject>("Game Object", (flow) => resultValue);
        }

        private IEnumerator Enter(Flow flow)
        {
            string url = flow.GetValue<string>(glTF_URL);
            GameObject gltfInstance = null;
            UniTask.Create(async () =>
            {
                gltfInstance = await LoadGltfWithURL(url);
            }).Forget();
            yield return new WaitUntil(() => gltfInstance);
            resultValue = gltfInstance.gameObject;
            yield return outputTrigger;
        }

        private async UniTask<GameObject> LoadGltfWithURL(string URL)
        {
            GameObject gltfInstance = new("glTFast");
            byte[] GlbBytes = null;
            UnityWebRequest request = UnityWebRequest.Get(URL);
            await request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                GlbBytes = request.downloadHandler.data;
                GLTFast.Materials.IMaterialGenerator materialGenerator = Utils.IsVisionOS() ? new PBRGraphMaterialGenerator(new MemoryStream(GlbBytes)) : null;
                var gltfImport = new GltfImport(null, null, materialGenerator, null);
                var instantiator = new GameObjectInstantiator(gltfImport, gltfInstance.transform);

                bool success = await gltfImport.LoadGltfBinary(
                    GlbBytes,
                    new Uri(URL)
                    );
                if (success)
                {
                    success = await gltfImport.InstantiateMainSceneAsync(instantiator);
                    if (success)
                    {
                        var legacyAnimation = instantiator.SceneInstance.LegacyAnimation;
                        if (legacyAnimation != null)
                        {
                            legacyAnimation.Play();
                        }
                    }
                }
            }
            return gltfInstance;
        }

    }
}