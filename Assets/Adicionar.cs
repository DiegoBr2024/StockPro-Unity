using System.IO;
using TMPro;
using UnityEngine;
using static Manager;
using static ManagerUI;

public class Adicionar : MonoBehaviour
{
    public TMPro.TMP_InputField nome;
    public TextMeshProUGUI nomePrevia;
    public TMP_InputField quantidade;
    public TextMeshProUGUI quantidadePrevia;
    public TMP_InputField precoDeCompra;
    public TextMeshProUGUI precoDeCompraPrevia;
    public TMP_InputField precoDeVenda;
    public TextMeshProUGUI precoDeVendaPrevia;

    public TextMeshProUGUI LucroPorProduto;
    public TextMeshProUGUI Lucrototal;
    public Manager manager;
    public ManagerUI managerUI;
    private string caminho;
    private Materialestoque materialestoque;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        managerUI = GetComponent<ManagerUI>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void Awake()
    {
        caminho = Path.Combine(Directory.GetCurrentDirectory(), "estoque.json");

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
        try{
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
    public void Obterdados()
    { 
        print("Obtendo dados...");
        materialestoque = new Materialestoque();

        try
        {
            materialestoque.Nome = nome.text.ToString();
            materialestoque.Quantidade = int.Parse(quantidade.text);
            materialestoque.PrecoDeCompra = float.Parse(precoDeCompra.text);
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
            print(materialestoque.Nome+materialestoque.Quantidade+materialestoque.PrecoDeCompra);

            return;
        }
        
        Adicionaritem(materialestoque);
    }

    public void Adicionaritem(Materialestoque material)
    {
        Limpar();
        print("Adicionando item...");
        Estoque estoque = new Estoque();
        string json = File.ReadAllText(caminho);
        JsonUtility.FromJson<Estoque>(json);
        if (json != null && json != "")
        {
            Estoque jsonnn = JsonUtility.FromJson<Estoque>(json);
            estoque.materiais.AddRange(jsonnn.materiais);
            estoque.materiais.Add(material);
        }
        string jsonsaver = JsonUtility.ToJson(estoque, true);

        File.WriteAllText(caminho, jsonsaver);
       
        //manager.showaddnow(material); 
        //congelado por falta de necessidade.
    }
}
