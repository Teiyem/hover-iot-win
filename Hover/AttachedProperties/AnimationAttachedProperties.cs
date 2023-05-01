using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Hover.Animation;
using Hover.Models;

namespace Hover.AttachedProperties;

#region Attribution
// Code attribution: Original Author Luke Malpass (Angel Six)
// Note: Few changes made
#endregion

/// <summary>
/// A base class to run any animation method when a boolean is set to true.
/// and a reverse animation when set to false.
/// </summary>
/// <typeparam name="TParent"></typeparam>
public abstract class AnimateBaseProperty<TParent> : BaseAttachedProperty<TParent, bool>
    where TParent : BaseAttachedProperty<TParent, bool>, new()
{
    /// <summary>
    /// True if this is the very first time the value has been updated.
    /// Used to make sure we run the logic at least once during first load.
    /// </summary>
    internal Dictionary<WeakReference, bool> AlreadyLoaded = new();

    /// <summary>
    /// The most recent value used if we get a value changed before we do the first load
    /// </summary>
    internal Dictionary<WeakReference, bool> FirstLoadValue = new();

    /// <summary>
    /// Fired.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="value"></param>
    public override void OnValueUpdated(DependencyObject sender, object value)
    {
        if (sender is not FrameworkElement element)
            return;

        var (alKey, alVal) = AlreadyLoaded.FirstOrDefault(f => Equals(f.Key.Target, sender));

        if ((bool)sender.GetValue(ValueProperty) == (bool)value && alKey != null)
            return;

        if (alKey == null)
        {
            var weakReference = new WeakReference(sender);

            AlreadyLoaded[weakReference] = false;

            element.Visibility = Visibility.Hidden;

            async void OnLoaded(object ss, RoutedEventArgs ee)
            {
                element.Loaded -= OnLoaded;

                await Task.Delay(5);

                var (flKey, flVal) = FirstLoadValue.FirstOrDefault(f => Equals(f.Key.Target, sender));

                DoAnimation(element, flKey != null ? flVal : (bool) value, true);

                AlreadyLoaded[weakReference] = true;
            }
            element.Loaded += OnLoaded;
        }

        else if (alVal == false)
            FirstLoadValue[new WeakReference(sender)] = (bool)value;
        else
            DoAnimation(element, (bool)value, false);
    }

    /// <summary>
    /// The animation method that is fired when the value changes.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="value">The new value.</param>
    /// <param name="firstLoad">If it is the first load.</param>
    protected virtual void DoAnimation(FrameworkElement element, bool value, bool firstLoad) { }
}

/// <summary>
/// Animates a framework element sliding it in from the left on show.
/// and sliding out to the left on hide.
/// </summary>
public class AnimateSlideInFromLeftProperty : AnimateBaseProperty<AnimateSlideInFromLeftProperty>
{
    protected override async void DoAnimation(FrameworkElement element, bool value, bool firstLoad)
    {
        if (value)
            await element.SlideAndFadeInAsync(AnimationDirection.Left, firstLoad, firstLoad ? 0 : 0.3f, false);
        else
            await element.SlideAndFadeOutAsync(AnimationDirection.Left, firstLoad ? 0 : 0.3f, false);
    }
}

/// <summary>
/// Animates a framework element sliding it in from the left on show.
/// and sliding out to the left on hide.
/// </summary>
public class AnimateSlideInFromLeftMarginProperty : AnimateBaseProperty<AnimateSlideInFromLeftMarginProperty>
{
    protected override async void DoAnimation(FrameworkElement element, bool value, bool firstLoad)
    {
        if (value)
            await element.SlideAndFadeInAsync(AnimationDirection.Left, firstLoad, firstLoad ? 0 : 0.3f);
        else
            await element.SlideAndFadeOutAsync(AnimationDirection.Left, firstLoad ? 0 : 0.3f);
    }
}

/// <summary>
/// Animates a framework element sliding it in from the right on show.
/// and sliding out to the right on hide.
/// </summary>
public class AnimateSlideInFromRightProperty : AnimateBaseProperty<AnimateSlideInFromRightProperty>
{
    protected override async void DoAnimation(FrameworkElement element, bool value, bool firstLoad)
    {
        if (value)
            await element.SlideAndFadeInAsync(AnimationDirection.Right, firstLoad, firstLoad ? 0 : 0.3f, false);
        else
            await element.SlideAndFadeOutAsync(AnimationDirection.Right, firstLoad ? 0 : 0.3f, false);
    }
}

/// <summary>
/// Animates a framework element sliding it in from the right on show.
/// and sliding out to the right on hide.
/// </summary>
public class AnimateSlideInFromRightMarginProperty : AnimateBaseProperty<AnimateSlideInFromRightMarginProperty>
{
    protected override async void DoAnimation(FrameworkElement element, bool value, bool firstLoad)
    {
        if (value)
            await element.SlideAndFadeInAsync(AnimationDirection.Right, firstLoad, firstLoad ? 0 : 0.3f);
        else
            await element.SlideAndFadeOutAsync(AnimationDirection.Right, firstLoad ? 0 : 0.3f);
    }
}

/// <summary>
/// Animates a framework element sliding down from the top on show.
/// and sliding out to the top on hide.
/// </summary>
public class AnimateSlideInFromTopProperty : AnimateBaseProperty<AnimateSlideInFromTopProperty>
{
    protected override async void DoAnimation(FrameworkElement element, bool value, bool firstLoad)
    {
        if (value)
            await element.SlideAndFadeInAsync(AnimationDirection.Top, firstLoad, firstLoad ? 0 : 0.3f, false);
        else
            await element.SlideAndFadeOutAsync(AnimationDirection.Top, firstLoad ? 0 : 0.3f, false);
    }
}

/// <summary>
/// Animates a framework element sliding up from the bottom on show.
/// and sliding out to the bottom on hide.
/// </summary>
public class AnimateSlideInFromBottomProperty : AnimateBaseProperty<AnimateSlideInFromBottomProperty>
{
    protected override async void DoAnimation(FrameworkElement element, bool value, bool firstLoad)
    {
        if (value)
            await element.SlideAndFadeInAsync(AnimationDirection.Bottom, firstLoad, firstLoad ? 0 : 0.3f, false);
        else
            await element.SlideAndFadeOutAsync(AnimationDirection.Bottom, firstLoad ? 0 : 0.3f, false);
    }
}

/// <summary>
/// Animates a framework element sliding up from the bottom on load.
/// if the value is true.
/// </summary>
public class AnimateSlideInFromBottomOnLoadProperty : AnimateBaseProperty<AnimateSlideInFromBottomOnLoadProperty>
{
    protected override async void DoAnimation(FrameworkElement element, bool value, bool firstLoad)
    {
        await element.SlideAndFadeInAsync(AnimationDirection.Bottom, !value, !value ? 0 : 0.3f, false);
    }
}

/// <summary>
/// Animates a framework element sliding up from the bottom on show.
/// and sliding out to the bottom on hide and keeps the margin.
/// </summary>
public class AnimateSlideInFromBottomMarginProperty : AnimateBaseProperty<AnimateSlideInFromBottomMarginProperty>
{
    protected override async void DoAnimation(FrameworkElement element, bool value, bool firstLoad)
    {
        if (value)
            await element.SlideAndFadeInAsync(AnimationDirection.Bottom, firstLoad, firstLoad ? 0 : 0.3f);
        else
            await element.SlideAndFadeOutAsync(AnimationDirection.Bottom, firstLoad ? 0 : 0.3f);
    }
}

/// <summary>
/// Animates a framework element fading in on show.
/// and fading out on hide.
/// </summary>
public class AnimateFadeInProperty : AnimateBaseProperty<AnimateFadeInProperty>
{
    protected override async void DoAnimation(FrameworkElement element, bool value, bool firstLoad)
    {
        if (value)
            await element.FadeInAsync(firstLoad, firstLoad ? 0 : 0.3f);
        else
            await element.FadeOutAsync(firstLoad ? 0 : 0.3f);
    }
}