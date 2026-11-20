using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static Manager;

public class EditorFinal : MonoBehaviour
{
    ManagerUI managerUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        managerUI = FindFirstObjectByType<ManagerUI>();
        manager = FindFirstObjectByType<Manager>();
       // EditorDisable();
    }

    // Update is called once per frame
    void Update()
    {
        
 
    
    }
    [SerializeField]
    GameObject editorobject;
    [SerializeField]
    GameObject adicionar;
    bool isDragging = false;
    public TMPro.TMP_InputField nome;
    public GameObject textome;
    public TMP_InputField quantidade;
    public TMP_InputField precoDeVenda;
    public TMP_InputField precoDeCompra;
    public TextMeshProUGUI nomePrevia;
    public TextMeshProUGUI quantidadePrevia;    
    public TextMeshProUGUI precoDeCompraPrevia;
    public TextMeshProUGUI precoDeVendaPrevia;
    public TextMeshProUGUI LucroPorProduto;    
    public TextMeshProUGUI Lucrototal;
    Estoque estoque = new Estoque();
    Manager manager;
    public Materialestoque materialestoque { get; private set; }
    public Materialestoque materialestoqueantigo;


    public void PreviaDoProduto()
    {
        try
        {
            nomePrevia.text = nome.text;
            quantidadePrevia.text = quantidade.text;
            precoDeCompraPrevia.text = precoDeCompra.text;
            precoDeVendaPrevia.text = precoDeVenda.text;
            LucroPorProduto.text = (float.Parse(precoDeVenda.text) - float.Parse(precoDeCompra.text)).ToString();
            Lucrototal.text = (float.Parse(quantidade.text) * (float.Parse(precoDeVenda.text) - float.Parse(precoDeCompra.text))).ToString();
        }
        catch (System.Exception e)
        {
            //sem tratamento de erro, pois a previa é apenas uma visualização,
            //não é necessário que o usuário preencha todos os campos para ver a previa.
        }
    }
    public void Limpar()
    {
        nome.text = "";
        quantidade.text = "";
        precoDeCompra.text = "";
        precoDeVenda.text = "";
        nomePrevia.text = "";
        quantidadePrevia.text = "";
        precoDeCompraPrevia.text = "";
        precoDeVendaPrevia.text = "";
        LucroPorProduto.text = "";
        Lucrototal.text = "";
    }

    public void Obterdados()
    {
        print("Obtendo dados...");

        materialestoque = new Materialestoque();

        try
        {
            materialestoque.Nome = nome.text;
            materialestoque.Quantidade = int.Parse(quantidade.text);
            materialestoque.PrecoDeCompra = float.Parse(precoDeCompra.text);
            materialestoque.PrecoDeVenda = float.Parse(precoDeVenda.text);
        }
        catch (System.Exception e)
        {
            managerUI.showErrorMessage("Erro ao obter os dados.");
            Debug.LogError(e.Message);
            return;
        }

        // Remove o item antigo antes de adicionar o novo
        if (materialestoqueantigo != null)
        {
            estoque.materiais.RemoveAll(x =>
                x.Nome == materialestoqueantigo.Nome);
        }

        manager.AtualizarMaterial(
     materialestoqueantigo,
     materialestoque
 );

        EditorDisable();
        Limpar();
    }

   

    private void EditorDisable()
    {
        editorobject.SetActive(isDragging);
        adicionar.SetActive(!isDragging);
        isDragging = !isDragging;
    }
}
