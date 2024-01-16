# glTFast Visual Scripting Nodes

## Unity Visual Scripting node library for glTF/glb 3d models

<img width="563" alt="Duck" src="https://github.com/from2001/glTFast_VisualScriptingNodes/assets/387880/771ee302-6c38-4a2c-9457-364af34e99f3">

<img width="1555" alt="glTFastVisualScriptingNodes" src="https://github.com/from2001/glTFast_VisualScriptingNodes/assets/387880/2169bbb4-cd70-4c69-a156-66b2b583753c">


### Features

- Load glTF/glb models with URL

## Install Option A: via OpenUPM command-line interface

```shell
# Install openupm-cli
npm install -g openupm-cli

# Go to your unity project directory
cd YOUR_UNITY_PROJECT_DIR

# Install package:
openupm add com.from2001.gltfast-visualscripting-nodes
```

## Install Option B: via Unity package manager

### 1. Setup scoped registories

Open "Edit - Project Settings - Package Manager" on your Unity project.

Add Scoped Registories and click "Apply".

Name: `OpenUPM`
URL: `https://package.openupm.com`
Scopes:
`com.cysharp.unitask`
`com.from2001.gltfast-visualscripting-nodes`
`com.openupm`

![Project Settings](https://github.com/from2001/glTFast_VisualScriptingNodes/assets/387880/dd5a5f9c-47fc-421c-b262-c27702ce882b)

### 2. Install glTFast Visual Scripting Node Package with Package Manager

![Package Manager](https://github.com/from2001/glTFast_VisualScriptingNodes/assets/387880/73f12fe8-164a-4774-9e76-27771b447186)

## How to Use

Notice: Check "Coroutine" in the "On Start Event" triger node.

## Samples

1. Load glb model
2. Load glb model and rotate

![Samples](https://github.com/from2001/glTFast_VisualScriptingNodes/assets/387880/db01dee5-401a-4ee4-9d00-6981ed37754c)

## ToDo

- ~~Make usage samples~~
- ~~VisionOS Support~~
- implement Cache feature
- ~~add target Gameobject as an input port~~
- ~~add size adjustment option~~

## Repositories

- [GitHub](https://github.com/from2001/glTFast_VisualScriptingNodes/)
- [OpenUPM](https://openupm.com/packages/com.from2001.gltfast-visualscripting-nodes/)

## Others

### Vision OS Support

Materials are generated with UnityGLTF routine with `UnityGLTF/PBRGraph` shader on ViisonOS until glTFast shaders support Vision OS.

### Avoid Multiple scripted importers error

If you want to use [glTFast Visual Scripting Nodes](https://openupm.com/packages/com.from2001.gltfast-visualscripting-nodes/) and [VRM Visual Scripting Nodes](https://openupm.com/packages/com.from2001.vrm-visualscripting-nodes/) in a same project, add two Scripting Define Symbols in `Project Settings` > `Player` > `Other Settings` > `Script Compilation` > `Scripting Define Symbols`

`UNIGLTF_DISABLE_DEFAULT_GLTF_IMPORTER`
`UNIGLTF_DISABLE_DEFAULT_GLB_IMPORTER`
