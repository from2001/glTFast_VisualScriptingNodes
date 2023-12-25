# Vision OS support with UnityGLTF routine

glTFast shaders are currently not supported VisionOS, but UnityGLTF shaders work on VisionOS. I implemented glTFast material generator with UnityGLTF routine.

UnityGLTF is imported from the following fork repository, not from KhronosGroup. prefrontalcortex version is not registered on OpenUPM, so the files are directory located in this repository.

https://github.com/prefrontalcortex/UnityGLTF
1.24.0-pre.2

In VisionOS build setting, glTFast Materials will be generated with
/Runtime/Utils/PBRGraphMaterialGenerator.cs
which uses UnityGLTF routine.

Code modification
