### File Structure:

- RealmScripts:
    - AuthenticationManager.cs - interacts with the UI to get input values, toggles between signup and signin modes, calls RealmController methods to login, register and logout
    - LeaderboardManager.cs - creates the Leaderboard in the UI, toggles the Leaderboards visibility using Unity USS classes, opens a synced Realm, gets the player's top score, gets the top 5 player scores, and creates a listener to update the top 5 scores
    - PlayerModel.cs - A RealmObject containing the player's name, and a link to their stats
    - StatModel.cs - A RealmObject containing a list of stats (tokens collected, enemies defeated, points, timestamp), and a link to the statOwner
    - RealmController.cs - Where most of the Realm Logic takes place: opens a realm, creates an on register function, an onPressLogin function, logic to calculate scores, logic to update the current score card, logic to restart the game and start a new realm by creating a new Stat(), and should contain logic to logout
    - Constants.cs - has your app id
- UI ToolKit:
    - AuthenticationScreen.uxml - Entire UI for Auth Screen (modes are toggled with AuthenticationManager.cs)
    - Leaderboard.uxml - Mostly empty UI file, since the leaderboard is generated after reading from the Realm, most of the leaderboard UI is actually created in LeaderboardManager.cs
    - scorecard.uxml - Mostly empty UI file, since the scorecard is generated after reading from the Realm, most of the leaderboard UI is actually created in LeaderboardManager.cs
    - Stylesheet.uss - global theme file containing classes applied to both the authentication screen and leaderboard

- UnityHub Microgame Tutorial Precreated files:
    - Scripts/Gameplay/PlayerDeath.cs - a file created by Unity, used by us to call the RealmController to delete the current score and restart the game
    - Scripts/Gameplay/PlayerEnemyCollision - a file created by Unity, used by us to call the RealmController to write to the current player state and += 1 to enemies defeated when a collision has occurred.
    - Scripts/Gameplay/PlayerEnteredVictoryZone.cs -  a file created by Unity, used by us to call the RealmController to call playerWon to calculate and write to final score, also displays the final score to the user
    - Scripts/Mechanics/TokenInstance.cs - a file created by Unity, used by us to call the RealmController to call collectToken() in order to record the collection of a token in the game



To do:
- Add realm in package manager
- Add scripts
- Add com.unity.ui (toolkit); com.unity.ui.builder (builder)
- Create folder assets/UI ToolKit
- In Hierarchy, create UI > Canvas
- Add "ScoreCard", "Leaderboard", "AuthenticationScreen" UI ToolKit > UI Documents to the Canvas Hierarchy
- Attach "AuthenticationScreen.uxml" and "Leaderboard.uxml" to Source Assets of their respective UI Document
- Attach StyleSheet.uss to each UI Toolkit view
- Paste Stylesheet code
- Attach "AuthenticationManager.cs" to Hierarchy's Canvas/Authentication
