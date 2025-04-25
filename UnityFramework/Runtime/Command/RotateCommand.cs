/*
* FileName:          RotateCommand
* CompanyName:       
* Author:            
* Description:       
*/

using UnityEngine;

public class RotateCommand : ICommand
{
    private Transform _transform;
    private Quaternion _oldRotation;
    private Quaternion _newRotation;

    public RotateCommand(Transform transform, Quaternion oldRotation, Quaternion newRotation)
    {
        _transform = transform;
        _oldRotation = oldRotation;
        _newRotation = newRotation;
    }

    public void Execute()
    {
        _transform.rotation = _newRotation;
    }

    public void Undo()
    {
        _transform.rotation = _oldRotation;
    }
}

