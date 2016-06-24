# Rush Hour Game Solver

Motivation of this project is that it was one of my university's assignments. The Rush Hour is quite popular game, but if 
you're not familiar with it, please visit: [Rush Hour - Description](https://en.wikipedia.org/wiki/Rush_Hour_(board_game)) or 
[try it yourself](http://www.thinkfun.com/play-online/rush-hour/).

Here, I'm presenting the solver to the problem of finding consequent moves allowing the desired car to reach the exit. It 
is the `minimal move` solution (if you read more about it, you might also want to find `minimal slide` solution, but it's not what this 
project is about - solutions provided by game levels' creators are mostly about the first approach) that uses `BFS` (Breadth First Search)
algorithm and I'm using `C#`.

#How to use it?
Think of a board as an two-dimensional array of `n` size. It's origin `(0,0)` is situated at the bottom-left corner. It consists of cars,
which position can be expressed relative to the origin by providing consequently the horizontal and vertical distance from it as well as
the orientation of a car- this can be horizontal (`H`) or vertical (`V`). We also need to pass the size of a car. So, if we want to mark 
the car `A` of length 2 as the horizontal one, that is positioned 3 units away horizontally, and 2 units away vertically to the origin 
point, the syntax will be: `A 3 2 H 2`. <b>Notice, that exactly one space between those alphanumerical values is allowed.</b> To use the
solver we need to define `n` and all the cars positioned on the board. <b>There is a neccessity to mark the main car (the one for which
we want to find the path to exit) as X</b>. Additionally, we need to provide the height of the exit cell, as we know there are different
variations of the game. The height is counted from the bottom of the board, starting with 0 as the lowest one (the first row in the board
counting from the bottom).

#Let's find the path!
To use the solver, you need to create the instance of the `RushHour` class by passing the size of an array and the height of the exit cell.
Then, all you need to to is to use the `SolvePuzzle()` method! You will then need to provide some of the data, each one must be confirmed
with the `Enter` key. This is:
- number of test cases (`T`) needed to perform (number of configurations to solve at one batch),
- number of cars (`N`) on the board for the next test case,
- list of `N` cars on the board- each of them must be accepted with `Enter` key,
This is to repeat for the whole of (`T`) times.

Here's the example of the correct input:
1
3
X 1 3 H 2
A 0 0 H 3
B 3 1 V 2

Have fun!


Notes:
- If you find it uncomfortable to provide the input from the console, feel free to adjust it to your need! I've just done to be 
correct according to the given requirements. Modification with the provided code is trivial.
- I've prepared some method that can display the consecutive board states. By default, they're not used, to improve the performance, 
but they are ready to go, so if you need the visualization in the form of the table, there's all you need 
(unless you want to do it in a more sophisticated way)!
