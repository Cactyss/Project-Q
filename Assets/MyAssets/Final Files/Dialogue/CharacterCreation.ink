INCLUDE Globals.ink
HELLO I AM BLIND!!!!
I AM ALSO BLIND! #name:BLIND
-> main

== main ==
i want to know about you... <br>ok?
+ [sure] great ->Q1

+ [no] uhhuh
->Q1

== Q1 ==
what is your head color? <<del> err... hair color i mean? #layout:left
* [blonde]
~eye = "blonde"
->Q2
* [brown]
~eye = "brown"
->Q2
* [black]   //has spaces after to test the skip blank lines feature
~eye = "black"
->Q2
* [red]
this is a really long unskippable piece of text because you have have red hair, looooooooooooooooooooooooooooooooooooooooooooooooool #unskippable #name:game dev #layout:right #autoskip:1
 #name:BLIND #layout:left
~eye = "red"
->Q2
== Q2 ==
ahh yes, {eye}...
what is your
.   .   . #typingspeed:1.3 #unskippable #autoskip:1
uhhhh

->Q3
== Q3 ==
Q3
->Q4
== Q4 ==
Q4
->END


