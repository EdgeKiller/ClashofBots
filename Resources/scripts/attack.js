/*
  File : attack.js
  Creator : EdgeKiller
  Info : Attack bases
  Last modification : 27-10-15
*/

//Log and set the bot's state to 'Attacking'
Bot.Log("Start attacking...");
Bot.botState = 4;

var hidemode = !(Config["game"]["hidemode"] == "True");

var sides = parseInt(Config["attack"]["sides"]);
var mode = parseInt(Config["attack"]["mode"]);

var topleftPoints = [
  {x:70,y:285},
  {x:127,y:247},
  {x:175,y:214},
  {x:218,y:178},
  {x:265,y:145},
  {x:318,y:108},
  {x:370,y:68}
];
var toprightPoints = [
  {x:740,y:290},
  {x:680,y:254},
  {x:640,y:220},
  {x:580,y:175},
  {x:535,y:142},
  {x:482,y:100},
  {x:432,y:68}
];
var bottomleftPoints = [
  {x:328,y:536},
  {x:270,y:490},
  {x:214,y:444},
  {x:166,y:408},
  {x:118,y:376},
  {x:74,y:344}
];
var bottomrightPoints = [
  {x:465,y:544},
  {x:524,y:502},
  {x:590,y:452},
  {x:645,y:416},
  {x:698,y:374},
  {x:747,y:334}
];

function randomInt(min,max)
{
    return Math.floor(Math.random()*(max-min+1)+min);
}

var attackPoints = [];

var screenshot = BotImage.CaptureRegion(37, 580, 734, 44, hidemode);

//Troops detection
var troopsLocation = [10];
var index;
for(i = 0; i < 10; i++){
  troopsLocation[i] = Bot.DetectTroop(i, screenshot);
}

if(Config["attack"]["topleft"] == "True"){
  attackPoints = attackPoints.concat(topleftPoints);
}
if(Config["attack"]["topright"] == "True"){
  attackPoints = attackPoints.concat(toprightPoints);
}
if(Config["attack"]["bottomleft"] == "True"){
  attackPoints = attackPoints.concat(bottomleftPoints);
}
if(Config["attack"]["bottomright"] == "True"){
  attackPoints = attackPoints.concat(bottomrightPoints);
}

//Mode 0 - Barb/Arch
if(mode == 0){
  var pixel;
  var pixel1;

  while(true){

    //Barbarians
    pixel = BotImage.GetWindowImage(hidemode).GetPixel(troopsLocation[0].X, troopsLocation[0].Y);
    if(pixel.R != pixel.G || pixel.R != pixel.B){
      BotMouse.SendClick(0, troopsLocation[0].X, troopsLocation[0].Y);
      for (index = 0; index < attackPoints.length; ++index) {
        pixel = BotImage.GetWindowImage(hidemode).GetPixel(troopsLocation[0].X, troopsLocation[0].Y);
        if(pixel.R != pixel.G || pixel.R != pixel.B){
          BotMouse.SendClick(0, attackPoints[index].x + randomInt(-5, 5), attackPoints[index].y + randomInt(-5, 5));
        }
        Bot.Wait(100);
      }
    }

    //Archers
    pixel1 = BotImage.GetWindowImage(hidemode).GetPixel(troopsLocation[1].X, troopsLocation[1].Y);
    if(pixel1.R != pixel1.G || pixel1.R != pixel1.B){
      BotMouse.SendClick(0, troopsLocation[1].X, troopsLocation[1].Y);
      for(index = 0; index < attackPoints.length; ++index){
        pixel1 = BotImage.GetWindowImage(hidemode).GetPixel(troopsLocation[1].X, troopsLocation[1].Y);
        if(pixel1.R != pixel1.G || pixel1.R != pixel1.B){
          BotMouse.SendClick(0, attackPoints[index].x + randomInt(-5, 5), attackPoints[index].y + randomInt(-5, 5));
        }
        Bot.Wait(100);
      }
    }

    //Check if archers & barbarians available
    if(pixel.R == pixel.G && pixel.R == pixel.B  && pixel1.R == pixel1.G && pixel1.R == pixel1.B){
      break;
    }
  }
}

//Log
Bot.Log("Troops deployment finished.");

//Wait for attack to finish
var returnHome;
while(true)
{
  returnHome = BotImage.GetWindowImage(hidemode).GetPixel(370, 550);
  if(returnHome.R > 85 && returnHome.R < 97 && returnHome.B > 13 && returnHome.B < 20){
    break;
  }
  Bot.Wait(1000);
}

//Log and set the bot's state to 'IDLE'
Bot.Log("Attacking complete, returning home.");
Bot.botState = 1;

//Read gains
var goldWin = parseInt(Bot.Read("w", BotImage.CaptureRegion(313, 293, 100, 24, hidemode)));
var elixirWin = parseInt(Bot.Read("w", BotImage.CaptureRegion(313, 329, 100, 24, hidemode)));
var darkWin = 0;
var trophyWin = 0;
var darkPixel = BotImage.GetWindowImage(hidemode).GetPixel(430, 380);
if(darkPixel.R > 68 && darkPixel.R < 80 && darkPixel.B > 76 && darkPixel.B < 90){ //Dark elixir
  darkWin = parseInt(Bot.Read("w", BotImage.CaptureRegion(323, 364, 90, 24, hidemode)));
  trophyWin = parseInt(Bot.ReadTrophy(BotImage.CaptureRegion(363, 398, 50, 24, hidemode)));
}
else{ //No dark elixir
  trophyWin = parseInt(Bot.ReadTrophy(BotImage.CaptureRegion(363, 364, 50, 24, hidemode)));
}

//Log
Bot.Log("Gains • [G] : " + goldWin + " [E] : " + elixirWin, " [D] : " + darkWin + " [T] : " + trophyWin);

//Click return home button
BotMouse.SendClick(0, 400, 540);

//Wait for village loading
while(true)
{
  returnHome = BotImage.GetWindowImage(hidemode).GetPixel(20, 370);
  if(returnHome.R > 200 && returnHome.R < 220 && returnHome.B > 18 && returnHome.B < 28){
    break;
  }
  Bot.Wait(500);
}
