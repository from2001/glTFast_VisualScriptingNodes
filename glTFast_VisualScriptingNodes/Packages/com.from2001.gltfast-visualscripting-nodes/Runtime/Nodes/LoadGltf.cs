using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System.Collections;
using GLTFast;
using System;
using STYLY.Http;
using STYLY.Http.Service;
using UnityGLTF;

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
        public ValueInput TargetGameobject;

        [DoNotSerialize]
        public ValueInput NormalizeScale;

        [DoNotSerialize]
        public ValueOutput result;

        private GameObject resultValue;
        private GameObject loadingIcon;
        
        private ThrobberRotator throbberRotator;
        protected override void Definition()
        {
            inputTrigger = ControlInputCoroutine("inputTrigger", Enter);
            outputTrigger = ControlOutput("outputTrigger");

            glTF_URL = ValueInput<string>("glTF/glb URL", "");
            TargetGameobject = ValueInput<GameObject>("Target Game Object", null);
            NormalizeScale = ValueInput<bool>("Normalize Scale", false);
            result = ValueOutput<GameObject>("Game Object", (flow) => resultValue);
        }

        private IEnumerator Enter(Flow flow)
        {
            string url = flow.GetValue<string>(glTF_URL);
            GameObject target = flow.GetValue<GameObject>(TargetGameobject);
            bool adjustScale = flow.GetValue<bool>(NormalizeScale);
            GameObject gltfInstance = null;

            ShowLoadingIcon(target);
            
            UniTask.Create(async () =>
            {
                gltfInstance = await LoadGltfWithURL(url, target, adjustScale);
            }).Forget();
            yield return new WaitUntil(() => gltfInstance);

            HideLoadingIcon();
            
            resultValue = gltfInstance;
            yield return outputTrigger;
        }

        /// <summary>
        /// Takes care of the loading icon creation and calls the animation accordingly
        /// </summary>
        /// <param name="target"></param>
        private void ShowLoadingIcon(GameObject target)
        {
            loadingIcon = new GameObject("LoadingIcon");
            Canvas canvas = loadingIcon.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            CanvasScaler canvasScaler = loadingIcon.AddComponent<CanvasScaler>();
            canvasScaler.dynamicPixelsPerUnit = 10;

            // Add an Image component to serve as the loading icon
            GameObject icon = new GameObject("Icon");
            icon.transform.SetParent(loadingIcon.transform);
            Image image = icon.AddComponent<Image>();

            // Load the throbber sprite from Resources, the image file can be changed easily. 
            Sprite throbberSprite = Resources.Load<Sprite>("throbber");
            if (throbberSprite != null)
            {
                image.sprite = throbberSprite;
            }
            else
            {
                Debug.LogWarning("Throbber sprite not found in Resources/throbber");
                image.color = Color.white; // Fallback color if the sprite is not found
            }

            // Set the size of the loading icon
            RectTransform rectTransform = image.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(0.5f, 0.5f); // Set size of the icon in world units. I decided to put it in 0.5 so it is not intrusive

            // Position the loading icon based on the target object
            if (target != null)
            {
                // Calculate bounds of the target object
                Bounds bounds = GetTargetBounds(target);
                loadingIcon.transform.position = bounds.center;
                rectTransform.anchoredPosition = Vector2.zero; // Center the icon
            }

            // Add ThrobberRotator component and start the throbber animation
            throbberRotator = loadingIcon.AddComponent<ThrobberRotator>();
            throbberRotator.Initialize(icon.transform);
        }

        /// <summary>
        /// Receives the location of the target model to put the loading icon on top of it
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        private Bounds GetTargetBounds(GameObject target)
        {
            Renderer[] renderers = target.GetComponentsInChildren<Renderer>();
            if (renderers.Length == 0)
            {
                return new Bounds(target.transform.position, Vector3.one);
            }

            Bounds bounds = renderers[0].bounds;
            foreach (Renderer renderer in renderers)
            {
                bounds.Encapsulate(renderer.bounds);
            }
            return bounds;
        }

        /// <summary>
        /// Hide the loading icon after the gltf is shown
        /// </summary>
        private void HideLoadingIcon()
        {
            if (loadingIcon != null)
            {
                if (throbberRotator != null)
                {
                    throbberRotator.StopRotation();
                }
                GameObject.Destroy(loadingIcon);
            }
        }



        /// <summary>
        /// Load glTF/glb with URL using UnityGLTF
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="target"></param>
        /// <param name="normalizescale"></param>
        /// <returns></returns>
        private async UniTask<GameObject> LoadGltfWithURL(string URL, GameObject target = null, bool normalizescale = true)
        {
            // Load completed flag
            bool loadCompleted = false;

            // Create glTF GameObject
            GameObject glTF = new("glTF");
            var UnityGltfScript = glTF.AddComponent<GLTFComponent>();

            // Set glTF loading parameters
            UnityGltfScript.GLTFUri = URL;
            UnityGltfScript.Multithreaded = true;
            UnityGltfScript.UseStream = true;
            UnityGltfScript.AppendStreamingAssets = false;
            UnityGltfScript.PlayAnimationOnLoad = true;
            UnityGltfScript.HideSceneObjDuringLoad = false;
            UnityGltfScript.Factory = null;
            UnityGltfScript.onLoadComplete = () => loadCompleted = true;

            // Wait until the glTF is loaded
            await UniTask.WaitUntil(() => loadCompleted);

            // Adjust scale to 1 unit size
            if (normalizescale) Utils.FitToUnitSize(glTF);

            // Set glTF location to Target Game Object
            if (target != null)
            {
                glTF.transform.SetParent(target.transform);
                glTF.transform.localPosition = Vector3.zero;
                glTF.transform.localRotation = Quaternion.identity;
                glTF.transform.localScale = Vector3.one;
            }

            return glTF;
        }

        // This method is not used now.
        private async UniTask<GameObject> LoadGltfWithURL_glTFast(string URL, GameObject target = null, bool normalizescale = true)
        {
            GameObject glTF = new("glTF"); // This is the parent object of the glTF instance. Always 1 unit size.
            GameObject gltfInstance = new("glTFast");
            glTF.SetActive(false);
            gltfInstance.transform.SetParent(glTF.transform);
            byte[] GlbBytes = null;
            try
            {
                HttpResponse httpResponse = await Http.Get(URL)
                                .UseCache(CacheType.UseCacheAlways)
                                .OnError(response => Debug.Log(response.StatusCode))
                                .SendAsync();
                if (httpResponse.IsSuccessful)
                {
                    GlbBytes = httpResponse.Bytes;

                    // Use glTfast materials since PBRGraphMaterialGenerator is broken.
                    // GLTFast.Materials.IMaterialGenerator materialGenerator = Utils.IsVisionOS() ? new PBRGraphMaterialGenerator(new MemoryStream(GlbBytes)) : null;
                    GLTFast.Materials.IMaterialGenerator materialGenerator = null;

                    var gltfImport = new GltfImport(null, null, materialGenerator, null);
                    var instantiator = new GameObjectInstantiator(gltfImport, gltfInstance.transform);

                    bool success_load = await gltfImport.LoadGltfBinary(
                        GlbBytes,
                        new Uri(URL)
                        );
                    if (success_load)
                    {
                        bool success_instantiniate = await gltfImport.InstantiateMainSceneAsync(instantiator);
                        if (success_instantiniate)
                        {
                            // Adjust scale to 1 unit size
                            if (normalizescale) Utils.FitToUnitSize(gltfInstance);

                            // Set glTF location to Target Game Object
                            if (target != null)
                            {
                                glTF.transform.SetParent(target.transform);
                                glTF.transform.localPosition = Vector3.zero;
                                glTF.transform.localRotation = Quaternion.identity;
                                glTF.transform.localScale = Vector3.one;
                            }

                            // Play animation
                            var legacyAnimation = instantiator.SceneInstance.LegacyAnimation;
                            if (legacyAnimation != null) legacyAnimation.Play();
                        }
                        else
                        {
                            Debug.LogError("Failed to instantiate glTF/glb from URL: " + URL);
                            UnityEngine.Object.Destroy(glTF);
                            return null;
                        }
                    }
                    else
                    {
                        Debug.LogError("Failed to LoadGltfBinary glTF/glb from URL: " + URL);
                        UnityEngine.Object.Destroy(glTF);
                        return null;
                    }
                }
                else
                {
                    Debug.LogError("Failed to download glTF/glb from URL: " + URL);
                    UnityEngine.Object.Destroy(glTF);
                    return null;
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                Debug.LogError("Failed to download glTF/glb from URL: " + URL);
                UnityEngine.Object.Destroy(glTF);
                return null;
            }
            glTF.SetActive(true);
            return glTF;
        }
    }
}