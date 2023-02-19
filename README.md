# RevitStraightSkeleton
Revit Pugins for line conversion and straight skeleton.
This tool can be used in generating roof, finding center line in custom shape, etc.
<p>1. straight skeleton</p>
<img src="https://github.com/Tanc60/RevitStraightSkeleton/blob/main/picture/3.png?raw=true" width="200">
<p> 
In revit the middle lines of walls can be automatically generated, however, for custom shaped wall and other object, there is no automatic function to do.
 Because of a lack of reference line, it is often tedious to hand draw the middle line. In this case, straight skeleton tool is useful for solving this problem.
</p> 
<p> For instance, the red middle line is generated from the blue boudary.</p>
![image](https://user-images.githubusercontent.com/48271765/219923464-d05b7c3c-1b1c-49de-aa7a-551e208a14b2.png)
![image](https://user-images.githubusercontent.com/48271765/219923529-8d6038b6-8710-413e-b62e-7cf3a6c66683.png)



<p>2. modelline to room separation line</p>

<img src="https://github.com/Tanc60/RevitStraightSkeleton/blob/main/picture/1.png?raw=true" width="200">

<p> 
Some time we need to import CAD lines to Revit, but the lines from the CAD can only be convert to model lines.
This tool can convert modelline to room separation line.
Useful in importing room separation from CAD file.
</p>
<p>3. modelline to area boundary line</p>
<img src="https://github.com/Tanc60/RevitStraightSkeleton/blob/main/picture/2.png?raw=true" width="200">
<p> 
 For the similar reason, area boundary line can be generated from CAD file.
The area boundary line is useful in defining custom area, especially for area with width less than 275mm, which otherwise will be ignored by room function.
</p>
