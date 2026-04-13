# Lancer
Main repo for a prototype game in Unity. Proptype assets are based on the TTRPG "Lancer" by Massif Press.

# Features
    1. Third Person Character Controller
    2. Displacement Shader
    3. Sensor / NavMesh Logic 
    3. Scriptable StateMachine

## Third Person Character Controller
Modified my default controller to work with Unity's new input system.

## ShaderGraphs
    1. Displacement Shader
    2. Gradient Terrain Shader

## Sensor / NavMesh Logic
ISensor and Sensor provide an easy component for LOS and NavMesh navigation. NavMeshEntity uses these components to represent humanoid NPCs.

## Scriptable Statemachine
Improved normal StateMachine by making it run on ScriptableObjects, making NPC behavior far more scalable then before.