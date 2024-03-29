`System.Random` and `Cryptography.RandomNumberGenerator` conceptually do the same thing: generate random numbers. However,  because these implementations are unrelated, neither benefits from  additions to the other; work is being duplicated, and developers have to learn two APIs.

It feels like like `System.Random` and `Cryptography.RandomNumberGenerator` are mixing concerns:

1. the concern of generating random bytes; and
2. the concern of what to *do* with those random bytes (e.g., compose them into an integer, shuffle an array, etc)

Third-party libraries also suffer. For example, Unity and Godot have their own `Random` classes, with different APIs (see Unity's `insideUnitCircle` - https://docs.unity3d.com/ScriptReference/Random-insideUnitCircle.html). These implementation will not gain any additions made to `System.Random` in .NET 8, or later, and these are _additional_ APIs C# developers must learn.

I believe this can all be tidied up.

### API Proposal

To solve the issues above, I propose a single interface - `IRandom` - which requires a single method: `FillBytes`. `System.Random` and `Cryptography.RandomNumberGenerator` would implement this method according to their requirements; the other methods we're familiar with from these classes (`Shuffle`, `NextDouble`, etc) would be removed, now provided by extension methods to the `IRandom` interface.

From a developer's point of view, `System.Random` and `Cryptography.RandomNumberGenerator` would each gain (most of) the methods of the other. Existing code continues to work as always. However, package writers would be able to write extensions on `IRandom` to buff all implementations, and write new implementations of `IRandom` without giving up the BCL RNG methods they know and love.

### Work to Do (as of 2023-05-03)

* The original `System.Random` and `Cryptography.RandomNumberGenerator` have slightly different implementations of a "get int within range" method. My implementation uses one method for both, so I need to check if this difference is significant (both appear to be doing extra work to get even distributions of numbers, which my implementation also does).
* For filling bytes, my implementations are very close in speed to  `Cryptography.RandomNumberGenerator `and `System.Random`; however for common number generation functions, my reimplementation of System.Random is about 1/4 speed. The speeds are still crazy-fast, but I expect telling someone "it's 1/4 as fast, though" will not go down well.
* This repo contains 5 RNGs that demonstrate a variety of RNG algorithms, including the two that are intended to replace `System.Random` and `Cryptography.RandomNumberGenerator`. I'd like to run them ALL the RNGs through TestU01 (see https://stackoverflow.com/questions/65403695/how-to-run-testu01-to-test-a-random-number-generator), partially just for my amusement (the `FF1Random` and `TetrisRandom` RNGs are sure to fail hard), but also to make sure I haven't screwed something up :P
* The API I've written so far is not like `System.Random` or `Cryptography.RandomNumberGenerator`. I need to pick a naming style, go with it, and then write pass-thrus for backwards compatibility. (The chosen naming style should minimize the number of pass-thrus needed.)
* I'm interested to add extension methods inspired by Unity and/or Godot, such as Unity's `insideUnitCircle`.