using UnityEngine;
using Unity.VisualScripting;
using Cysharp.Threading.Tasks;
using System.Collections;
using GLTFast;
using VisualScriptingNodes;

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
            UniTask.Create(async () => { gltfInstance = await LoadGltfFunc(url); }).Forget();
            yield return new WaitUntil(() => gltfInstance);
            resultValue = gltfInstance.gameObject;

            if (Utils.IsVisionOS()) Utils.ChangeShadersWithTexture(resultValue, "Universal Render Pipeline/Lit", "baseColorTexture", "_BaseMap");

            yield return outputTrigger;
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