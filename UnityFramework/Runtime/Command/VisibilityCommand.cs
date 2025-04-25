/*
* FileName:          VisibilityCommand
* CompanyName:       
* Author:            
* Description:       
*/

using System.Collections.Generic;
using UnityEngine;

public class VisibilityCommand : ICommand
{
    private List<GameObject> _oldShowGameObjects;
    private List<GameObject> _newShowGameObjects;
    private List<GameObject> _oldHideGameObjects;
    private List<GameObject> _newHideGameObjects;

    public VisibilityCommand(List<GameObject> oldShowGameObjects, List<GameObject> oldHideGameObjects, List<GameObject> newShowGameObjects, List<GameObject> newHideGameObjects)
    {
        _oldShowGameObjects = oldShowGameObjects;
        _oldHideGameObjects = oldHideGameObjects;
        _newShowGameObjects = newShowGameObjects;
        _newHideGameObjects = newHideGameObjects;
    }

    public void Execute()
    {
        foreach (GameObject gameObject in _newShowGameObjects)
        {
            gameObject.SetActive(true);
        }

        foreach (GameObject gameObject in _newHideGameObjects)
        {
            gameObject.SetActive(false);
        }
    }



    public void Undo()
    {
        foreach (GameObject gameObject in _oldShowGameObjects)
        {
            gameObject.SetActive(true);
        }

        foreach (GameObject gameObject in _oldHideGameObjects)
        {
            gameObject.SetActive(false);
        }
    }
}
