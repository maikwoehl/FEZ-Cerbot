FEZ-Cerbot
==========

Reverse engineered FEZ-Cerbot Project files for .NETMF 4.3 and the newest Gadgeteer SDK

Install
-------
1. Download [FEZCerbot.msi](https://github.com/daemonTutorials/FEZ-Cerbot/blob/master/FEZCerbot/FEZCerbot/FEZCerbot/bin/Release/Installer/FEZCerbot.msi)
2. Download [CerbotController.msi](https://github.com/daemonTutorials/FEZ-Cerbot/blob/master/CerbotController/CerbotController/CerbotController/bin/Release/Installer/CerbotController.msi)
3. Install both applications
4. Now you can start using FEZCerbot in Visual Studio 2012 and with .NETMF 4.3


Usage
-----

After you installed the files above and created a new FEZCerbot-Project, you can simply use FEZCerbot the same as other FEZ-Mainboards. 
For example, type the following to turn on the Debug-LED:

```C#
Mainboard.SetDebugLed(true);
```
