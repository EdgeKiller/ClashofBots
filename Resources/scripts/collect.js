/*
  File : collect.js
  Creator : EdgeKiller
  Info : Collect resources
  Last modification : 14-10-15
*/

//Log and set the bot's state to 'Collecting'
Bot.Log("Start collecting...");
Bot.botState = 5;

//Get collectors positions from config file
var collectors = [];
for (i = 1; i <= 17; i++){
  collectors[i] = {x:parseInt(Config["collectors"]["collector" + i].split(":")[0]), y:parseInt(Config["collectors"]["collector" + i].split(":")[1])};
}

//Click on each collector
for (i = 1; i <= 17; i++){
  if(collectors[i].x != -1 && collectors[i].y != -1){
    BotMouse.SendClick(0, collectors[i].x, collectors[i].y);
    Bot.Wait(250);
  }
}

//Click on the top left corner to unselect collector
BotMouse.SendClick(0, 0, 0);

//Log and set the bot's state to 'IDLE'
Bot.Log("Collecting complete.");
Bot.botState = 1;
