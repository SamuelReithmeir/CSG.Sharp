# CSG.Sharp

![](https://img.shields.io/badge/.net8.0-blue.svg)
![](https://img.shields.io/badge/build-passing-green.svg)

This is a .Net8.0 port/fork of the PSneijders [CSG.Sharp library](https://github.com/PSneijder/CSG.Sharp). The original library was written in .Net-Framework and was not published to NuGet.
This is a slimmed down version with some bugfixes and extensions.


# OG Readme:

Constructive solid geometry on meshes using BSP trees in C#

![alt tag](https://github.com/PSneijder/CSG.Sharp/blob/master/Assets/CSG.Sharp.png)

Constructive Solid Geometry (CSG) is a modeling technique that uses Boolean operations like union and intersection to combine 3D solids. This library implements CSG operations on meshes elegantly and concisely using BSP trees, and is meant to serve as an easily understandable implementation of the algorithm. All edge cases involving overlapping coplanar polygons in both solids are correctly handled.

Example usage:

```csharp
    var cube = Cube.Create();
    var sphere = Sphere.Create(radius: 1.3);
    var polygons = cube.Subtract(sphere).ToPolygons();
```

## Implementation Details

All CSG operations are implemented in terms of two functions, `clipTo()` and `invert()`, which remove parts of a BSP tree inside another BSP tree and swap solid and empty space, respectively. To find the union of `a` and `b`, we want to remove everything in `a` inside `b` and everything in `b` inside `a`, then combine polygons from `a` and `b` into one solid:

```csharp
    a.ClipTo(b);
    b.ClipTo(a);
    a.Build(b.AllPolygons());
```

The only tricky part is handling overlapping coplanar polygons in both trees. The code above keeps both copies, but we need to keep them in one tree and remove them in the other tree. To remove them from `b` we can clip the inverse of `b` against `a`. The code for union now looks like this:

```csharp
    a.ClipTo(b);
    b.ClipTo(a);
    b.Invert();
    b.ClipTo(a);
    b.Invert();
    a.Build(b.AllPolygons());
```

Subtraction and intersection naturally follow from set operations. If union is `A | B`, subtraction is `A - B = ~(~A | B)` and intersection is `A & B = ~(~A | ~B)` where `~` is the complement operator.

## TODOs
* Complete porting of csg.js

## Recent Changes
See [CHANGES.txt](CHANGES.txt)

## Committers
* [Philip Schneider](https://github.com/PSneijder)

Parts are take from the wonderful library https://evanw.github.io/csg.js/

## License

[MIT license](LICENSE)
