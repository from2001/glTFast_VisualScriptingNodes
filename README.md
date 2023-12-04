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

<!--
![Project Settings](https://github.com/from2001/VRM_VisualScriptingNodes/assets/387880/595b1d91-4435-4195-9b6d-1ca6b43113ce)
-->

### 2. Install glTFast Visual Scripting Node Package with Package Manager

<!--
![Package Manager](https://github.com/from2001/VRM_VisualScriptingNodes/assets/387880/2809ed0b-61a8-47d9-bdb0-24335ac60163)
-->

## How to Use

Notice: Check "Coroutine" in the "On Start Event" triger node.

## ToDo

- Make usage samples
- implement Cache feature
