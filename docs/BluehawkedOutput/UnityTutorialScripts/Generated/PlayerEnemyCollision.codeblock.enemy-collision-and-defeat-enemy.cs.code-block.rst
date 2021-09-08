.. code-block:: csharp
   :emphasize-lines: 7

   public override void Execute()
   {
       var willHurtEnemy = player.Bounds.center.y >= enemy.Bounds.max.y;

       if (willHurtEnemy)
       {
           RealmController.defeatEnemy(); 
           var enemyHealth = enemy.GetComponent<Health>();
           if (enemyHealth != null)
           {
               enemyHealth.Decrement();
               if (!enemyHealth.IsAlive)
               {
                   Schedule<EnemyDeath>().enemy = enemy;
                   player.Bounce(2);
               }
               else
               {
                   player.Bounce(7);
               }
           }
           else
           {
               Schedule<EnemyDeath>().enemy = enemy;
               player.Bounce(2);
           }
       }
       else
       {
           Schedule<PlayerDeath>();
       }
   }
