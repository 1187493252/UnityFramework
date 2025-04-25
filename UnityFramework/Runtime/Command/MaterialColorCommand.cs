/*
* FileName:          MaterialColorCommand
* CompanyName:       
* Author:            
* Description:       
*/

using UnityEngine;

public class MaterialColorCommand : ICommand
{
    private Renderer _renderer;
    private Color _oldColor;
    private Color _newColor;

    public MaterialColorCommand(Renderer renderer, Color oldColor, Color newColor)
    {
        _renderer = renderer;
        _oldColor = oldColor;
        _newColor = newColor;
    }

    public void Execute()
    {
        _renderer.material.color = _newColor;
    }

    public void Undo()
    {
        _renderer.material.color = _oldColor;
    }
}
