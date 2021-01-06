function UnityProgress(gameInstance, progress) {
	if (!gameInstance.Module)
	return;
	if(!gameInstance.background){
	gameInstance.background = document.createElement("div");
	gameInstance.background.className = "background";
	gameInstance.container.appendChild(gameInstance.background);
	}
	if(!gameInstance.message1){
	gameInstance.message1 = document.createElement("div");
	gameInstance.message1.className = "message1";
	gameInstance.container.appendChild(gameInstance.message1);
	}
	if(!gameInstance.message2){
	gameInstance.message2 = document.createElement("div");
	gameInstance.message2.className = "message2";
	gameInstance.container.appendChild(gameInstance.message2);
	}
	if(!gameInstance.message3){
	gameInstance.message3 = document.createElement("div");
	gameInstance.message3.className = "message3";
	gameInstance.container.appendChild(gameInstance.message3);
	}
  //if (!gameInstance.logo) {
    //gameInstance.logo = document.createElement("div");
    //gameInstance.logo.className = "logo " + gameInstance.Module.splashScreenStyle;
    //gameInstance.container.appendChild(gameInstance.logo);
  //}
  if (!gameInstance.progress) {    
    gameInstance.progress = document.createElement("div");
    gameInstance.progress.className = "progress " + gameInstance.Module.splashScreenStyle;
    gameInstance.progress.empty = document.createElement("div");
    gameInstance.progress.empty.className = "empty";
    gameInstance.progress.appendChild(gameInstance.progress.empty);
    gameInstance.progress.full = document.createElement("div");
    gameInstance.progress.full.className = "full";
    gameInstance.progress.appendChild(gameInstance.progress.full);
    gameInstance.container.appendChild(gameInstance.progress);
  }
  gameInstance.progress.full.style.width = (100 * progress) + "%";
  gameInstance.progress.empty.style.width = (100 * (1 - progress)) + "%";
  if (progress == 1){
	gameInstance.background.style.display = gameInstance.progress.style.display = "none";
    //gameInstance.logo.style.display = gameInstance.progress.style.display = "none";
	gameInstance.message1.style.display = gameInstance.progress.style.display = "none";
	gameInstance.message2.style.display = gameInstance.progress.style.display = "none";
	gameInstance.message3.style.display = gameInstance.progress.style.display = "none";
  }
}