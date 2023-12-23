using UnityEngine;
using Unity.VisualScripting;
using Cysharp.Threading.Tasks;
using System.Collections;
using GLTFast;
using VisualScriptingNodes;
using UnityEngine.Networking;
using System;

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
            UniTask.Create(async () => { gltfInstance = await LoadGlbWithURL(url); }).Forget();
            yield return new WaitUntil(() => gltfInstance);
            resultValue = gltfInstance.gameObject;

            if (Utils.IsVisionOS()) Utils.ChangeShadersWithTexture(resultValue, "Universal Render Pipeline/Lit", "baseColorTexture", "_BaseMap");

            yield return outputTrigger;
        }

        private async UniTask<GameObject> LoadGlbWithURL(string URL)
        {
            GameObject gltfInstance = new("glTFast");
            byte[] GlbBytes = null;
            UnityWebRequest request = UnityWebRequest.Get(URL);
            await request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                GlbBytes = request.downloadHandler.data;

                var gltf = new GltfImport();
                bool success = await gltf.LoadGltfBinary(
                    GlbBytes,
                    // The URI of the original data is important for resolving relative URIs within the glTF
                    new Uri(URL)
                    );
                if (success)
                {
                    success = await gltf.InstantiateMainSceneAsync(gltfInstance.transform);
                }
            }
            return gltfInstance;
        }


        private async UniTask<GameObject> LoadGltfFunc(string URL)
        {
            GameObject gltfInstance = new("glTFast");
            var gltf = gltfInstance.AddComponent<GltfAsset>();
            gltf.Url = URL;
            while (!gltf.IsDone) await UniTask.Yield();

            //Wait until gltf objects are loaded
            if (gltfInstance.transform.childCount > 0)
            {
                while (gltfInstance.transform.GetChild(0).name == "New Game Object")
                {
                    await UniTask.WaitForFixedUpdate();
                    await UniTask.Yield();
                }
            }
            return gltfInstance;
        }






    }
}