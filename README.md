🎮 Unity Rendering Demo
A Unity rendering demo project built for interview and portfolio purposes, focusing on real-time rendering techniques and client-side graphics implementation.

📌 Project Overview
This project demonstrates intermediate-to-advanced rendering techniques in Unity using URP (Universal Render Pipeline).
It is designed to showcase practical understanding of:

Real-time rendering pipeline
Post-processing effects
Shader-based visual effects
Modular rendering architecture

✨ Features
✅ Object Highlight (selection feedback)
✅ Outline Effect (custom shader, multi-pass)
✅ Bloom (URP Volume post-processing)
✅ Emission workflow (HDR-based glow)
🔄 Ongoing: Rendering optimization & performance tuning

🧠 Technical Highlights
Rendering
Based on Unity URP
Understanding of:
Forward Rendering Pipeline
Post-processing stack (Volume system)
HDR & Bloom interaction
Emission lighting model
Shader
Shader written in ShaderLab / HLSL
Multi-pass rendering (Outline extrusion technique)
Vertex offset for silhouette rendering
Architecture
Clear separation between:
Scene logic
Rendering logic
Modular demo structure for easy extension

🏗️ Tech Stack
Unity 2022.3 LTS
URP (Universal Render Pipeline)
C#
ShaderLab / HLSL

🚀 How to Run
Open project with Unity 2022.3.x
Load main demo scene
Click Play
Interact with objects to see highlight / outline effects

📸 Showcase

Demo screenshots
GIF recordings (highlight / outline / bloom)

📈 Development Roadmap
 Base project setup (URP)
 Highlight interaction system
 Outline rendering implementation
 Bloom & emission integration
 Rendering performance optimization
 GPU instancing / batching exploration
 More advanced effects (e.g. rim light / dissolve)
 
📂 Project Structure (Simplified)
Assets/
 ├── Scripts/
 ├── Shaders/
 ├── Materials/
 ├── Scenes/
 └── Resources/
Packages/
ProjectSettings/

🎯 Purpose

This project is built as a technical portfolio to demonstrate:
Rendering fundamentals in Unity
Shader implementation ability
Code organization & engineering thinking
Continuous iteration and version control practices

🔗 Repository Usage
This repository is actively maintained and updated during development.
Each feature is committed incrementally with clear commit messages to reflect engineering workflow.

👤 Author
Name: Zhangshu
Background: Game Client Developer (Unity / Rendering Focus) 
