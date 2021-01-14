# Connect-The-Dots
Gameplay: <br/>
Game screen has points, each point has assigned number. Player has to click points from first to last in correct sequence.  
When clicking on correct points, line between them is drawn. <br/>Game works in landscape mode with all aspect ratios. <br/>
<br/>
Actions: <br/>
If correct point is clicked, its texture changes to blue. <br/>
If clicked point is correct and is not 1st, line between previous point and clicked point is drawn with slow animation.  <br/>
If player clicks on points faster than the line appear animation can go, next line is animated only when previous one is finished. <br/>
If clicking points in wrong sequence, nothing happens. <br/>
If all dots connected victory text appears and upon pressed returns player to level menu. <br/>
<br/>
Work in progress: <br/>
*Add functionality "OPTIONS" field in main menu. <br/>
*Fix "Only first started level woks" bug. Game objects "Dots" is deleted first time ant recreated for another level. <br/>
But instance of new dots is not found. <br/>
<br/>


![Main Menu](https://github.com/oOPoloOo/Connect-The-Dots/blob/main/Photos/CTD_Menu.png)  <br/>
![Level Menu](https://github.com/oOPoloOo/Connect-The-Dots/blob/main/Photos/CTD_LevelMenu.png)  <br/>
![Gameplay](https://github.com/oOPoloOo/Connect-The-Dots/blob/main/Photos/CTD_Gameplay.png)  <br/>
