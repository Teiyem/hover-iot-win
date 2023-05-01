namespace Hover.Models;

#region Attribution
// Code attribution: Original Author Luke Malpass (Angel Six)
#endregion

/// <summary>
/// The direction an animation slides in.
/// </summary>
public enum AnimationDirection
{
    /// <summary>
    /// Indicates that the animation animates to/from the left.
    /// </summary>
    Left = 0,

    /// <summary>
    /// Indicates that the animation animates to/from the right .
    /// </summary>
    Right = 1,

    /// <summary>
    /// Indicates that the animation animates to/from the top.
    /// </summary>
    Top = 2,

    /// <summary>
    /// Indicates that the animation animates to/from the bottom.
    /// </summary>
    Bottom = 3
}