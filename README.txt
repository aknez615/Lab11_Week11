Alyssa Knez
DIG 4778
Lab11_Week11

How does each pathfinding algorithm calculate and prioritize paths?
BFS: Does not use cost, heuristic, or weighted and is the only one to use Queue.
A*: Uses cost and heuristic and PriorityQueue which makes it the fastest out of the three
Dijskta: Uses cost, and weighted/unweighted but does not use heuristic which causes it to lose speed even though it uses a PriorityQueue

What challenges arise when dynamically updating obstacles in real-time?
I've noticed that it starts to run slower. When only implementing the generator the code ran quickly even though the cells changed on Start, however,
when adding in AddObstacle it took a while for the code to run.


Which algorithm should you choose and how should you adapt it for larger grids or open-world settings?
I would either choose A* and BFS due to it being faster run times which are needed for open-world environments. If the scene loads too slow then the player can
be brought out of their immersion and it would no longer be fun for them. I think performance should be a main priority.

What would your approach be if you were to add weighted cells (e.g., "difficult terrain" areas)?
I honestly think it would tank the performance even more so especially in open-world environments. Adding in different terrain and how your player goes
through them would be fun and exciting but I imagine it would be difficult for the system to keep up if it is constantly generating a random terrain when the player
goes into that load area. For example, Minecraft has this kind of randomized seeds where it is loading in this large open-world and on lower end computers it can cause
the game to lower in performance.