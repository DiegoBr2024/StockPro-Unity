using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using static Manager;

public class editor : MonoBehaviour
   
{
    public GameObject editorobject;
    public GameObject nome;
    public GameObject textome;
    public GameObject quantidade;
    public GameObject preco;
    public ManagerUI managerUI;
    public Manager manager;
    public Materialestoque materialestoque { get; private set; }    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    


    // Update is called once per frame
    void Update()
    {
        
    }
    public void editar(Materialestoque material)
    {
       materialestoque = material;
    }
    public void click()
    {
        showeditoritem();
        Debug.Log(materialestoque.Nome);

        managerUI.showeditor();
    }

    private void showeditoritem()
    {
        textome.GetComponent<TextMeshProUGUI>().text = materialestoque.Nome;
        nome.GetComponent<TMP_InputField>().text = materialestoque.Nome;
        quantidade.GetComponent<TMP_InputField>().text = materialestoque.Quantidade.ToString();
        preco.GetComponent<TMP_InputField>().text = materialestoque.PrecoDeCompra.ToString("F2");

        manager.ArmazenarDados(materialestoque);
    }
   
    private void shoeitemUp()
    {
       
    }
    public void close()
    {
        managerUI.showeditor();

    }
    

}
