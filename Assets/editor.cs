using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using static Manager;

public class editor : MonoBehaviour
   
{
    public GameObject editorobject;
    public TMPro.TMP_InputField nome;
    public GameObject textome;
    public TMP_InputField quantidade;
    public TMP_InputField precoDeVenda;
    public TMP_InputField precoDeCompra;
    public ManagerUI managerUI;

    public TextMeshProUGUI nomePrevia;
    public TextMeshProUGUI quantidadePrevia;
    public TextMeshProUGUI precoDeVendaPrevia;
    public TextMeshProUGUI precoDeCompraPrevia;
    public TextMeshProUGUI LucroPorProduto;
    public TextMeshProUGUI Lucrototal;

    public Manager manager;

    bool isDragging = false;
    public GameObject Adicionar;

    public Materialestoque materialestoque { get; private set; }    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void Finalizareditar(Materialestoque material)
    {

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
        editorobject.SetActive(!isDragging);
        Adicionar.SetActive(isDragging);
        Debug.Log(materialestoque.Nome);
        isDragging = !isDragging;
        var editorfinal = FindFirstObjectByType<EditorFinal>().materialestoqueantigo = materialestoque;
               // managerUI.showeditor();
    }
    public void Obterdados()
    {
        print("Obtendo dados...");
        materialestoque = new Materialestoque();

        try
        {
            materialestoque.Nome = nome.text.ToString();
            materialestoque.Quantidade = int.Parse(quantidade.text);
            materialestoque.PrecoDeCompra = float.Parse(precoDeCompra.text);
            materialestoque.PrecoDeVenda = float.Parse(precoDeVenda.text);

        }
        catch (System.Exception e)
        {
            managerUI.showErrorMessage("Erro ao obter os dados. Por favor, verifique os campos e tente novamente.");
            Debug.LogError("Erro ao obter os dados: " + e.Message);
            return;
        }
        if (string.IsNullOrEmpty(materialestoque.Nome) || materialestoque.Quantidade <= 0 || materialestoque.PrecoDeCompra <= 0)
        {
            managerUI.showErrorMessage("Dados inválidos. Por favor, preencha todos os campos corretamente.");
            print(materialestoque.Nome + materialestoque.Quantidade + materialestoque.PrecoDeCompra);

            return;
        }

       managerUI.GetComponent<Adicionar>().Adicionaritem(materialestoque);
    }

    private void showeditoritem()
    {
       
        nome.GetComponent<TMP_InputField>().text = materialestoque.Nome;
        quantidade.GetComponent<TMP_InputField>().text = materialestoque.Quantidade.ToString();
        precoDeVenda.GetComponent<TMP_InputField>().text = materialestoque.PrecoDeVenda.ToString("F2");
        precoDeCompra.GetComponent<TMP_InputField>().text = materialestoque.PrecoDeCompra.ToString("F2");

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
