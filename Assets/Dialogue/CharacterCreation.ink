INCLUDE Globals.ink
HELLO I AM BLIND!!!! #sound:blind
I AM ALSO BLIND! #name:BLIND
-> main

== main ==
i want to know about you... <br>ok?
+ [sure] great ->start

+ [no] uhhuh ->start


== start ==
{
- hair_color == "": -> Q1 
- else: You've already customized your character!!!! 
-> DONE
}

== Q1 ==
what is your head color? <<del> err... hair color i mean? #layout:left #unskippable
* [brown]
~hair_color = "brown"
->Q2
* [black]   //has spaces after to test the skip blank lines feature
~hair_color = "black"
->Q2
* [red]
this is a really long unskippable piece of text because you have have red hair, looooooooooooooooooooooooooooooooooooooooooooooooool #unskippable #name:game dev #layout:right #autoskip:1 #sound:game_dev
 #name:BLIND #layout:left #sound:blind
~hair_color = "red"
->Q2
* [demon]
~hair_color = "demon"
->Q2
== Q2 ==
ahh yes, {hair_color}...
what is your
.<<space:3>.<<space:3>. #typingspeed:1.8 #unskippable #autoskip:1
uhhhh #unskippable #autoskip:1
eye color?

* [blue]
~eye_color = "blue"
->Q3
* [brown]
~eye_color = "brown"
->Q3
* [green]
~eye_color = "green"
->Q3
* [demon]
~eye_color = "demon"
->Q3

== Q3 ==
ALRIGHT!!!
what is your skin color? 🕵️

* [light]
~skin_color = "light"
->comments
* [dark]
~skin_color = "dark"
->comments
* [cyborg]
~skin_color = "cyborg"
->comments
* [demon]
~skin_color = "demon"
->comments



== comments ==
Okay, so you have {skin_color} skin, <br><<wait:1>{eye_color} eyes, <br><<wait:1>and {hair_color} hair.
im blind
->END


