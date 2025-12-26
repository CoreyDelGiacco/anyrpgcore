# AnyRPG

## Introduction

This project contains the code for the AnyRPG Engine.

AnyRPG is a Role Playing Game engine written in C# for Unity.

It is a 100% free and open source project with the goal of enabling content creators to rapidly create unique and engaging short stories, scenarios, adventures, and even full games.

It accomplishes this by providing a platform with the most common Role Playing Game functionality out of the box so that content creators only need to provide visual assets and story content.

## Links

### Web Site

http://www.anyrpg.org/

### Documentation

Documentation hosting is provided by Gitbook at https://docs.anyrpg.org/

### Patreon

https://www.patreon.com/anyrpg

### Download Unity Packages and Example Games

http://www.anyrpg.org/downloads/

### YouTube Channel

https://www.youtube.com/channel/UC-SiqAyRXR6eijPggFhFG2g

### Discord Server

https://discord.gg/huSAuqk

### Trello

https://trello.com/anyrpg/

### Twitter

https://twitter.com/AnyRPGEngine

## Setup

There are 3 ways to install AnyRPG, depending on your preference.  They are listed below.

### AnyRPG Engine Unity Package Installation

A Unity package that includes this repository, a complex sample game, and all dependencies pre-installed is available for download at http://www.anyrpg.org/downloads/

### AnyRPG Core Github Installation
A video tutorial for installing AnyRPG Core version 0.10.1a (and higher) from github can be watched here: https://www.youtube.com/watch?v=oMeWWgNYrYI

1. Install the correct Unity version to open this project.  The current project Unity version is 2021.3.28f1
1. Clone this repository into a directory on your computer
1. Open the Unity Hub and add the project
1. Open the project and install the following Unity packages:
	* Text Mesh Pro.  This package is installed by default.
	* UMA 2 from the Unity Asset Store (https://assetstore.unity.com/packages/3d/characters/uma-2-unity-multipurpose-avatar-35611) or github (https://github.com/umasteeringgroup/UMA/tree/master/UMAProject)
1. Open the Window menu in Unity and choose 'TextMeshPro' > 'Import TMP Essential Resources'
1. Open the UMA menu in Unity and choose Global Library.  In the Global Library window, choose File > Rebuild From Project

### AnyRPG Core Unity Asset Store Package Installation
AnyRPG Core is available on the Unity Asset Store at https://assetstore.unity.com/packages/slug/234361

A video tutorial for installing AnyRPG Core version 0.14.5 (and higher) from the Unity Asset Store can be watched here: https://youtu.be/syI3ohFWVck

A PDF file with instructions for installing AnyRPG Core from the Unity Asset Store can be found at [Unity Asset Store Package Setup Guide](Assets/AnyRPG/Unity%20Asset%20Store%20Package%20Setup%20Guide.pdf)

## Getting Started

Open the Tools menu in Unity and choose 'AnyRPG' > 'Welcome Window'

From the welcome window you can easily
* Find and open the included sample games
* Launch the New Game Wizard to setup your own game
* Find and open online support resources to get in depth help and information about using AnyRPG

## Triarch Vertical Slice (Step 2)

Triarch adds data-driven zone and death rule loading on startup. Data lives in `/data` and is mirrored to `Assets/StreamingAssets/TriarchData`.

Quickstart:
1. Open the project in Unity 2021.3.28f1.
2. Run `Tools > Triarch > Sync Data to StreamingAssets` to copy `/data` into `Assets/StreamingAssets/TriarchData`.
3. Create an empty GameObject and add `TriarchDataBootstrap`, `TriarchZoneService`, and `TriarchDeathService`.
4. Add a `TriarchPlayer` + `TriarchInventory` to your test player.
5. Add `TriarchZoneVolume` triggers in the scene and set their `zoneId` to `sanctum`, `frontier`, or `shatterwilds`.
6. Add `TriarchDebugUI` to any GameObject to see the current zone/risk and the “Kill Me” button.
7. Play the scene; entering zones updates risk tier and the debug kill button applies the zone death rule.
