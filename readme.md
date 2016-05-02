# PhotoGeoTag

Add Exif Geo Tag to photos

## Requirements

#### Common

1. [GMap.Net](http://greatmaps.codeplex.com) ( or download via NuGet) for web map display
2. [ypmap](https://code.google.com/archive/p/ypmap/) (can not download source code direct, maybe export it to github then download it), Get AMap/SoSo map provider code
3. Microsoft .net framework v4.5 (maybe 4.0, but not tested)

#### PhotoGeoTag

1. [ExplorerTree](http://www.codeproject.com/Articles/14570/A-Windows-Explorer-in-a-user-control) for display filesystem treeview
2. [ImageListView](http://www.codeproject.com/Articles/43265/ImageListView) for display images thumbnail list

#### PhotoGeoTagShell

1. [Windows-API-Code-Pack-1.1](https://github.com/aybe/Windows-API-Code-Pack-1.1), not recommanded download from NuGet, it can not add ExplorerBrowser WinForm control to toolbox for now version 1.1.2. When you clone source and recompiling it, everything is good.
 1. https://www.nuget.org/packages/WindowsAPICodePack-Core/
 2. https://www.nuget.org/packages/WindowsAPICodePack-Shell/
 3. https://www.nuget.org/packages/WindowsAPICodePack-ShellExtensions/
 4. https://www.nuget.org/packages/WindowsAPICodePack-DirectX/
 5. https://www.nuget.org/packages/WindowsAPICodePack-ExtendedLinguisticServices/
 6. https://www.nuget.org/packages/WindowsAPICodePack-Sensors/

## Usage
1. Open application
2. Navigating to you photo folder
3. Select photo(s) and click `View Map` button to open MapViewer window
4. Default map provider is google china, you can change it, and auto saved this setting
5. If selected photo(s) has EXIF GEO Tag, it will auto adding marker(s) to map, these markers may changing via mouse drag
6. If photo no GEO Tag, you can click blue marker button, pin photo to map, drap marker to change position, after release mouse left button, it will prompt you "Yes/No", 
 1. if selectd "Yes", will auto set GEO Tag to photo(s) and try touch the photo datetime with photo's EXIF DateTaken Tag
 2. it selected "No", you can continued drag marker to changing position

## Download
#### Binary
[bitbucket.org](https://bitbucket.org/netcharm/photogis/downloads)

#### Source
[bitbucket.org](https://bitbucket.org/netcharm/photogis)
[github.com](https://github.com/netcharm/PhotoGIS)

## To-Do
Load GPX/KML file and display on map as reference of photo(s) geo location

