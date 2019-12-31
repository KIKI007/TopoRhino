# Topolgical interlocking Creator for Rhino 

## Workflow
1. Compile **C++** Code into dynamic link library (.dll).
2. Use RhinoCommon SDK (C#) to import the C++ library.

## Installment for Windows

Step 0:
Install the newest version of Rhino 6 for Windows

Step 1:
To run the plugin, please make sure the visual c++ runtime enviroment is installed.You can find the runtime enviroment package in  "WinBin/VC_redist.x64.exe"

Step 2:
Copy all "*.dll, *ilk, *pdb" file in "WinBin" folder into "C:\Program Files\Rhino 6\System" (need administrator authority?

Step 3:
Drag "TopoGrasshopperWin.gha" and "TopoRhino.rhp" into Rhino6 Software Interface.

Step 4:
To run example, please have a look at "example" folder