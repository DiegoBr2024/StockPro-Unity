using JetBrains.Annotations;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static Manager;
public class ManagerUI : MonoBehaviour
{
    Estoque esoque = new Estoque();
    public string caminho;
    public string caminhosavercompras;
    public GameObject manager;
    public GameObject MensagemUI;
    public GameObject config;
    public Transform adicionaritem;
    public GameObject editorobject;
    public List<GameObject> lists = new List<GameObject>();

    public ExibirClientes exibirClientes;

    public GameObject Vendas;
    public GameObject PainelClientes;
    public GameObject estoque;
    public GameObject Showestoque;
    public GameObject Vendidos;
    public GameObject PositionSelectProductname;
    public GameObject SelectProductname;
    public TextMeshProUGUI Precototal;
    public TMP_InputField NomeCliente;
    public TMP_InputField BarraDePesquisa;
    public Button PrecisarEntregarButton;
    public Button PagamentoEfetuado;

    public bool isAdding = false;
    public bool mensagem = false;
    private bool isEditing = false;

    private Vector2 positionselectproduct = new Vector2(-330, 55);

    public List<string> productNames = new List<string>();

    private Coroutine moverCoroutine;
    private Color originalColorButton = new Color(147f/255f, 147f/255f, 147f/255f, 0);
    private Color ClickedColorButton = new Color(75f/255f, 152f/255f, 234f/255f, 1);
    public Estoque estoquedeprodutos = new Estoque();
    public ExibirProdutosVendidos exibirProdutosVendidos;
    Venda venda = new Venda();
    public TMPro.TMP_InputField LocalizacaoDaEntrega;
    public bool PrecisaEntrgar = false;
    public bool EstaPago = false;

    private Image buttonVendas;
    private Image buttonEstoque;
    private Image buttonVendidos;
    private Image buttonClientes;
    [SerializeField]
    private GameObject ComprasClientes;
    public Config configuration;

    [System.Serializable]
    public class SaverComprasList
    {
        public List<Materialestoque> Compras = new List<Materialestoque>();
    }

    public enum SetorExibido{
        Vendas,
        Estoque,
        Vendidos,
        Clientes
    }
    SetorExibido setorExibido = new SetorExibido();
    public List<Materialestoque> SaverCompra = new List<Materialestoque>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      
        caminho = Path.Combine(Directory.GetCurrentDirectory(), "estoque.json");

        buttonVendas = GameObject.Find("Vendas").GetComponent<Image>();
        buttonEstoque = GameObject.Find("Estoque").GetComponent<Image>();
        buttonVendidos = GameObject.Find("Vendidos").GetComponent<Image>();
        buttonClientes = GameObject.Find("Clientes").GetComponent<Image>();





        buttonEstoque.color = ClickedColorButton;
        buttonVendas.color = originalColorButton;
        buttonVendidos.color = originalColorButton;
        buttonClientes.color = originalColorButton;

        StartCoroutine(Movereditor());
        moverCoroutine = StartCoroutine(Mover());
        ActiveEstoque();
        caminhosavercompras = Path.Combine(Directory.GetCurrentDirectory() , "Compras.json");
        print( caminhosavercompras);
    }

    private void Update()
    {
        float scroll = Mouse.current.scroll.y.ReadValue();
        
         if (setorExibido == SetorExibido.Estoque)
        {
            Showestoque.GetComponent<RectTransform>().localPosition += new Vector3(0, scroll * 1000f, 0) * Time.deltaTime;
        }
        else if (setorExibido == SetorExibido.Vendidos)
        {
            ComprasClientes.GetComponent<RectTransform>().localPosition += new Vector3(0, scroll * 1000f, 0) * Time.deltaTime;
        }
        else if (setorExibido == SetorExibido.Clientes)
        {
            PainelClientes.GetComponent<RectTransform>().localPosition += new Vector3(0, scroll * 1000f, 0) * Time.deltaTime;
        }
    }

    public void ActiveVendas()
    {
        setorExibido = SetorExibido.Vendas;
        buttonVendas.color = ClickedColorButton;
        buttonEstoque.color = originalColorButton;
        buttonVendidos.color = originalColorButton;
        buttonClientes.color = originalColorButton;

        Vendas.SetActive(true);
        estoque.SetActive(false);
        Showestoque.SetActive(false);
        Vendidos.SetActive(false);
        PainelClientes.SetActive(false);
    }
    public void ActiveEstoque()
    {
        setorExibido = SetorExibido.Estoque;
        buttonEstoque.color = ClickedColorButton;
        buttonVendas.color = originalColorButton;
        buttonVendidos.color = originalColorButton;
        buttonClientes.color = originalColorButton;

        Vendas.SetActive(false);
        estoque.SetActive(true);
        Showestoque.SetActive(true);
        Vendidos.SetActive(false);
        PainelClientes.SetActive(false);
    }
    public void ActiveProdutosVendidos()
    {
        setorExibido = SetorExibido.Vendidos;
        buttonVendidos.color = ClickedColorButton; 
        buttonEstoque.color = originalColorButton;
        buttonVendas.color = originalColorButton;
        buttonClientes.color = originalColorButton;



        Vendas.SetActive(false);
        estoque.SetActive(false);
        Showestoque.SetActive(false);
        Vendidos.SetActive(true);
        PainelClientes.SetActive(false);
        ExibirCompras();
        exibirProdutosVendidos.ClickVendidos("Tudo");
    }
    public void ActiveSetorClientes()
    {
        setorExibido = SetorExibido.Clientes;
        buttonClientes.color = ClickedColorButton;
        buttonEstoque.color = originalColorButton;
        buttonVendidos.color = originalColorButton;
        buttonVendas.color = originalColorButton;




        Vendas.SetActive(false);
        estoque.SetActive(false);
        Showestoque.SetActive(false);
        Vendidos.SetActive(false);
        PainelClientes.SetActive(true);
        exibirClientes.ExibirClientesButton();

    }
    private void ExibirCompras()
    {

    }
    
    public void click()
    {
        
        isAdding = !isAdding;
        if(moverCoroutine != null) 
        {
            StopCoroutine(moverCoroutine);
        }
        moverCoroutine = StartCoroutine(Mover());

    }
    public void finalizar()
    {
        //click();
        GetComponent<Adicionar>().Obterdados();
        manager.GetComponent<Manager>().Reloadestoque(0);  


    }

    IEnumerator Mover()
    {
        Vector2 destino;
        estoque.SetActive(!isAdding);
        if (isAdding)
        {
            print("adicionando");
            destino = new Vector2(75, adicionaritem.localPosition.y);
        }
        else
            destino = new Vector2(1597.89f, adicionaritem.localPosition.y);

        while (Vector2.Distance(adicionaritem.position, destino) > 0.1f)
        {
            adicionaritem.localPosition = Vector2.MoveTowards(
                adicionaritem.localPosition,
                destino,
                2000f * Time.deltaTime
            );

            yield return null; 
        }

        adicionaritem.localPosition = destino;

    }

    public void showErrorMessage(string message)
    {
        mensagem = true;
        MensagemUI.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = message;
        StartCoroutine(ShowMensage());
    }

    IEnumerator ShowMensage()
    {
        while (mensagem && MensagemUI.GetComponent<RectTransform>().localPosition.y <= 400f)
        {
           MensagemUI.GetComponent<RectTransform>().localPosition += new Vector3(0, 1500f, 0) * Time.deltaTime;
            yield return null;
           
        }
        yield return new WaitForSeconds(5f);

        while (mensagem && MensagemUI.GetComponent<RectTransform>().localPosition.y <= 550)
        {
            
            MensagemUI.GetComponent<RectTransform>().localPosition += new Vector3(0, 700f, 0) * Time.deltaTime;
            
            if (MensagemUI.GetComponent<RectTransform>().localPosition.y >= 540) {mensagem = false;
                MensagemUI.GetComponent<RectTransform>().localPosition = new Vector3(MensagemUI.GetComponent<RectTransform>().localPosition.x, -719, MensagemUI.GetComponent<RectTransform>().localPosition.z); }
            yield return null;
        }
    }
    public void showConfig()
    {
        lists[0].SetActive(!lists[0].activeSelf);
        Vendas.SetActive(false);
    }
    private void setcolorlist(Color color)
    {
        //var ObjectsTemas = GameObject.FindGameObjectsWithTag("Temas");
        foreach (GameObject obj in lists)
        {
            obj.GetComponent<UnityEngine.UI.Image>().color = color;
        }
    }
    public void ToBeRed()
    {
        setcolorlist(Color.red);
        showConfig();
    }
    public void ToBeBlue()
    {
        setcolorlist(Color.blue);
        showConfig();
    }
    public void ToBeGreen()
    {
        setcolorlist(Color.green);
        showConfig();
    }


    IEnumerator Movereditor()
    {
        while (isEditing && editorobject.transform.localPosition.y <= -337f)
        {
            editorobject.transform.localPosition += new Vector3(0, 500f, 0) * Time.deltaTime;
            yield return null;
        }
        while (!isEditing && editorobject.transform.localPosition.y >= -769f)
        {
            editorobject.transform.localPosition -= new Vector3(0, 500f, 0) * Time.deltaTime;
            yield return null;
        }

    }
    void Awake()
    {
        //configuration = GameObject.Find("ConfigUI").GetComponent<Config>();
    }

    public void showeditor()
    {
        isEditing = !isEditing;
        if (moverCoroutine != null)
        {
            StopCoroutine(moverCoroutine);
        }
        moverCoroutine = StartCoroutine(Movereditor());
    }
    public List<string> NamesObject = new List<string>();
    public void SelectProduct(string name)
    {
        if (!NamesObject.Contains(name))
        { 
            NamesObject.Add(name);
        }
       
        SaverCompra.Add(ObterDados(name));

        foreach (string productName in productNames)
        {
            if (productName == name)
            {
              //  print("já adicionado");

                var productexistente = PositionSelectProductname.transform.Find(name);

                if (productexistente == null)
                {
                    return;
                }
                var quantidade = productexistente.transform.GetChild(0);


                
                if (!lists[0].GetComponent<Config>().VendaSemEstoquePermission)
                {   var leitura = ObterDados(productName);
                    if (PermissionVendaSemEstoque(int.Parse(quantidade.GetComponent<TMPro.TMP_InputField>().text) + 1
                        , leitura.Quantidade)) 
                    { 
                        showErrorMessage("Venda utrapassa quantidade de produtos em estoque");

                        var timer = StartCoroutine(Timer(1));
                        StopCoroutine(timer);
                        showErrorMessage("Vá em configurações para alterar!");
                        timer = StartCoroutine(Timer(1));
                        StopCoroutine(timer);

                        return;

                    }

                   
                }
                if (quantidade != null)
                {
                    quantidade.GetComponent<TMPro.TMP_InputField>().text =
                        (int.Parse(quantidade.GetComponent<TMPro.TMP_InputField>().text) + 1).ToString();
                    
                var precoo = productexistente.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

                    precoo.text = (float.Parse(quantidade.GetComponent<TMPro.TMP_InputField>().text) * ObterDados(name).PrecoDeCompra).ToString("F2");
                }
                productNames.Add(name);

                ValorTotal();
                return;
            }
        }

       


        productNames.Add(name);

        var product = Instantiate(SelectProductname);
        product.tag = "ProdutoSelecionado";
        product.name = name;

        var nomeproduct = product.GetComponent<TextMeshProUGUI>();
        nomeproduct.text = name;

        var preco = product.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        float productPrice = ObterDados(name).PrecoDeCompra;

        var quantityText = product.transform.GetChild(0).GetComponent<TMPro.TMP_InputField>();
        float quantity = float.Parse(quantityText.text);

       
        
        preco.text = (float.Parse((productPrice * quantity).ToString("F2"))).ToString("F2");
        product.transform.SetParent(PositionSelectProductname.transform, false);

        positionselectproduct.y -= 55;

        product.GetComponent<RectTransform>().localPosition = positionselectproduct;

        ValorTotal();
    }

    private IEnumerator Timer(int tempo)
    {
        yield return new WaitForSeconds(tempo);
    }
    public void AtualizarSoma()
    {
        var produtosSelecionados = PositionSelectProductname.GetComponentsInChildren<BoxCollider2D>();
        string nameproduct = "";
        Materialestoque produtosDaCompra = new Materialestoque();
        foreach (var produto in produtosSelecionados)
        {
            nameproduct = produto.name;
            produtosDaCompra = ObterDados(produto.name);
            var quantidade = produto.transform.Find("quantidade").GetComponent<TMPro.TMP_InputField>();

            var preco = produto.transform.Find("valor").GetComponent<TextMeshProUGUI>();
             
            float valorTotal = produtosDaCompra.PrecoDeCompra * float.Parse(quantidade.text);
            preco.text = valorTotal.ToString("F2");


        }
        ValorTotal();

    }
    private Materialestoque ObterDados(string name)
    {
        CarregamendodeProdutos();
        foreach (var estoquename in estoquedeprodutos.materiais)
        {
            if (name == estoquename.Nome)
            {
                return estoquename;
            }
        }
        return null; // Return null if the product is not found
    }

    private bool PermissionVendaSemEstoque(int Venda , int Estoque)
    {
        if (Estoque >= Venda) { return false; }//Vair Permitir Continuar a Venda
        else { return true; } //vai dar retorno de atribuição de PRODUTOS
    }

    public void ValorTotal()
    {
        CarregamendodeProdutos();
        
        GameObject[] produtosSelecionados = GameObject.FindGameObjectsWithTag("ProdutoSelecionado");
        float quantidadeTotal = 0;
        foreach(GameObject produto in produtosSelecionados)
        {
            print(produto.name);
            quantidadeTotal += float.Parse(produto.transform.Find("valor").GetComponent<TextMeshProUGUI>().text);
        }

        Precototal.text = "R$ " + quantidadeTotal.ToString("F2");
        /* foreach (string name in productNames) {

             foreach (var estoquename in estoquedeprodutos.materiais) {
                 if (name == estoquename.Nome)
                 {
                     total += estoquename.Preco;
                 }

             }

         }
         Precototal.text = "R$ " + total.ToString("F2");*/


    }

    private void CarregamendodeProdutos()
    {
        string caminho = Path.Combine(Directory.GetCurrentDirectory(), "estoque.json");
        if (!File.Exists(caminho)){
            
            
            return;
        }
           

        string jsonn = File.ReadAllText(caminho);

        estoquedeprodutos = JsonUtility.FromJson<Estoque>(jsonn);
    }
    public void FinalizarCompra()
    {
       if(!VerificarExistenciaDeDadosDigitados())
        {
            return;
        }
        SaverComprasExiste();
        ResetDadosDigitados();
    }
    private void ResetDadosDigitados()
    {
        BarraDePesquisa.text = string.Empty;
        NomeCliente.text = string.Empty;
        Precototal.text = "R$ 0,00";
    }

    private bool VerificarExistenciaDeDadosDigitados()
    {
        if(string.IsNullOrEmpty(NomeCliente.text))
        {
            showErrorMessage("Digite o nome do cliente.");
            return false;
        }
        if (string.IsNullOrEmpty(Precototal.text) || Precototal.text == "R$ 0,00")
        {
            showErrorMessage("Adicione produtos à compra.");
            return false;
        }

        return true;
    }

    [System.Serializable]
    public class Compra
    {
      
        public string NomeProduto;
        public int Quantidade;
        public float Valor;
        

    }

    [System.Serializable]
    public class ComprasList
    {
        public List<Compra> Compras = new List<Compra>();
    }
    [System.Serializable]


    public class Venda
    {
        public string NomeCliente;
        public string Data;
        public float ValorTotal;
        public bool EstaPago;
        public bool PrecisaEntregar;
        public string LocalizaçãoEntrega;
        public List<Compra> Produtos = new List<Compra>();
    }
    [System.Serializable]
    public class ListaVendas
    {
        public List<Venda> Vendas = new List<Venda>();
    }
    private void SaverComprasExiste()
    {
        
        string jjson = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "estoque.json")); //procura o que está salvo no arquivo de estoque
                                                 //para fazer a atualização do mesmo no final do codigo
        esoque = JsonUtility.FromJson<Estoque>(jjson);



        ListaVendas listaVendas = new ListaVendas();

        venda.Produtos.Clear();//Limpeza de dados anteriores

        venda.NomeCliente = NomeCliente.text;
        venda.Data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        venda.ValorTotal = float.Parse(Precototal.text.Replace("R$ ", ""));
        venda.EstaPago = EstaPago;
        venda.PrecisaEntregar = PrecisaEntrgar;
        venda.LocalizaçãoEntrega = LocalizacaoDaEntrega.text;
         
        foreach (Transform produto in PositionSelectProductname.transform)
        {
            venda.Produtos.Add(new Compra
            {
                
                NomeProduto = produto.name,
                Quantidade = int.Parse(
                    produto.GetChild(0).GetComponent<TMPro.TMP_InputField>().text
                ),
                Valor = float.Parse(
                    produto.GetChild(1).GetComponent<TextMeshProUGUI>().text
                )
            });
            
            esoque.materiais.Find(p => p.Nome == produto.name).Quantidade -= int.Parse(
                    produto.GetChild(0).GetComponent<TMPro.TMP_InputField>().text);
            if (esoque.materiais.Find(p => p.Nome == produto.name).Quantidade <= 0)
            {
                showErrorMessage($"O produto {produto.name} está em falta no estoque.");
            }
            TrocaDeDados(esoque);
        }  
        listaVendas.Vendas.Add(venda);
        
        string jsonExistente = File.ReadAllText(caminhosavercompras);
       
        
        print(jsonExistente);
        print(jsonExistente.Length);
        if ( jsonExistente.Length == 0) {
            File.WriteAllText(caminhosavercompras, JsonUtility.ToJson(listaVendas, true));
            LimparCompras();
            return; }//Se estiver Vazio , não precisa adicionar nada, apenas salvar a nova venda
        listaVendas.Vendas.AddRange(JsonUtility.FromJson<ListaVendas>(jsonExistente).Vendas);
        string json = JsonUtility.ToJson(listaVendas, true);
        File.WriteAllText(caminhosavercompras, json);
            LimparCompras();
    
    }

    private void TrocaDeDados(Estoque estoque)
    {
    

        string jsonn = File.ReadAllText(caminho);
        JsonUtility.FromJson<Estoque>(jsonn);

        Estoque estoqueAtualizado = new Estoque();
        estoqueAtualizado.materiais.AddRange(estoque.materiais);
        string json = JsonUtility.ToJson(estoqueAtualizado, true);

        File.WriteAllText(caminho, json);
        ActiveEstoque();
        CarregamendodeProdutos();
        manager.GetComponent<Manager>().Reloadestoque(0);



    }



    public void LimparCompras()
    {
        var produtosmarcados = PositionSelectProductname.gameObject.GetComponentsInChildren<RectTransform>();
        foreach (var produto in produtosmarcados)
        {
            if (produto != PositionSelectProductname.GetComponent<RectTransform>())
            {
                Destroy(produto.gameObject);
            }
        }
        LocalizacaoDaEntrega.text = string.Empty;
        productNames.Clear();
        positionselectproduct = new Vector2(-330, 55);

    }
    public void VerificarNecessidadeDeEntrega() {

        PrecisaEntrgar = !PrecisaEntrgar;
        PrecisarEntregarButton.GetComponentInChildren<TextMeshProUGUI>().text = PrecisaEntrgar ? "Entregar: Sim" : "Entregar: Não";
        PrecisarEntregarButton.GetComponent<Image>().color = PrecisaEntrgar ? Color.green : Color.red;

    }
    public void VerificarProdutoPago()
    {

        EstaPago = !EstaPago;
        PagamentoEfetuado.GetComponentInChildren<TextMeshProUGUI>().text = EstaPago ? "Pago" : "Pendente";
        PagamentoEfetuado.GetComponent<Image>().color = EstaPago ? Color.green : Color.red;

    }



}
