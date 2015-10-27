/*
  File : search.js
  Creator : EdgeKiller
  Info : Search bases
  Last modification : 26-10-15
*/

//Log and set the bot's state to 'Searching'
Bot.Log("Start searching...");
Bot.botState = 3;

var hidemode = !(Config["game"]["hidemode"] == "True");

BotMouse.SendClick(0, 60, 600);
Bot.Wait(250);
BotMouse.SendClick(0, 200, 500);
Bot.Wait(500);
var color = BotImage.GetWindowImage(hidemode).GetPixel(320, 380);
if(color.R >= 239 && color.R <= 241){ // Shield verification
	BotMouse.SendClick(0, 500, 400);
	Bot.Wait(250);
}

var i = 1, next = true, gold = 0, elixir = 0, dark = 0, trophy = 0;
while(true)
{
	//Wait find opponent
	while(BotImage.GetWindowImage(hidemode).GetPixel(50, 535).R != 192){
		Bot.Wait(10);
	}
	Bot.Wait(50);

	//Variable reset
 	next = true;
 	gold = 0;
 	elixir = 0;
 	dark = 0;
 	trophy = 0;

	gold = parseInt(Bot.Read("g", BotImage.CaptureRegion(48, 88, 90, 17, hidemode))); //Read gold
	elixir = parseInt(Bot.Read("e", BotImage.CaptureRegion(48, 115, 90, 17, hidemode))); //Read elixir

	if(Boolean(Config["search"]["bgold"]) && gold < parseInt(Config["search"]["gold"])){ //Gold filter
		next = false;
	}

	if(Boolean(Config["search"]["belixir"]) && elixir < parseInt(Config["search"]["elixir"])){ //Elixir filter
		next = false;
	}

	if(BotImage.GetWindowImage(hidemode).GetPixel(40, 140).R < 5){
		dark = parseInt(Bot.Read("d", BotImage.CaptureRegion(48, 140, 62, 17, hidemode))); //Read dark elixir
		trophy = parseInt(Bot.Read("t", BotImage.CaptureRegion(48, 180, 25, 17, hidemode))); //Read trophy
	}else{
		trophy = parseInt(Bot.Read("t", BotImage.CaptureRegion(48, 153, 25, 17, hidemode))); //Read trophy
	}

	if(Boolean(Config["search"]["bdark"]) && dark < parseInt(Config["search"]["dark"])){ //Dark elixir filter
		next = false;
	}

	if(Boolean(Config["search"]["btrophy"]) && trophy < parseInt(Config["search"]["trophy"])){ //Trophy filter
		next = false;
	}

	Bot.Log(i + " • [G] : " + gold + " [E] : " + elixir + " [D] : " + dark + " [T] : " + trophy); //Log the console

	if(next){ break; }

	Bot.Wait(2000);

	BotMouse.SendClick(0, 700, 500); //Click on next
	Bot.Wait(250);
	i++;
}

Bot.Log("Village found !");
if(Boolean(Config["search"]["alert"])){ //Sound alert
	System.Console.Beep(300, 250);
}
