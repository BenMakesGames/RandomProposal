`System.Random` and `Cryptography.RandomNumberGenerator` conceptually do the same thing: generate random numbers. However,  because these implementations are unrelated, neither benefits from  additions to the other; work is being duplicated, and developers have to learn two APIs.

It feels like like `System.Random` and `Cryptography.RandomNumberGenerator` are mixing concerns:

1. the concern of generating random bytes; and
2. the concern of what to *do* with those random bytes (e.g., compose them into an integer, shuffle an array, etc)

Third-party libraries also suffer. For example, Unity and Godot have their own `Random` classes, with different APIs (see Unity's `insideUnitCircle` - https://docs.unity3d.com/ScriptReference/Random-insideUnitCircle.html). These implementation will not gain any additions made to `System.Random` in .NET 8, or later, and these are _additional_ APIs C# developers must learn.

I believe this can all be tidied up.

### API Proposal

To solve the issues above, I propose a single interface - `IRandom` - which requires a single method: `FillBytes`. `System.Random` and `Cryptography.RandomNumberGenerator` would implement this method according to their requirements; the other methods we're familiar with from these classes (`Shuffle`, `NextDouble`, etc) would be removed, now provided by extension methods to the `IRandom` interface.

From a developer's point of view, `System.Random` and `Cryptography.RandomNumberGenerator` would each gain (most of) the methods of the other. Existing code continues to work as always. However, package writers would be able to write extensions on `IRandom` to buff all implementations, and write new implementations of `IRandom` without giving up the BCL RNG methods they know and love.