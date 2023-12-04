# glTFast Visual Scripting Nodes

## Unity Visual Scripting node library for glTF/glb 3d models

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

- Make usage samples
- implement Cache feature

## Repositories

- [GitHub](https://github.com/from2001/glTFast_VisualScriptingNodes/)
- [OpenUPM](https://openupm.com/packages/com.from2001.gltfast-visualscripting-nodes/)
