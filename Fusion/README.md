# Fusion
My game project, don't look it's shit.

Work in progress.

10/5/2017 - Finally uploaded what I had already done, the base solution from Visual Studio. 6 Files in the first part of the program:
	-	Ability.cs (with classes Ability, Buff(s), Debuff(s), AbilityList - for writing to database, and enum Ability_Type)
	- 	Item.cs (with classes Stats, Item, ItemList - for writing to database, GearSlots, Bag, and enum GearSlot)
	- 	Levels.cs (handles character level and exp)
	- 	playerClass.cs (handles playerClasses and Specs, has ClassList for writing to database)
	- 	Combat.cs (classes needed for combat system like CombatField, CharacterInCombat and Fight)
	- 	Core.cs (class that's going to be the base of the game)
	
10/8/2017 - Finally started doing work on having EventHandler, and IOHandler. Started filling in STRINGS class, with static strings. 
The name of the string should be like an argument to IOHandler.Push(). The function, should be the part of the external program, which
should decide how the string should be presented to the user. If _GAMELOAD_BAD shows up, it should tell the IOHandler to show the string, and
turn off the program, tell that something like the program is corrupted, and you should reinstall/some shit like that.
Will implement using nameof() function. 
I'm adding some functions to like playerClass proportion of CharacterInfo (renamed Character) - changing playerClass, swapping abilities, and resolving gearslot
that don't equate.
Moving more functions to character(), making it much more easier to call functions.

10/13/2017 - To do list for next week :
- Add crit mechanic to abilities, and armor mechanic to abilities (DONE)
- Need to work still around character, how im gonna implement NPC's. 
- Need to implement shit for RNG, and finishing up combat. (RNG - DONE)

10/16/2017 - happy birthday bro
Decided I'm gonna do some rework on Character.cs
- spliting it up, in like Character, NPC : Character, Player : Character,
and InCombat that has either a reference to a NPC, or a Player. Gonna make shit easier
to do later, with loottables, and abilitysequences. Also, adding other stuff.
After everything is done, gotta make like two EventHandlers, one for player to control their character,
one for PC to controll NPC's.

10/17/2017 - Rebuild everything, even if it's still not done.
Added LootTables. - Dictionary<Item, float> that keeps the base of items that an NPC can drop, and the chance of item dropping in the float.
Has a bit of functions, like AddItem(Item i, float f), ChangeItemChance(Item i, float newchance), DeleteItem(Item i), and DropItem()
DropItems() is a function, that return a list of Items. List<Item> should be shown by the IOHandler in whatever handles output of this game.
IOHandler should call up EventHandler when an Item is selected, to be done something with.

Did some stuff for Core.cs, added some EventHandlers i made (one for NPC), global npc list, and some other shitfucks.

-- 10/18 gotta fix all the nameof(string) shit, whereever i return string.Format, gotta do it before i return. Nameof() in IOHandler.Push() will fuck shit up!

10/21/2017 - strings (probably) fixed. Added some things to NPC's. Tried doing some work on the IOHandler, and failed extremely, soooo.
I gotta first finish up NPC (AbilitySequencer) and finish Character. Gotta finish those classes, so I can get onto controlling, and stuff like that.
Probably gonna have to rework some classes/methods. Stuff like Stats, and probably gonna add alot of methods there. And, Lists prob need some more work.

10/22/2017
 - Added class ReturnValue, for argumentning IOHandler.Push(), cause nameof() in Push() doesn't work, and it seems like there is no other way to get the name of
 an argument in the method. 
 - Added class CONSTANTS in Constants.cs, will serve for putting in constant numbers, and also formulas, such as Calculating health for players, and stuff like 
 health to string. 
 - Fixed up some minor things in methods, around items, around NPC's creating in InCombat, and alot of also small stuff.
 - Also, added one huge file (<60MB) with calculatedhealthvalues, up to 600k+ with survivability rating in steps of 25 from 0 to 100!
 - Right now, the console app writes and saves health values up to 100. Just used it to write the data. Will serve for other stuff later.
 
10/23/2017
Found a solution for that thing with IOHandler, made a class in Console, containing everything Fusion_External should have, and edited in the software where this is
added to.
Solution is made very easily. Method is placed in IOHandler.Push(), it calls an static Actions<ReturnValue> that is found in the class. That value can be edited out of the
file, for any other .cs file to change the function. There's a class, called IOHandlerModification, that has a method, that contains what the new Push() should contain,
and a method that should be called up on the start of the program to apply the changes to push.

	- Prob gonna tackle some other stuff to, later. (ME SUPERCHARGED WITH BOOSTER LUL)
	
10/24/2017
Okay, i'm getting a bit slow recently, a bit tired too.
I really need to jump into doing Character, Player and NPC. Every function, if nothing, has to atleast have a header. Need to have eventhandlers for those, accessed through the global one (OFC)
Player one attached to the playing player, NPC to which ever computer decides it need to do work on. After that, I should structure out combat. Turns, the main component of those, should be made.
Found big problem in the calculatedhealthvalues file. There's alot of values for which the into string converter is snapping, meaning that it is showing a lower value.
It's probably my fault for not covering values up to that high. Probably gonna redo it up to the original maximum value, 65535! So, I'm gonna do that for tonight probably, and that's it. SLEEEEEP!

10/26/2017 kinda late, i'm tired....
Starting to get back into doing this. I'm more thinking about everything, less writing, which I don't like that much. I gotta start writing down notes.
Okay, back to progress:
- Done CharacterList, EventHandler and everything.
- Was throughout adding some strings, fixing up constant number with GearSlots, and also fixed some functions to use ReturnValue.
- Added some methods in Player, cause i really need to be done with Player really, really soon. I gotta do that, then head onto the AbilitySequencer for NPC's.
That is going to be a class that will probably tell the IOHandler to cast abilities for an NPC. How am I gonna do that? We'll have to see. I really don't know at this point.
Probably will have to implement it with like an ability_ID list. After that, I have to jump onto doing combat. TurnHandler, Turns in general need to be done. I have to think about doing 
range, turns, ability casting, stuff like that. Just if you forget (cause I know I sometimes get retarded), turn based combat, "inspiried" by South Park : The Stick Of Truth (the one I can
run on my shitty laptop!)

That needs to be done fast, so I can jump into making a database creating software. Console program, that is supposed to make the databases. Will probably use a GameStarter() without all the functions.
Yeah, and school is really cool these 2 days :)