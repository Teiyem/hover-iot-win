using System;
using System.Windows.Input;

namespace Hover.Commands;

/// <summary>
/// A basic command that runs an Action
/// </summary>
public class RelayParameterizedCommand : ICommand
{
    /// <summary>
    /// The action to run
    /// </summary>
    private readonly Action<object> mAction;

    /// <summary>
    /// The event that's fired when the <see cref="CanExecute(object)"/> value has changed
    /// </summary>
    public event EventHandler CanExecuteChanged = (sender, e) => { };

    /// <summary>
    /// Default constructor
    /// </summary>
    public RelayParameterizedCommand(Action<object> action)
    {
        mAction = action;
    }

    /// <summary>
    /// A relay command can always execute
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public bool CanExecute(object parameter)
    {
        return true;
    }

    /// <summary>
    /// Executes the commands Action
    /// </summary>
    /// <param name="parameter"></param>
    public void Execute(object parameter)
    {
        mAction(parameter);
    }
}