/*
* FileName:          ScaleCommand
* CompanyName:       
* Author:            
* Description:       
*/

using UnityEngine;

public class ScaleCommand : ICommand
{
    private Transform _transform;
    private Vector3 _oldScale;
    private Vector3 _newScale;

    public ScaleCommand(Transform transform, Vector3 oldScale, Vector3 newScale)
    {
        _transform = transform;
        _oldScale = oldScale;
        _newScale = newScale;
    }

    public void Execute()
    {
        _transform.localScale = _newScale;
    }

    public void Undo()
    {
        _transform.localScale = _oldScale;
    }
}
