using UnityEngine;
using static Manager;

public class DeleteItem : MonoBehaviour
{
    [SerializeField]
    private editor editor;
    private Manager manager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        manager = FindFirstObjectByType<Manager>();
    }

    public void deleteItem()
    {
        Materialestoque materialS = editor.materialestoque;
        print(materialS.Nome);
        manager.DeleteItem(materialS);

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
