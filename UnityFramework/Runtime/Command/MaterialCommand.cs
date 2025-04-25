/*
* FileName:          MaterialCommand
* CompanyName:       
* Author:            
* Description:       
*/

using System.Collections.Generic;
using UnityEngine;

public class MaterialCommand : ICommand
{
    private Renderer _renderer;
    private List<Material> _oldMaterials;
    private List<Material> _newMaterials;

    public MaterialCommand(Renderer renderer, List<Material> oldMaterials, List<Material> newMaterials)
    {
        _renderer = renderer;
        _oldMaterials = oldMaterials;
        _newMaterials = newMaterials;
    }

    public void Execute()
    {
        _renderer.materials = _newMaterials.ToArray();
    }

    public void Undo()
    {
        _renderer.materials = _oldMaterials.ToArray();
    }
}
