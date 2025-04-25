/*
* FileName:          GameobjectCommand
* CompanyName:       
* Author:            
* Description:       
*/

using UnityEngine;

public class GameobjectCommand : ICommand
{
    private GameObject _gameObject;
    private bool _oldVisibility;
    private bool _newVisibility;

    public GameobjectCommand(GameObject gameObject, bool oldVisibility, bool newVisibility)
    {
        _gameObject = gameObject;
        _oldVisibility = oldVisibility;
        _newVisibility = newVisibility;
    }

    public void Execute()
    {
        _gameObject.SetActive(_newVisibility);
    }

    public void Undo()
    {
        _gameObject.SetActive(_oldVisibility);
    }
}
