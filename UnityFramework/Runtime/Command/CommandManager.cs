/*
* FileName:          CommandManager
* CompanyName:       
* Author:            
* Description:       
*/



using System.Collections.Generic;

public class CommandManager
{
    private Stack<List<ICommand>> _commandHistory = new Stack<List<ICommand>>();
    private Stack<List<ICommand>> _undoHistory = new Stack<List<ICommand>>();

    public void ExecuteCommand(List<ICommand> command)
    {
        foreach (ICommand cmd in command)
        {
            cmd.Execute();
        }
        _commandHistory.Push(command);
        _undoHistory.Clear(); // 如果执行了新的命令，则丢弃之前的撤销历史
    }

    public void Undo()
    {
        if (_commandHistory.Count > 0)
        {
            List<ICommand> command = _commandHistory.Pop();
            foreach (ICommand cmd in command)
            {
                cmd.Undo();
            }
            _undoHistory.Push(command);
        }
    }

    public void Redo()
    {
        if (_undoHistory.Count > 0)
        {
            List<ICommand> command = _undoHistory.Pop();
            foreach (ICommand cmd in command)
            {
                cmd.Execute();
            }
            _commandHistory.Push(command);
        }
    }
}
