`System.Random` and `Cryptography.RandomNumberGenerator` conceptually do the same thing: generate random numbers. However,  because these implementations are unrelated, neither benefits from  additions to the other; work is being duplicated, and developers have to learn two APIs.

It feels like like `System.Random` and `Cryptography.RandomNumberGenerator` are mixing concerns:

1. the concern of generating random bytes; and
2. the concern of what to *do* with those random bytes (e.g., compose them into an integer, shuffle an array, etc)

Third-party libraries also suffer. For example, Unity and Godot have their own `Random` classes, with different APIs (see Unity's `insideUnitCircle` - https://docs.unity3d.com/ScriptReference/Random-insideUnitCircle.html). These implementation will not gain any additions made to `System.Random` in .NET 8, or later, and these are _additional_ APIs C# developers must learn.

I believe this can all be tidied up.

### API Proposal

To solve the issues above, I propose a single interface - `IRandom` - which requires a single method: `FillBytes`. `System.Random` and `Cryptography.RandomNumberGenerator` would implement this method according to their requirements; the other methods we're familiar with from these classes (`Shuffle`, `NextDouble`, etc) would be removed, now provided by extension methods to the `IRandom` interface.

From a developer's point of view, `System.Random` and `Cryptography.RandomNumberGenerator` would each gain (most of) the methods of the other. Existing code continues to work as always. However, package writers would be able to write extensions on `IRandom` to buff all implementations, and write new implementations of `IRandom` without giving up the BCL RNG methods they know and love.

### Work to Do (as of 2023-03-03)

* `System.Random` and `Cryptography.RandomNumberGenerator` have slightly different implementations of a "get int within range" method. Need to check if this difference is significant (both appear to be doing extra work to get even distributions of numbers).
* The "get int" method in this proposal is slightly slower; compared to `Cryptography.RandomNumberGenerator`, the difference is negligible; compared to `System.Random`, however,  it's almost half the speed!
* This repo contains 4 RNGs, including the two that are intended to replace `System.Random` and `Cryptography.RandomNumberGenerator`. I'd like to run them ALL the RNGs through TestU01 (see https://stackoverflow.com/questions/65403695/how-to-run-testu01-to-test-a-random-number-generator), partially just for my amusement (the `FF1Random` RNG is sure to fail hard), but also to make sure I haven't screwed something up :P
* The API I've written so far is not like `System.Random` or `Cryptography.RandomNumberGenerator`. I need to pick a naming style, go with it, and then write pass-thrus for backwards compatibility.
* It'd be interesting to add methods from Unity and/or Godot, like Unity's `insideUnitCircle`.