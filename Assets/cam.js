#pragma strict

 private var theRatio : float;
 private var x : float;
 private var y : float;
 var offset : float;
 var marginFactor : float;
 
 function Update () {
     FindingRatio();
     Reposition();
 }
 
 function FindingRatio(){
     x = Screen.width+0.0;
     y = Screen.height+0.0;
     theRatio = x/y;
 }
 
 function Reposition(){
     gameObject.transform.position.x = (offset-theRatio)*marginFactor;
 }