using System;
using System.Windows.Input;

namespace Hover.Commands;

/// <summary>
/// A basic command that runs an action.
/// </summary>
public class RelayCommand : ICommand
{
    /// <summary>
    /// The action to run.
    /// </summary>
    private readonly Action mAction;

    /// <summary>
    /// The parameterized action to run.
    /// </summary>
    private readonly Action<object> mParamAction;

    /// <summary>
    /// The event that is fired when the <see cref="CanExecute(object)"/> value has changed.
    /// </summary>
    public event EventHandler CanExecuteChanged = (sender, e) => { };

    /// <summary>
    /// Initializes a new instance of the <see cref="RelayCommand"/> class.
    /// </summary>
    /// <param name="action">The action to run.</param>
    public RelayCommand(Action action)
    {
        mAction = action;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RelayCommand"/> class.
    /// </summary>
    /// <param name="action">The parameterized action to run.</param>
    public RelayCommand(Action<object> action)
    {
        mParamAction = action;
    }

    /// <summary>
    /// A relay command can always execute.
    /// </summary>
    /// <param name="parameter"> The data used by the command if any.</param>
    /// <returns> Always return true.</returns>
    public bool CanExecute(object parameter) => true;

    /// <summary>
    /// Executes the command actions.
    /// </summary>
    /// <param name="parameter">The parameter being passed.</param>
    public void Execute(object parameter)
    {
        mAction?.Invoke();
        mParamAction?.Invoke(parameter);
    }
}