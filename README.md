# 🎯 Fast Break Tech Lead Take-Home Test
https://theiterativeco.notion.site/fast-break-tech-test?pvs=73

## 🏀 Task

We’ve set up a simple Unity project for you. Your task is to build the basics of a **2v2 local multiplayer arcade basketball prototype**.


## ✅ Requirements

- Implement a **2 vs 2 basketball setup**:
    - **Movement**: Use WASD and/or Arrow Keys to control players
    - **Pass**: Press one button to pass the ball to your teammate
    - **Shoot**: Press another button to shoot
- Only **2 players are controllable, both on the same team.** The other 2 opponents are static, nothing needs to be implemented there.
- Controls:
    - Player 1: WASD to move, ‘**E’ to pass**, ‘**R’ to shoot**
    - Player 2: Arrow Keys to move, ‘**O’ to pass**, ‘**P’ to shoot**
- The game should:
    - Spawn all 4 players in a court scene
    - Allow passing and shooting between teammates
    - Print logs when passes or shots happen


## 📦 What You Get

A Unity starter project with:

- A simple court scene
- A placeholder player prefab (e.g. capsule with Rigidbody)
- A ball prefab
- Core script stub
    - `Scripts/Core/ServiceLocator.cs`
    - `Scripts/Core/ServiceInitializer.cs`
    - `Scripts/Core/IService.cs`
- Camera setup

**Fork this project.**
https://github.com/TheIterativeCo/Fast-Break-Tech-Test


## 🧑‍💻What you need to do

- Implement `Scripts/Services/GameManagerService` as a service (plain C# class implementing `IService`).
- Implement `Scripts/Gameplay/PlayerController` and `Scripts/Gameplay/BallController` as MonoBehaviour scripts, to be attached to the player and ball prefabs.
- Register and initialize `GameManagerService` using the provided `ServiceLocator` and `ServiceInitializer`.
- Demonstrate passing and shooting between two controllable players, with logs for each action.
- Focus on clear code structure and integration with the provided architecture.
- Ensure the project is bug-free.

## 📝 What to Submit

1. A GitHub link to your updated Unity project.
2. A short **Technical Design Document** describing:
    - How your code is structured as if you were explaining to a junior.
    - How you'd extend it (and break it down into individual tasks) to support the following:
        - Full shooting logic
        - Score tracking
        - Basic AI
        - Online multiplayer


## 🔍 What We’re Evaluating

| Area | What We Look For |
| --- | --- |
| Code Quality | Clear, modular, maintainable |
| Input Handling | Scalable and understandable |
| Game Logic | Clean, responsive, and bug-free |
| Communication | Clear thinking and explanation of trade-offs |
