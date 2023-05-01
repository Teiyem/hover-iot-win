using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hover.Helpers;

namespace Hover.ViewModels;

#region Attribution
// Code attribution: Original Author Luke Malpass (Angel Six)
#endregion

/// <summary>
/// A base class for all ViewModels that fires Property Changed events as needed.
/// </summary>
public class BaseViewModel : INotifyPropertyChanged
{
    /// <summary>
    /// A global lock for property checks so prevent locking on different instances of expressions.
    /// Considering how fast this check will always be it isn't an issue to globally lock all callers.
    /// </summary>
    protected object mPropertyValueCheckLock = new();

    /// <summary>
    /// A delegate that is used to notify the XAML UI that a property has changed.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

    /// <summary>
    /// Calls the <see cref="PropertyChanged"/> event with the name of the property as the parameter.
    /// </summary>
    /// <param name="name">The name of the property that changed.</param>
    public void OnPropertyChanged(string name)
    {
        PropertyChanged(this, new PropertyChangedEventArgs(name));
    }

    /// <summary>
    /// Runs a command if the updating flag is not set.
    /// If the flag is true (indicating the function is already running) then the action is not run.
    /// If the flag is false (indicating no running function) then the action is run.
    /// Once the action is finished if it was run, then the flag is reset to false
    /// </summary>
    /// <param name="updatingFlag">The boolean property flag defining if the command is already running</param>
    /// <param name="action">The action to run if the command is not already running</param>
    /// <returns></returns>
    protected async Task RunCommandAsync(Expression<Func<bool>> updatingFlag, Func<Task> action)
    {
        lock (mPropertyValueCheckLock)
        {
            if (updatingFlag.GetPropertyValue())
                return;
            updatingFlag.SetPropertyValue(true);
        }

        try
        {
            await action();
        }
        finally
        {
            updatingFlag.SetPropertyValue(false);
        }
    }
}