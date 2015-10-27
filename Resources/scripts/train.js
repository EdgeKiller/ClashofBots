/*
  File : train.js
  Creator : EdgeKiller
  Info : Train troops
  Last modification : 16-10-15
*/

//Unit's case's location
var unitCaseLocation = [
  {x:222,y:344}, // Barbarian
  {x:322,y:344}, // Archer
  {x:422,y:344}, // Giant
  {x:522,y:344}, // Goblin
  {x:622,y:344}, // Wallbreaker
  {x:214,y:444}, // Baloon
  {x:314,y:444}, // Wizard
  {x:414,y:444}, // Healer
  {x:514,y:444}, // Dragon
  {x:614,y:444} // Pekka
];

//Barrack's case's location
var barrackCaseLocaton = [
  {x:240,y:530},
  {x:296,y:530},
  {x:352,y:530},
  {x:408,y:530}
];

var hidemode = !(Config["game"]["hidemode"] == "True");

while(true){
  //Log and set the bot's state to 'Training'
  Bot.Log("Start training...");
  Bot.botState = 2;
  BotMouse.SendClick(0, 50, 510);
  Bot.Wait(250);
  var color, unit; //Variables declaration
  for (i = 0; i <= 3; i++){
    color = BotImage.GetWindowImage(hidemode).GetPixel(barrackCaseLocaton[i].x, barrackCaseLocaton[i].y);
    if(color.R >= 135 && color.R <= 137){
      Bot.Wait(250);
      BotMouse.SendClick(0, barrackCaseLocaton[i].x, barrackCaseLocaton[i].y);
      Bot.Wait(250);
      unit = parseInt(Config["troops"]["barrack" + (i + 1)]);
      if(unit != -1){
        while(true){
          color = BotImage.GetWindowImage(hidemode).GetPixel(unitCaseLocation[unit].x, unitCaseLocation[unit].y);
          if(color.R == color.G && color.R == color.B){ break; } //Exit if barrack full
          BotMouse.SendClick(0, unitCaseLocation[unit].x, unitCaseLocation[unit].y);
          Bot.Wait(5);
        }
      }
    }
    Bot.Wait(250);
  }
  //Log and set the bot's state to 'IDLE'
  Bot.Log("Training complete.");
  Bot.botState = 1;
  BotMouse.SendClick(0, barrackCaseLocaton[0].x, barrackCaseLocaton[0].y);
  Bot.Wait(250);
  color = BotImage.GetWindowImage(hidemode).GetPixel(366, 173);
  if(color.R >= 215 && color.R <= 217){
    break;
  }
  else {
    Bot.Log("Wait for full army..."); //Waiting for full army
    Bot.botState = 1;
    BotMouse.SendClick(0, 0, 0); //Click on the top left corner to unselect barrack
    Bot.Wait(30000); //Wait 30 seconds
  }
}

Bot.Wait(250);
BotMouse.SendClick(0, 0, 0); //Click on the top left corner to unselect barrack
Bot.Wait(250);
