Author: Ottavio Ficaccio
Student ID: 261068575

There are some known bugs in my solution: 
1) sometimes the pinball clips through the paddles
2) sometimes the pinballs get stuck inside of each other. 

My solution implements a rigid body system to move the pinballs and a collision system that detects and resolves when pinballs intersect with other objects differently depending on the type of collision. 

The paddle is represented by an OBB collider, that rotates on the same update delta time as the pinballs. When the paddle is moving and a pinball collides with it, force is applied on the normal proportional to the amount of intersection and the velocity of the pinball. This feels pretty realistic most of the time. 

I detect pinball-pinball collision by setting one pinball to be stationary and combine the velocities in one pinball. I do this in both directions to create a collision event for each pinball. Then I calculate the impulse vectors for resolution using the formulas we covered in class, maintaining conservation of energy.