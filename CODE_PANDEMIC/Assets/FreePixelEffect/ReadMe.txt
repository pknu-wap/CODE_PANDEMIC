FreePixelEffect
art: benmhenry@gmail.com
code: davidahenry@gmail.com

Description:
Includes animated, tintable, arrow sprites and additive blend shader.<br>
Also includes simple animated slime.<br>
Can be used to draw simple tile paths or whatever you like.

Documentation:
0. Use Path.prefab or
1. Create->2D->Sprite
2. Add Animator and select path animation
3. Set Additive mat on sprite renderer if want

Files:
FreePixelEffect/Additive.mat : additive blend mode sometimes looks better with transparent sprites
FreePixelEffect/Additive.shader : this is just the built in sprite shader copied and changed to "Blend SrcAlpha One"
FreePixelEffect/Effect.unity : example scene
FreePixelEffect/Path.controller : animation controller cycles through sprites
FreePixelEffect/Path.png : all path sprites on single sprite sheet
FreePixelEffect/Path.prefab : can be used to instantiate this effect
FreePixelEffect/ReadMe.txt : this
FreePixelEffect/Slime.controller : animation controller with walk, attack, and idle animations
FreePixelEffect/SlimeA.png : all slime sprites on single sprite sheet
FreePixelEffect/StateRandom.cs : used in slime animation controller to select random idle animation
