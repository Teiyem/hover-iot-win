using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using Hover.Helpers;
using Hover.Models;

namespace Hover.Animation;

#region Attribution
// Code attribution: Original Author Luke Malpass (Angel Six)
#endregion

/// <summary>
/// Helpers to animate framework elements in specific ways
/// </summary>
public static class FrameworkAnimations
{
    /// <summary>
    /// Slides an element in
    /// </summary>
    /// <param name="element">The element to animate</param>
    /// <param name="direction">The direction of the slide</param>
    /// <param name="seconds">The time the animation will take</param>
    /// <param name="keepMargin">Whether to keep the element at the same width during animation</param>
    /// <param name="size">The animation width/height to animate to. If not specified the elements size is used</param>
    /// <param name="firstLoad">Indicates if this is the first load</param>
    /// <returns></returns>
    public static async Task SlideAndFadeInAsync(this FrameworkElement element, AnimationDirection direction, bool firstLoad, float seconds = 0.3f, bool keepMargin = true, int size = 0)
    {
        var sb = new Storyboard();

        switch (direction)
        {
            case AnimationDirection.Left:
                sb.AddSlideFromLeft(seconds, size == 0 ? element.ActualWidth : size, keepMargin);
                break;
            case AnimationDirection.Right:
                sb.AddSlideFromRight(seconds, size == 0 ? element.ActualWidth : size, keepMargin);
                break;
            case AnimationDirection.Top:
                sb.AddSlideFromTop(seconds, size == 0 ? element.ActualHeight : size, keepMargin);
                break;
            case AnimationDirection.Bottom:
                sb.AddSlideFromBottom(seconds, size == 0 ? element.ActualHeight : size, keepMargin);
                break;
        }
        sb.AddFadeIn(seconds);

        sb.Begin(element);

        if (seconds != 0 || firstLoad)
            element.Visibility = Visibility.Visible;

        await Task.Delay((int)(seconds * 1000));
    }

    /// <summary>
    /// Slides an element out
    /// </summary>
    /// <param name="element">The element to animate</param>
    /// <param name="direction">The direction of the slide (this is for the reverse slide out action, so Left would slide out to left)</param>
    /// <param name="seconds">The time the animation will take</param>
    /// <param name="keepMargin">Whether to keep the element at the same width during animation</param>
    /// <param name="size">The animation width/height to animate to. If not specified the elements size is used</param>
    /// <returns></returns>
    public static async Task SlideAndFadeOutAsync(this FrameworkElement element, AnimationDirection direction, float seconds = 0.3f, bool keepMargin = true, int size = 0)
    {
        var sb = new Storyboard();

        switch (direction)
        {
            case AnimationDirection.Left:
                sb.AddSlideToLeft(seconds, size == 0 ? element.ActualWidth : size, keepMargin);
                break;
            case AnimationDirection.Right:
                sb.AddSlideToRight(seconds, size == 0 ? element.ActualWidth : size, keepMargin);
                break;
            case AnimationDirection.Top:
                sb.AddSlideToTop(seconds, size == 0 ? element.ActualHeight : size, keepMargin);
                break;
            case AnimationDirection.Bottom:
                sb.AddSlideToBottom(seconds, size == 0 ? element.ActualHeight : size, keepMargin);
                break;
        }

        sb.AddFadeOut(seconds);

        sb.Begin(element);

        if (seconds != 0)
            element.Visibility = Visibility.Visible;

        await Task.Delay((int)(seconds * 1000));

        if (element.Opacity == 0)
            element.Visibility = Visibility.Collapsed;
    }

    /// <summary>
    /// Fades an element in
    /// </summary>
    /// <param name="element">The element to animate</param>
    /// <param name="seconds">The time the animation will take</param>
    /// <param name="firstLoad">Indicates if this is the first load</param>
    /// <returns></returns>
    public static async Task FadeInAsync(this FrameworkElement element, bool firstLoad, float seconds = 0.3f)
    {
        var sb = new Storyboard();

        sb.AddFadeIn(seconds);

        sb.Begin(element);

        if (seconds != 0 || firstLoad)
            element.Visibility = Visibility.Visible;

        await Task.Delay((int)(seconds * 1000));
    }

    /// <summary>
    /// Fades out an element
    /// </summary>
    /// <param name="element">The element to animate</param>
    /// <param name="seconds">The time the animation will take</param>
    /// <returns></returns>
    public static async Task FadeOutAsync(this FrameworkElement element, float seconds = 0.3f)
    {
        var sb = new Storyboard();

        sb.AddFadeOut(seconds);

        sb.Begin(element);

        if (seconds != 0)
            element.Visibility = Visibility.Visible;

        await Task.Delay((int)(seconds * 1000));

        element.Visibility = Visibility.Collapsed;
    }
}