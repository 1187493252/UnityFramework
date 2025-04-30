/*
* FileName:          GameObjectHierarchySaver
* CompanyName:       
* Author:            
* Description:       缓存指定GameObject下的子物体结构
*/

using System.Collections.Generic;
using UnityEngine;

public class GameObjectHierarchySaver : MonoBehaviour
{
    public GameObject Root;
    GameObjectHierarchy Hierarchy;
    private void Awake()
    {
        Init();
    }

    void Init()
    {
        if (Root == null)
        {
            Root = this.gameObject;
        }
        Hierarchy = new GameObjectHierarchy(Root);
        SaveGameObjectHierarchy(Root, Hierarchy);
    }

    private void OnEnable()
    {
    }

    void SaveGameObjectHierarchy(GameObject parent, GameObjectHierarchy parentChilds)
    {
        foreach (Transform childTransform in parent.transform)
        {
            GameObject childGameObject = childTransform.gameObject;
            GameObjectHierarchy childChilds = new GameObjectHierarchy(childGameObject);
            parentChilds.children.Add(childChilds);
            SaveGameObjectHierarchy(childGameObject, childChilds);
        }
    }


    public void GetAllGameObjectsChilds(string targetName, List<GameObject> gameObjects)
    {
        GameObjectHierarchy gameObjectChilds = FindGameObjectHierarchyByName(targetName);
        if (gameObjectChilds != null)
        {
            GetAllGameObjectsChilds(gameObjectChilds, gameObjects);
        }
    }
    public void GetAllGameObjectsChilds(GameObjectHierarchy root, List<GameObject> gameObjects)
    {
        gameObjects.Add(root.gameObject);
        foreach (GameObjectHierarchy child in root.children)
        {
            GetAllGameObjectsChilds(child, gameObjects);
        }
    }
    public void SetAllGameObjectActive(string targetName, bool isActive)
    {
        GameObjectHierarchy gameObjectChilds = FindGameObjectHierarchyByName(targetName);
        if (gameObjectChilds != null)
        {
            SetAllGameObjectActive(gameObjectChilds, isActive);
        }
    }

    public void SetAllGameObjectActive(GameObjectHierarchy root, bool isActive)
    {
        root.gameObject.SetActive(isActive);
        foreach (GameObjectHierarchy child in root.children)
        {
            SetAllGameObjectActive(child, isActive);
        }
    }

    public GameObjectHierarchy FindGameObjectHierarchyByName(string targetName)
    {
        return FindGameObjectHierarchyByName(Hierarchy, targetName);
    }
    GameObjectHierarchy FindGameObjectHierarchyByName(GameObjectHierarchy root, string targetName)
    {
        if (root.name == targetName)
        {
            return root;
        }

        foreach (GameObjectHierarchy child in root.children)
        {
            GameObjectHierarchy found = FindGameObjectHierarchyByName(child, targetName);
            if (found != null)
            {
                return found;
            }
        }

        return null;
    }
}

public class GameObjectHierarchy
{
    public string name;
    public GameObject gameObject;
    public List<GameObjectHierarchy> children;

    public GameObjectHierarchy(GameObject go)
    {
        this.name = go.name;
        this.gameObject = go;
        this.children = new List<GameObjectHierarchy>();
    }
}
