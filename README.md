# ffvii-optimal
FFVII Optimal Max HP/MP Helper Tool

Created by codemann8

This tool is to assist any player wishing to achieve Natural Max HP/MP for any characters in the Final Fantasy VII game.  The goal is to minimize the probability of requiring resets into the future by making smarter choices in the begininng.  Prior to this tool, there was only a guide[See 'Evolution' below] to tell you each character's Safety Level, the level in which all HP and MP values that can be achieved at this level all have paths that can lead to the maximum possible HP and MP.  The level following the Safety Level is where the player must now start verifying whether their HP and MP can eventually lead to maximum values.  To do this, one had to look up values in a chart, found in Party Mechanics Guide on GameFAQs made by TFergusson, and later corrected and made more complete in AbsoluteSteve's Full Walkthrough, also on GameFAQs.  This tool builds on the knowledge of both guides and incorporates the game's probability calculation to determine and provide for you an insight as to which value you can expect to be more fruitful.  For example, you might hit one of your safe values for a level on the first try, but that value may cost you an estimated 63 restarts more down the line due to the possible paths to the end shrinking as you reach the end; However, instead of taking that value you got on the first try which leads to sooner doom, it could be possible that there's a 1/8 chance of hitting a value with a bexecutable (*.exeetter future path, and this tool helps you determine that.

How to Use:
1) First, all that is needed is the executable (*.exe), found in /FF7OptimalHP/, however, this should be placed into a directory of some kind that you have 'write' permissions for, there are some cache files that are created in the same directory as the program to save yourself from constant recalculation.
2) Upon opening, you can click each Character to see all the paths available in the tree window.
3) You can start by entering in your Characters' Level, HP, and MP in the boxes on the top and then hitting Set, this will refresh the tree to that node and will show you the possible values you can get during the next Level Up.  Hitting Clear will reset that Character and the tree will show Lv 1.
3) The tree will show to the Level of the Character, followed by the HP and MP value, followed by the chance of hitting that value, followed by the estimated range of game resets required. For example, one node in the tree may say: Lv 83 (7307 / 814) 4.3% chance [2141.92 - 14505.46 resets].  Don't be alarmed by the high number of resets that it displays, due to this tool, it will be much less than this.  The reason it shows such a high value is because there is no way for it to consider that you are willing to bend and accept a lesser value, the math assumes you would only accept one of the possible values, the best one.  The main thing to consider is if you were to start down one path, check to see if the maximum resets drops by a bunch and that the minimum stays low as well.
4) When you've decided to accept the value the game gave you, select that node and hit Set, this will re-structure the tree and display it accordingly.  I recommend doing this each level as opposed to hopping up multiple levels at once, the math will be more correct when eliminating values of the past, meaning the top level values should be treated more accurately.

In review, you can either enter your Characters' Level, HP, and MP in the textboxes or by selecting a node in the tree and hitting Set, depending on which is more convenient given the situation. Try to focus on reducing the range of min/max resets but not to be too picky, that will ensure the best results.

Evolution:
-First, TFergusson, a user on GameFAQs, pioneered the exploration of the game's inner calculation of HP and MP during each Level Up.  He found that for each given Level Up, there are 8 possible HP values and 8 possible MP values.  Due to the game's algorithm, he found there was a curve to the calculation, effectively penalizing you for always only going for the highest HP or MP available.  Instead, it's better to go for average until the end.  He then provided 2 charts for each Character, one for HP safe values and another for MP safe values.  These are to help guarantee a possible path to a maximum value at Level 99.
-However, it was later found that it's not possible to gain both the highest HP and highest MP during the same Level Up, so additional work was needed to determine how to grow both HP and MP at the same time.  AbsoluteSteve, another user of GameFAQs, created 1 chart for each character that combined both HP and MP.
-As mentioned earlier, there's 8 possible HP and MP values during each Level Up.  These are determined by the game's RNG (Random Number Generator). However, due to the game having a poor RNG implementation, it was found that not only is the highest of both not possible to achieve, there's a couple combinations that also aren't possible.  Along with that, it was found that some combinations hit more often than others, in fact, some hit 9/256 while others hit 1/256.  Because of this wide disparity of probability, this would mean that some paths leading to Max HP/MP have better probabilities than some others.  That is what this tool helps you, the gamer, determine.  Starting at Level 1 for a given Character, the min/max resets may show as 2500 vs 14000, meaning, if you stayed true to the 2500 path, on average, you will reset your console 2500 times.  This, of course, is an absurb amount, this is due to it only showing you THAT path and not considering that you have other options that also are nearly close to that, so you may not need to reset for those times, but it is wise to consider only paths that reduce your minumim probable resets.
