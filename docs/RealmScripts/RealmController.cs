//
//  RealmController.cs
//
//
//  Copyright © 2020-2021 MongoDB, Inc. All rights reserved.
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Realms;
using UnityEngine.UI;
using MongoDB.Bson;
using System.Threading.Tasks;
using Realms.Sync;
using System.Linq;

// :snippet-start: RealmController
public class RealmController : MonoBehaviour
{
    private static Realm realm;
    private static App realmApp;
    private static int runTime; // total amount of time you've been playing during this playthrough/run (losing/winning resets runtime)
    private static int bonusPoints = 0; // start with 0 bonus points and at the end of the game we add bonus points based on how long you played

    public static Player currentPlayer; // current logged in player
    public static Stat currentStat; // current stats for this run/playthrough
    public static Realm GetRealm()
    {
        // :code-block-start: get-realm-fn
        // :state-start: start 
        // TODO: open a realm and return it
        // :state-end: :state-start: local sync
        realm = Realm.GetInstance();
        return realm;
        // :state-end:
        // :code-block-end:
    }
    public static void setLoggedInUser(string loggedInUser)
    {
        // :code-block-start: set-logged-in-user
        realm = GetRealm();
        // :state-start: start
        // TODO: "Set the `currentPlayer` variable by querying the realm for the
        // player. If the player exists, give the player a new Stat object,
        // otherwise create a new player and give the new player a new Stat
        // object
        // :state-end:
        // :state-start: sync local
        var matchedPlayers = realm.All<Player>().Where(p => p.Name == loggedInUser);

        if (matchedPlayers.Count() > 0) // if the player exists
        {
            currentPlayer = matchedPlayers.First();
            var s1 = new Stat();
            s1.StatOwner = currentPlayer;

            realm.Write(() =>
            {
                currentStat = realm.Add(s1);
                currentPlayer.Stats.Add(currentStat);
            });
        }
        else
        {
            var p1 = new Player();
            p1.Id = ObjectId.GenerateNewId().ToString();
            p1.Name = loggedInUser;

            var s1 = new Stat();
            s1.StatOwner = p1;

            realm.Write(() =>
            {
                currentPlayer = realm.Add(p1);
                currentStat = realm.Add(s1);
                currentPlayer.Stats.Add(currentStat);
            });
        }
        // :state-end:

        startGame();
        // :code-block-end:
    }

    // startGame() begins the timer, increasing runTime every 10 seconds.
    // The less time a player takes to complete the playthrough/run, the more bonusPoints the player is rewarded
    public static void startGame()
    {
        // record each 10 seconds (runTime will be used to calculate bonus points once the player wins the game)
        var myTimer = new System.Timers.Timer(10000);
        myTimer.Enabled = true;
        myTimer.Elapsed += (sender, e) => runTime += 10;
    }

    public static void collectToken() // performs an update on the Character Model's token count
    {
        // :code-block-start: collect-token-fn
        // :state-start: start
        // TODO: within a write transaction, increment the number of token's collected in the current playthrough/run's stat
        // :state-end:
        // :state-start: sync local
        realm.Write(() =>
        {
            currentStat.TokensCollected += 1;
        });
        // :state-end:
        // :code-block-end:
    }
    public static void defeatEnemy() // performs an update on the Character Model's enemiesDefeated Count
    {
        // :code-block-start: defeat-enemy-fn
        // :state-start: start
        // TODO: within a write transaction, increment the number of enemies's defeated in the current playthrough/run's stat
        // :state-end:
        // :state-start: sync local
        realm.Write(() =>
        {
            currentStat.EnemiesDefeated += 1;
        });
        // :state-end:
        // :code-block-end:
    }

    // deleteCurrentScore deletes the stats of the player's current playthrough/run and unregisters the listener on the old stat
    // deleteCurrentScore is typically called on the "PlayerDeath" event
    public static void deleteCurrentScore()
    {
        ScoreCardManager.unRegisterListener();

        // :code-block-start: delete-current-stat
        // :state-start: start
        // TODO: within a write transaction, delete the current state. Once the current stat is deleted, remove the reference from the currentPlayer object
        // :state-end:
        // :state-start: sync local
        realm.Write(() =>
        {
            realm.Remove(currentStat);
            currentPlayer.Stats.Remove(currentStat);
        });
        // :state-end:
        // :code-block-end:
    }
    public static void restartGame()
    {
        if (currentPlayer != null)
        {
            // :code-block-start: restart-game
            // :state-start: start
            // TODO: within a write transaction, create a new Stat for the current player
            // :state-end:
            // :state-start: sync local
            var s1 = new Stat();

            realm.Write(() =>
            {
                currentStat = realm.Add(s1);
                currentPlayer.Stats.Add(currentStat);
            });
            // :state-end:
            // :code-block-end:

            ScoreCardManager.setCurrentStat(currentStat); // call `setCurrentStat()` to set the current stat in the UI using ScoreCardManager
            ScoreCardManager.watchForChangesToCurrentStats(); // call `watchForChangesToCurrentStats()` to register a listener on the new score in the ScoreCardManager

            startGame(); // start the game by resetting the timer and officially starting a new run/playthrough
        }
    }

    public static int[] playerWon()
    {
        if (runTime <= 30) // if the game is beat in in less than or equal to 30 seconds, +80 bonus points
        {
            bonusPoints = 80;
        }
        else if (runTime <= 60) // if the game is beat in in less than or equal to 1 min, +70 bonus points
        {
            bonusPoints = 70;
        }
        else if (runTime <= 90) // if the game is beat in less than or equal to 1 min 30 seconds, +60 bonus points
        {
            bonusPoints = 60;
        }
        else if (runTime <= 120) // if the game is beat in less than or equal to 2 mins, +50 bonus points
        {
            bonusPoints = 50;
        }

        // :code-block-start: player-won-game-fn
        // :state-start: start
        // TODO: within a write transaction, create a new Stat for the current player
        // :state-end:
        // :state-start: sync local
        // calculate final points + write to realm with points
        var finalScore = calculatePoints();
        realm.Write(() =>
        {
            currentStat.Score = finalScore;
        });
        // :state-end:
        // :code-block-end:


        var scoreAndBonusPoints = new int[2];
        scoreAndBonusPoints[0] = finalScore;
        scoreAndBonusPoints[1] = bonusPoints;

        return scoreAndBonusPoints; // return the final score and bonus points to display to the UI
    }

    public static int calculatePoints()
    {
        return (currentStat.EnemiesDefeated + 1) * (currentStat.TokensCollected + 1) + bonusPoints;
    }
}
// ::snippet-end: