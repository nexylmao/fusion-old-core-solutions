# fusion-core
Application-base frontend side, C# library

Huh, let me try to explain this shortly...

Well, for now this is a C# library, it contains a few things that can be expanded on upon.
For now, it contains : 
	- Message and Log system for communication :
		- Messages for communicating between objects
		- Loggers for logging messages to LocalStorage or OnlineDatabase(to be implemented)
	- Dynamic database storing system that can determine which storing system you are using,
	for now works with REST API's and Local Storing.
	Generally, all storing goes with JSON's, and Databases (class) can be fully loaded from those,
	fully saved there or fully deleted
	- Assembly loading system, that can load already compiled .dll files, that should use
	Fusion namespace. Class library can contain classes that inherit either the IScript interface,
	which allows the class to have void Start() and void End() that will be executed in the program,
	or classes that will inherit the abstract class StorableObject, allowing the objects of the class
	to be stored in the Database<> 
	
Well, for now, how is this app run?
For now, I test it with a console app I have made on my local PC. It has a reference to fusion-core, and
in main it calls ApplicationBase.Start(), with a path to my REST API server (mine's https://fusion-backend.herokuapp.com/FusionAPI/)

If the database which you're targeting doesn't have a metadatafile (file that contains the list of paths to dll's that are next to the program)
you can do one of two things : 
	1. Call ApplicationBase.Start() with additional arguments, which are the paths to dll's, and 
	2. Watch the "DLLManager doesn't have what to load message in the logs"
	
This part still has some things that have to be touched up, but for this case it works like it's supposed to...

For now, small->large plans include 

	1. First, making args never get confused, the program will always need to know which args are path to database, and which ones are scripts/libraries
	
	2. Second, making it possible for program to compile a file in .cs format, so you don't have to compile it yourself, and making it more easier to fix for the end-user
	
	3. (DONE ALREADY WITH STATIC CLASS Integrated, Integrated messages were a mistake to be put in Database<>, because that created an infinite loop) Maybe the logs and messages could be a bit less hard-coded in. I went from one end (where i tried to load every message, sender and receiver online) to hardcoding them in...
	
	4. (OFFICIALY INHERITS HashSet<>, so that's done!) Database class needs alot of work, it's missing alot of methods/properties for it to be a IEnumerable level class with methods for saving the data...
	
	5. Maybe to see if all the IScript's can run in parallel in Load()? Also, Unload() should be added to the Finisher of DLLManager... That's an easy fix, god dammit...
	
	6. Next to runing in parallel, adding Update() and LateUpdate() to IScript, so that seriously could take advantage of that... WOW, AppDomains can do that i think...
	
	7. Also, at one point I'm gonna start adding much more useful stuff in here, ability to have a rendering engine, maybe replace the Newtonsoft.Json library (you're great guys, but it's not
	Fusion.Json :P)
	
	Ahh, am I a dreamer... How much times will I commit this README file tonight? Any bets?