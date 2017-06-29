# Rotator Block Dev Log

## 6 June
Basic test code (TestGrid.cs) to check the ability to create 100+ blocks at startup. Its not much to look at, but I needed to be sure if could be done.

![Alt](devlog-170606.gif) 
Its not much to see, a mass of random tiles appearing

(Tech note to self: http://www.screentogif.com/ seems a great tool for animated gif capture, lets have at least one per log)

Tech notes:
* Tile-Sprite set at 32 pixels per unit in Unity makes a tile 1x1
* Camera is size 7 which gives 14*4/3= 18.66 tiles x 14 tiles, which is enough for the 2 grids of 9x11

## 25 June
Looking at tech challenge. How to hold the map?  Simplest idea, use Unity json and test. The code only supports a subset of json (1D array & List only, no dictionary, no 2D arrays), but we can use that. Also managed to load the asset from a .json file stored in the project as a simple TextAsset. Therefore I can use an array or TextAssets for levels, or I can use resource directories.

Also using an idea from http://answers.unity3d.com/questions/40568/base64-encodedecoding.html to use the System's built in base64 encoding to obfuscate data. This is a tech I can drop in later if I need to.

Ran rough test of loading code, almost worked except for the Unity +y is up vs +y is down in arrays.

![Alt](devlog-172506.gif) 
Almost right, the json and the screen.

Going to take a little more work to get is 100% correct on the screen, but the idea is good.

Next challenge: detecting clicks, and rotating the block about.

## 26 June
Focus for today is the rotating blocks.
Going to ignore the input for now, just work on the switching blocks.

Rather then actually moving the blocks about (which will get messy), instead change the grid of integers around and then update the block sprites to follow this.

Idea looks like a GridModel which holds the grid and a GridView which displays it.  Animation will be on the view, but the actual updating of grid will be on the model (not having a controller).  Inputs can be detected at view level and send out as events.
No actual work done, just a bit of thinking for today.

