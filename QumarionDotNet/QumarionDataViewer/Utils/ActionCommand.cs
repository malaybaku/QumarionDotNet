using System;
using System.Windows.Input;

namespace QumarionDataViewer
{
    /// <summary>ジェネリックアクションを用いた単純なコマンド実装です。</summary>
    public class ActionCommand : ICommand
    {
        public ActionCommand(Action<object> action)
        {
            TargetAction = action;
        }
        public ActionCommand(Action action) 
            : this(_ => action())
        {

        }

        public Action<object> TargetAction { get; }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) => TargetAction(parameter);
    }
}
