
	Some notes I write for myself for development,
	I'll try to put all thoughts here, just so I don't get to a point like before, where I get lost in my own code.

	So, day is 12/26 : 
		- Making two classes -> __BASEOBJECT and __DATABASE
			I plan on using them, for :
			__BASEOBJECT as the class that will derive 'object', but other classes will derive from, it'll have some fields for identification,
			and also a key of some sort, that __DATABASE can use for dynamic loading.
			__DATABASE as the class that will keep all the data of type, and will also know how to load everything, or partially through something
			i will call dynamic loading for now. What will it do, is basically just load parts of the database, it has an array of items, that it
			will store the loaded items. It'll have some kind of refresh cycle, that will say that stuff that's very far away from our current calculated
			key, it will unload those, and load some that it finds that are close, or in the 'range' as we could say.
			Could also implement something that will try to see based on how much memory the game can get, to load that many items.

		- For now, i decide to just put in a placeholder file, that will have some empty classes that i will implement at some point in the future, ones that 
		are generally needed to make a game, not just the core itself.

		Now, for __BASEOBJECT, i plan on having two fields, one notifying the category of the file, based on it's type (class) and one noting the ID inside that class.
		Classes that i will use, should be listed in some kind of a metafile. That is the first file that should be loaded inside of the program, and all it's data in
		somekind of a global variable class, and there a list of Types should be made. Maybe, the category ID should be stored with them, because i don't want the program
		generating them (well, not every time, maybe only the first time). Now, there should be some static fields keeping that in __BASEOBJECT.

		Now, how should I keep the ID's ? Now, i need to keep in mind that they should be writable to export. String? Uint? I think string would be cool, and then uint for
		particular item in the database.

		Key, 100% must be some kind of a number thing, i really don't want to do complicated maths with alphanumberical strings... Maybe hexadecimal, just to seem kinda cooler XD.
		(And again, junkie joke...)

		How do i pass through the file that will have metadata to the program? Through string[] args? I really need a playground console, just to see how to structure out
		what i need to right now, to see if i can pass through the path to the __METAFILE, and how can i make it? Do i need to take in string into him, or do i put in Types[].

		First, in the program, I'll set up the Return class... - OKAY, I'LL ACTUALLY SET THAT FOR LATER, AND JUST CHANGE METHODS TO LATER RETURN 'Return'

		Making some progress on _METADATA file, i had managed to make alot of stuff
		Okay, progress on __METADATA : 
			1. It has a class inside, called __METADATA_BIT, that serves a single bit of information it holds, it keeps the Type, and two strings, one for path to the database, and one as
			category ID.
			2. An array of just named is kept in the static class __METADATA
			3. There's a set of methods, that allows the work with just named, stuff as LOAD, SAVE (for now only in .json file, sent through the string[] args), import new bit, delete bit (by index or values)
			and also ToString() for writing __METADATA out.
			?? I really should test stuff right now, so yeah...
			Okay, so i learned alot of things... The JSON.net is much better than the shit that microsoft gives, so yay...
			My __METADATA file finally works, so it can be made/read, but it only works if the classes it reads, do exist in the program, so it won't work with any classes
			that don't exist, or the program can't find, so that's a concern. i still need to find a way of loading assemblies, and stuff like that...

			Okay, so i think i figured out loading assemblies, and making __METADATA files with classes from other .dll's 
			I in my playgroundconsole, made a classlibrary called Item, it has 3 fields, and 2 constructors (one empty, one with parameters)
			Compiled the .dll, and in the console i pulled the reference to __METADATA (from fusion.dll) and Item (from Item.dll)
			added that to the __METADATA file, copied the file and .dll to PublicTestConsole, and it makes an instance of Item, COOOOOL
			That means that i am able to load classes from other files, yay, which means that i can kinda separate my core dll from other files, which is neat.
			That is a goal im trying to achieve, making my core and my classes separated and modular, so i can update only one .dll (on example if my Item's aren't working as they're supposed to, i can just update those, without touching everything else)
			I plan on standardizing for myself that everything should have like a start or end method, just so i have a way of starting stuff from other classes.
			Or, maybe even making a separate assembly that controls all of the other ones, and uses them...
			Okay, i may be starting to complicate life for myself right now, but i'm having alot of learning experiences while doing it, so this is also neat :)

			I think i am gonna compile everything for now, and move my playground console to GitHub

			AAA AM I HAVING FUN NERDING OUT OVER IDEAS THAT ARE HITTING ME RIGHT NOW!

			{
				[
					".\Item.dll","others..."
				]
			}
			,
			{
				.. regular __METADATA_BITS
			}
