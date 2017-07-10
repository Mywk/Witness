# Witness
Home Automation System using REST for Windows 10 IOT

This project is what I use at my place to control several relays via a Web server with a Raspberry PI3 running Windows 10 IOT.

Features:
- Relay control
- Timed relay control
- Lights control (MiLight bridge)

Planned features:
- YouTube Playlist
- Voice control (probably going with Cortana for this one)

Currently working on:
- Alarm clock

Planned for later (if someone else wants to help with this project):
- Plugins (for example instead of a static MiLight bridge controller, use a plugin instead)
- Login and authentication
- Embedded remote control for desktop (mouse, window view, cmd line, shutdown/sleep/etc..)
- Add/Remove relays in the webpage instead of statically in the code


Notes:
MiLight combo-mode is a way to use two MiLight receivers (groups) to control both the RGB and W at the same time, it works by setting the first receiver with the Red->Red and Green->Green and the second one with Blue->Blue White->Green. 


Libraries/NuGet packages used:

	[MIT License]
	Metro UI CSS, JQuery, Restup, sqlite-net-pcl, SQLite for Universal Windows Platform, Font Awesome, datetimepicker by Chupurnov Valeriy, iro.js

Don't expect the commits to be very detailed.
Want to collaborate? Just send me a message using the website below!

Author: Mywk @ www.TechCoders.Net © 2017