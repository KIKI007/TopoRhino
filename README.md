# Topolgical interlocking Creator for Rhino 

## Workflow
1. Compile **C++** Code into dynamic link library (.dll).
2. Use RhinoCommon SDK (C#) to import the C++ library.

## For Windows Users

**Step 0**:

Install the newest version of Rhino 6 for Windows

**Step 1**:

To run the plugin, please make sure the visual c++ runtime enviroment is installed.You can find the runtime enviroment package in  "WinBin/VC_redist.x64.exe"

**Step 2**:

Copy all "*.dll, *ilk, *pdb" file in the "WinBin" folder into "C:\Program Files\Rhino 6\System" (need administrator authority)

**Step 3**:

Open Rhino 6 and drag "WinBin/TopoGrasshopperWin.gha" and "WinBin/TopoRhino.rhp" into the Software Interface.

**Step 4**:

The example folder has 3 grasshopper scripts to explain the usage of this tool.
