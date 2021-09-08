.. code-block:: csharp
   :emphasize-lines: 9

   void OnPlayerEnter(PlayerController player)
   {
       if (collected) return;
       //disable the gameObject and remove it from the controller update list.
       frame = 0;
       sprites = collectedAnimation;
       if (controller != null)
           collected = true;
           RealmController.collectToken(); 
       //send an event into the gameplay system to perform some behaviour.
       var ev = Schedule<PlayerTokenCollision>();
       ev.token = this;
       ev.player = player;
   }
