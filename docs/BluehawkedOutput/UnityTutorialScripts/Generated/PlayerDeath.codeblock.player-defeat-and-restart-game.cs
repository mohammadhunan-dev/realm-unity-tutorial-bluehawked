public override void Execute()
{
    var player = model.player;
    if (player.health.IsAlive)
    {
        player.health.Die();
        model.virtualCamera.m_Follow = null;
        model.virtualCamera.m_LookAt = null;
        // player.collider.enabled = false;
        player.controlEnabled = false;

        if (player.audioSource && player.ouchAudio)
            player.audioSource.PlayOneShot(player.ouchAudio);
        player.animator.SetTrigger("hurt");
        player.animator.SetBool("dead", true);
        Simulation.Schedule<PlayerSpawn>(2);
        // scores should not be persisted if the player has not won, 
        // on player lost should delete the current score
        RealmController.deleteCurrentScore(); 
        // alert RealmController that the game is being restarted
        RealmController.restartGame(); 
    }
}
