# CrosswalkSimulation
[demo video](https://youtu.be/11USfZurHVs)
[deploy link](https://mjrb.github.io)

simulation of crosswalk for analyzing traffic and comparing a traffic light vs free crosswalk.

Description of system:  
We attempted to model the environment of a city crosswalk with two kinds of agents: cars and pedestrians. This simulation could be used to test different avoidance behaviors for cars and agents, test flow or wait time of agents trying to use the intersection, or even test different placements of entrances and exits to examine effects on traffic flow or business popularity in a city environment.

Hypothesis:
At high enough level traffic, a traffic light will improve traffic flow through an intersection, but at a lower level of traffic, it will hinder flow through the intersection.

Independent variables: pedestrian and car spawn rate
Dependent variables: pedestrians and cars flowing out of the intersection with or without traffic light.

Car Behavior:  Car will move based on mouse click input where you click destination and it will drive there using shortest pathfinding algorithm known A*. Car is able to drive through the street when the user clicks on a location, let's say where pedestrians are walking. Car will first let the pedestrians cross the street before it moves to its clicked location. Car also knows how to recognize detour areas and know which detour areas to avoid.

Pedestrian behavior:
Pedestrians are loosely based on the ones we used for the social forces project. Originally the idea was to replace the wall force with one based on moving it to the closest point on the nav mesh, but it wound up being easier just to have the agent just teleport if it gets too far from the nav mesh. We also added the sliding force that we didn’t have in the social forces project. One issue we found our implementation is that an agent can unfortunately get trapped in a corner along with a few other artifacts. We tried toggling navmesh obstacles to get the agents to route around each other but that made things worse since when the agent is turned on the navmesh hasn’t updated, and it teleports to a point nearby making artifacts worse.

To more closely model the city environment we placed sources and sinks at certain doors in the environment. Sources spawn pedestrians at a regular interval to simulate them leaving the building, and are destroyed by sinks to simulate entering a building.

Once spawned agents will begin running through a cycle of behavior, first picking a random point from a shared set and navigating to it, and then waiting a random interval. A timeout for the walking phase can be set, however when using it there can be issues with an agent deciding to stop in the middle of a road.

Pedestrians also have the optional ability to ignore traffic lights and jaywalk, see traffic light behavior for more details.

Traffic light behavior:
A traffic light has been placed in the scene to control the flow through the crosswalk (whether cars or pedestrians have the right of way). It uses groups of trigger boxes to detect agents wishing to use the intersection. A Waiter interface is implemented by agents so that if an agent tries to use the intersection but does not have the right of way, its wait method will be called. Once the light changes right of way, any waited agents have the unwait method called on them, so they can proceed to use the intersection. Since agents could have many colliders for different purposes, a waiter can specify which collider on the agent is used for entering the intersection.

When a pedestrian is configured to jaywalk, it tells the traffic light it is ignoring it when the traffic light asks them to wait. Instead while in the car check/traffic light zone they look for cars within a large trigger box in front of them, and if it finds any it will call wait on itself, and if none exist or the car exits it will unwait and proceed through the intersection.

We also created a more complicated script that would use a ray cast and take the speed of an oncoming car and use its own velocity to project if the two will collide. The math in the implementation should be close to working, but for the sake of time we wound up using the simpler mechanism.

Click to move car navigation mostly worked with custom a* code, but wound up having a bug with setting the point programmatically so we unfortunately couldn’t run the simulation properly to collect data

Evaluation/Reflection:
Even though we wound up not collecting data we still got to explore some very interesting avoidance behavior, and got to work with a more custom a* algorithm for the cars. The pedestrians used built in nav mesh. The project was very close to being able to collect data but unfortunately we weren’t focused on collecting this data until the day before it was due, which was poor planning on our part. It drifted a bit from what we initially proposed, but i hope some of this behavior we modeled is at least interesting to see/think about
