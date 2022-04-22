// // The enemy is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        // Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, groundLayer);

        // bool touched = false; 

        // for (int i = 0; i < colliders.Length; i++)
        // {   
        //     if (colliders[i].gameObject != gameObject)
        //     {   
        //         touched = true;
        //         isGrounded = true;
        //         if (!wasGrounded)
        //             OnLandEvent.Invoke();
        //     }
        // }

        // if (touched)
        // {
        //     Debug.Log({transform.position, rigidBody.velocity});        
        // }

        // if (!isGrounded && (!isHostile || isPatrolling))
        // {
        //     mustTurn = true;
        // }