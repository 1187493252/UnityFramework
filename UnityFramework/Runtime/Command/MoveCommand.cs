/*
* FileName:          MoveCommand
* CompanyName:       
* Author:            
* Description:       
*/

using UnityEngine;

public class MoveCommand : ICommand
{
    private Transform _transform;
    private Vector3 _oldPosition;
    private Vector3 _newPosition;

    public MoveCommand(Transform transform, Vector3 oldPosition, Vector3 newPosition)
    {
        _transform = transform;
        _oldPosition = oldPosition;
        _newPosition = newPosition;
    }

    public void Execute()
    {
        _transform.position = _newPosition;
    }

    public void Undo()
    {
        _transform.position = _oldPosition;
    }
}
