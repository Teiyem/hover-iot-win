using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace Hover.Helpers;

#region Attribution
// Code attribution: Original Author Luke Malpass (Angel Six)
#endregion

/// <summary>
/// Extenstion animations methods for storyBoard.
/// </summary>
public static class StoryboardHelpers
{
    /// <summary>
    /// Adds a slide from left animation to the storyboard.
    /// </summary>
    /// <param name="storyboard">The storyboard to add the animation to.</param>
    /// <param name="seconds">The time the animation will take.</param>
    /// <param name="offset">The distance to the left to start from.</param>
    /// <param name="keepMargin">Whether to keep the element at the same width during animation.</param>
    public static void AddSlideFromLeft(this Storyboard storyboard, float seconds, double offset, bool keepMargin = true)
    { 
        var animation = new ThicknessAnimation
        {
            Duration = new Duration(TimeSpan.FromSeconds(seconds)),
            From = new Thickness(-offset, 0, keepMargin ? offset : 0, 0),
            To = new Thickness(0),
            DecelerationRatio = 0.9f
        };

        Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));

        storyboard.Children.Add(animation);
    }

    /// <summary>
    /// Adds a slide to left animation to the storyboard.
    /// </summary>
    /// <param name="storyboard">The storyboard to add the animation to.</param>
    /// <param name="seconds">The time the animation will take.</param>
    /// <param name="offset">The distance to the left to end at.</param>
    /// <param name="keepMargin">Whether to keep the element at the same width during animation.</param>
    public static void AddSlideToLeft(this Storyboard storyboard, float seconds, double offset, bool keepMargin = true)
    {
        var animation = new ThicknessAnimation
        {
            Duration = new Duration(TimeSpan.FromSeconds(seconds)),
            From = new Thickness(0),
            To = new Thickness(-offset, 0, keepMargin ? offset : 0, 0),
            DecelerationRatio = 0.9f
        };

        Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));

        storyboard.Children.Add(animation);
    }

    /// <summary>
    /// Adds a slide from right animation to the storyboard
    /// </summary>
    /// <param name="storyboard">The storyboard to add the animation to.</param>
    /// <param name="seconds">The time the animation will take.</param>
    /// <param name="offset">The distance to the right to start from.</param>
    /// <param name="keepMargin">Whether to keep the element at the same width during animation.</param>
    public static void AddSlideFromRight(this Storyboard storyboard, float seconds, double offset, bool keepMargin = true)
    {
        var animation = new ThicknessAnimation
        {
            Duration = new Duration(TimeSpan.FromSeconds(seconds)),
            From = new Thickness(keepMargin ? offset : 0, 0, -offset, 0),
            To = new Thickness(0),
            DecelerationRatio = 0.9f
        };

        Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));

        storyboard.Children.Add(animation);
    }

    /// <summary>
    /// Adds a slide to right animation to the storyboard.
    /// </summary>
    /// <param name="storyboard">The storyboard to add the animation to.</param>
    /// <param name="seconds">The time the animation will take.</param>
    /// <param name="offset">The distance to the right to end at.</param>
    /// <param name="keepMargin">Whether to keep the element at the same width during animation.</param>
    public static void AddSlideToRight(this Storyboard storyboard, float seconds, double offset, bool keepMargin = true)
    {
        var animation = new ThicknessAnimation
        {
            Duration = new Duration(TimeSpan.FromSeconds(seconds)),
            From = new Thickness(0),
            To = new Thickness(keepMargin ? offset : 0, 0, -offset, 0),
            DecelerationRatio = 0.9f
        };

        Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));

        storyboard.Children.Add(animation);
    }

    /// <summary>
    /// Adds a slide from top animation to the storyboard
    /// </summary>
    /// <param name="storyboard">The storyboard to add the animation to.</param>
    /// <param name="seconds">The time the animation will take.</param>
    /// <param name="offset">The distance to the top to start from.</param>
    /// <param name="keepMargin">Whether to keep the element at the same height during animation.</param>
    public static void AddSlideFromTop(this Storyboard storyboard, float seconds, double offset, bool keepMargin = true)
    {
        var animation = new ThicknessAnimation
        {
            Duration = new Duration(TimeSpan.FromSeconds(seconds)),
            From = new Thickness(0, -offset, 0, keepMargin ? offset : 0),
            To = new Thickness(0),
            DecelerationRatio = 0.9f
        };
        Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));

        storyboard.Children.Add(animation);
    }

    /// <summary>
    /// Adds a slide to top animation to the storyboard
    /// </summary>
    /// <param name="storyboard">The storyboard to add the animation to.</param>
    /// <param name="seconds">The time the animation will take.</param>
    /// <param name="offset">The distance to the top to end at.</param>
    /// <param name="keepMargin">Whether to keep the element at the same height during animation.</param>
    public static void AddSlideToTop(this Storyboard storyboard, float seconds, double offset, bool keepMargin = true)
    {
        var animation = new ThicknessAnimation
        {
            Duration = new Duration(TimeSpan.FromSeconds(seconds)),
            From = new Thickness(0),
            To = new Thickness(0, -offset, 0, keepMargin ? offset : 0),
            DecelerationRatio = 0.9f
        };

        Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));
        storyboard.Children.Add(animation);
    }

    /// <summary>
    /// Adds a slide from bottom animation to the storyboard.
    /// </summary>
    /// <param name="storyboard">The storyboard to add the animation to.</param>
    /// <param name="seconds">The time the animation will take.</param>
    /// <param name="offset">The distance to the bottom to start from.</param>
    /// <param name="keepMargin">Whether to keep the element at the same height during animation.</param>
    public static void AddSlideFromBottom(this Storyboard storyboard, float seconds, double offset, bool keepMargin = true)
    {
        var animation = new ThicknessAnimation
        {
            Duration = new Duration(TimeSpan.FromSeconds(seconds)),
            From = new Thickness(-10, keepMargin ? offset : 0, -10, -offset),
            To = new Thickness(0),
            DecelerationRatio = 0.9f
        };

        Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));
        storyboard.Children.Add(animation);
    }

    /// <summary>
    /// Adds a slide to bottom animation to the storyboard.
    /// </summary>
    /// <param name="storyboard">The storyboard to add the animation to.</param>
    /// <param name="seconds">The time the animation will take.</param>
    /// <param name="offset">The distance to the bottom to end at.</param>
    /// <param name="keepMargin">Whether to keep the element at the same height during animation.</param>
    public static void AddSlideToBottom(this Storyboard storyboard, float seconds, double offset, bool keepMargin = true)
    {
        var animation = new ThicknessAnimation
        {
            Duration = new Duration(TimeSpan.FromSeconds(seconds)),
            From = new Thickness(0),
            To = new Thickness(0, keepMargin ? offset : 0, 0, -offset),
            DecelerationRatio = 0.9f
        };

        Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));
        storyboard.Children.Add(animation);
    }

    /// <summary>
    /// Adds a fade in animation to the storyboard.
    /// </summary>
    /// <param name="storyboard">The storyboard to add the animation to.</param>
    /// <param name="seconds">The time the animation will take.</param>
    /// <param name="from"></param>
    public static void AddFadeIn(this Storyboard storyboard, float seconds, bool from = false)
    {
        var animation = new DoubleAnimation
        {
            Duration = new Duration(TimeSpan.FromSeconds(seconds)),
            To = 1,
        };

        if (from)
            animation.From = 0;

        Storyboard.SetTargetProperty(animation, new PropertyPath("Opacity"));

        storyboard.Children.Add(animation);
    }

    /// <summary>
    /// Adds a fade out animation to the storyboard.
    /// </summary>
    /// <param name="storyboard">The storyboard to add the animation to.</param>
    /// <param name="seconds">The time the animation will take.</param>
    public static void AddFadeOut(this Storyboard storyboard, float seconds)
    {
        var animation = new DoubleAnimation
        {
            Duration = new Duration(TimeSpan.FromSeconds(seconds)),
            To = 0,
        };

        Storyboard.SetTargetProperty(animation, new PropertyPath("Opacity"));
        storyboard.Children.Add(animation);
    }
}