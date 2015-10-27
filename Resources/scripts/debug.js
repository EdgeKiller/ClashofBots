/*
  File : debug.js
  Creator : EdgeKiller
  Info : Script for debugging
  Last modification : 27-10-15
*/

var image = Bot.BmpFromFile("img1.png");

var gold = parseInt(Bot.ReadTrophy(image));

Bot.Log(gold);
