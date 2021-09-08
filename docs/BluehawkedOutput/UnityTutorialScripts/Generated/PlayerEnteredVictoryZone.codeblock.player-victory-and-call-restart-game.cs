public override void Execute()
{
    var scoreAndBonusPoints = RealmController.playerWon();

    var didClickRestart = EditorUtility.DisplayDialog("You won!", $"Final Score = {scoreAndBonusPoints[0]} (including {scoreAndBonusPoints[1]} bonus points)", "restart game", "cancel");
    if (didClickRestart == true)
    {
        Simulation.Schedule<PlayerSpawn>(2);
        RealmController.restartGame();
    }
}
