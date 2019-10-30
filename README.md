# SampleNativeLibrary
RhinoCommon cross-platform native library sample

## Overview

This sample demonstrates how to create a RhinoCommon plug-in that utilizes functions from a native C++ library.

## Building Sample

### Windows

To build the sample on Windows, you are going to need:

* Rhino 6 for Windows (WIP, http://discourse.mcneel.com/)
* Microsoft Visual C# 2013
* Microsoft Visual C++ 2013

To build both the Rhino plug-in and the native library, load the **SampleNativeLibrary.sln** solution in Visual Studio 2013. Then, click **Build -> Build Solution**. Note, you might want to unload the **SampleRhino.Mac** project before building. You can do this by right-clicking on the project and clicking **Unload project**.

### Mac

To build the sample on OS X, you are going to need:

* Rhino 5 for Mac (5.1 or later)
* Xamarin Studio 5.9 (or later)
* Apple Xcode 6.4 (or later) and command-line tools.

On OS X, building the .NET plug-in and the native library are technically separate processes.  For convenience purposes, this sample includes a **build_native.py** called from a Custom Command in the SampleRhino.Mac csproj.  This build script does the building of the native library automatically before the .NET plug-in is built.

#### Step-by-Step

1. Open the **SampleNativeLibrary.sln** solution in Xamarin Studio.
1. Unload both the **SampleLibrary** and **SampleRhino.Win** projects before building. You can do this by right-clicking on each project and clicking **Unload**.
1. Click **Build** > **Build All**.

#### Behind The Scenes

The **SampleRhino.Mac** contains a number of Custom Commands that perform necessary steps to properly build and use the native library.  (You can find these Custom Commands by double-clicking the **SampleRhino.Mac** project in the Solution Explorer in Xamarin Studio and navigating to **Build** > **Custom Commands**).  

First, **build_native.py** is called.  This step builds the **libSampleLibrary.dylib** using the xcodebuild command line tools.  The same result can be achieved by building the **SampleLibrary.xcodeproj** in Xcode.  

Second, a crucial file called **SampleRhino.dll.config** is copied into the plug-in folder.  This config file tells the .NET dll where to look for the dylib.  For more information on the config file, see the [Mono: Interop with Native Libraries](http://www.mono-project.com/docs/advanced/pinvoke/) guide.  

Third, and finally, the **libSimpleLibrary.dylib** is copied into the plug-in build folder after the build has finished.

## Related Topics

* [MSDN Platform Invoke Tutorial](https://msdn.microsoft.com/en-us/library/aa288468(VS.71).aspx)
* [Mono: Interop with Native Libraries](http://www.mono-project.com/docs/advanced/pinvoke/)

## Legal Stuff
Copyright © 2015 Robert McNeel & Associates. All Rights Reserved.

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software.

THIS SOFTWARE IS PROVIDED "AS IS" WITHOUT EXPRESS OR IMPLIED WARRANTY. ALL IMPLIED WARRANTIES OF FITNESS FOR ANY PARTICULAR PURPOSE AND OF MERCHANTABILITY ARE HEREBY DISCLAIMED.

Rhinoceros is a registered trademark of Robert McNeel & Associates.
