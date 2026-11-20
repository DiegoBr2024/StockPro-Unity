using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class Manager : MonoBehaviour
{

    
    public GameObject positionestoque;
    private bool colorselect = false;
    [SerializeField]
     GameObject seletorpage;
    [SerializeField]
     GameObject page;
    [System.Serializable]
    
    public class Materialestoque
    {
        public string Nome;
        public int Quantidade;
        public float PrecoDeCompra;
        public float PrecoDeVenda;
    }

    Materialestoque materialestoqueeditavel;
    Materialestoque materialestoqueatual = new Materialestoque();


    

    [System.Serializable]
    

    public class Estoque
    {
        public List<Materialestoque> materiais = new List<Materialestoque>();
    }

    public ManagerUI ManagerUI;
    public GameObject isa;
    Estoque estoque = new Estoque();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       /* Estoque estoque = new Estoque();
        estoque.materiais.Add(new Materialestoque { Nome = "Cimento CP II 50kg", Quantidade = 120, PrecoDeCompra = 39.90f });
        estoque.materiais.Add(new Materialestoque { Nome = "Areia Média (m³)", Quantidade = 35, PrecoDeCompra = 145.00f });
        estoque.materiais.Add(new Materialestoque { Nome = "Brita Nº 1 (m³)", Quantidade = 28, PrecoDeCompra = 169.90f });
        estoque.materiais.Add(new Materialestoque { Nome = "Tijolo Cerâmico", Quantidade = 3500, PrecoDeCompra = 1.20f });
        estoque.materiais.Add(new Materialestoque { Nome = "Bloco de Concreto 14x19x39", Quantidade = 850, PrecoDeCompra = 6.90f });
        estoque.materiais.Add(new Materialestoque    { Nome = "Vergalhão CA-50 10mm", Quantidade = 140, PrecoDeCompra = 58.90f });
        estoque.materiais.Add(new Materialestoque { Nome = "Tubo PVC 100mm 6m", Quantidade = 42, PrecoDeCompra = 119.90f });
        estoque.materiais.Add(new Materialestoque { Nome = "Joelho PVC 90° 100mm", Quantidade = 95, PrecoDeCompra = 14.90f });
        estoque.materiais.Add(new Materialestoque { Nome = "Tinta Acrílica Branca 18L", Quantidade = 18, PrecoDeCompra = 289.90f });
        estoque.materiais.Add(new Materialestoque { Nome = "Argamassa AC-II 20kg", Quantidade = 74, PrecoDeCompra = 34.90f });
        Salvar(estoque); */
       Showitem(1);
       


    }
    [SerializeField] GameObject itemPrefab;
    [SerializeField] Canvas canva;
    private Vector2 posicao = new Vector2(0, 132);
    public TMP_InputField[] itens;

    [SerializeField]
    private int DistanceItemObject;

    [SerializeField]
    TextMeshProUGUI QuantidadeDeProdutos;
    [SerializeField]
    TextMeshProUGUI QuantidadeDetotalProdutos ;
    [SerializeField]
    TextMeshProUGUI ValorEmEstoque  ;

    [SerializeField]
    GameObject EditorScript;

    public void Showitem(int ValorDaPaginaDeExibicao)
    {
        int QuantidadeDetotalProdutos = 0;
        float ValorEmEstoque = 0;


        posicao = new Vector2(0, 132);
        estoque = Carregar();
        this.QuantidadeDeProdutos.text = estoque.materiais.Count.ToString();
        foreach (var produto in estoque.materiais)
        {
            QuantidadeDetotalProdutos += produto.Quantidade;
            ValorEmEstoque += produto.PrecoDeVenda * produto.Quantidade;
            print($"Produto: {produto.Nome}, Quantidade: {produto.Quantidade}, Preço de Compra: {produto.PrecoDeCompra}, Preço de Venda: {produto.PrecoDeVenda}");
        }
        this.QuantidadeDetotalProdutos.text = QuantidadeDetotalProdutos.ToString();
        this.ValorEmEstoque.text = ValorEmEstoque.ToString("F2");
        

        PageEstoque(estoque.materiais.Count);
        int calculo = (ValorDaPaginaDeExibicao - 1) * 5;
        int numerosDeexibicao = 0;
        if (ValorDaPaginaDeExibicao < 1)
            ValorDaPaginaDeExibicao = 1;
        print(calculo);
        for (int i = calculo; i < estoque.materiais.Count && numerosDeexibicao < 5; i++)
        {
            numerosDeexibicao++;
            GameObject intantiate = Instantiate(itemPrefab);
            intantiate.GetComponent<RectTransform>().anchoredPosition = posicao;
            intantiate.transform.SetParent( positionestoque.transform, false);
            intantiate.name = estoque.materiais[i].Nome;
            //intantiate.GetComponent<editor>().editar(estoque.materiais.Find(M => M.Nome == estoque.materiais[i].Nome));
            intantiate.transform.Find("EditorScript"  ).gameObject.GetComponent<editor>().editar(estoque.materiais.Find(M => M.Nome == estoque.materiais[i].Nome));
            intantiate.transform.SetSiblingIndex(0);
            GameObject nome = intantiate.transform.Find("name").gameObject;
           
            nome.GetComponent<TextMeshProUGUI>().text = estoque.materiais[i].Nome;
            GameObject quantidade = intantiate.transform.Find("quantidade").gameObject;
            
            quantidade.GetComponent<TextMeshProUGUI>().text = estoque.materiais[i].Quantidade.ToString();
            quantidade.GetComponent<TextMeshProUGUI>().color = GetColorForIndex(int.Parse(quantidade.GetComponent<TextMeshProUGUI>().text));
            GameObject preco = intantiate.transform.Find("preco").gameObject;
            preco.GetComponent<TextMeshProUGUI>().text = estoque.materiais[i].PrecoDeVenda.ToString("F2");
            //color(intantiate.transform.Find("image").gameObject);
            posicao.y -= DistanceItemObject;
        }
    }
    private Vector3 pagePositioselector = new Vector3(-660,-210, 0);
    public void PageEstoque(int numeropages)
    {
        int totalPages = Mathf.CeilToInt(numeropages / 5f);
        pagePositioselector = new Vector3(-660, -210, 0);
        var transform = seletorpage.GetComponentInChildren<RectTransform>();
        foreach (Transform child in transform)
        {
            if(seletorpage.gameObject != child.gameObject)
                Destroy(child.gameObject);
           
        }
        for (int i = 0; i < totalPages; i++)
        {
            int pagina = i + 1;

            GameObject intantiate = Instantiate(page);
            intantiate.transform.SetParent(seletorpage.transform, false);
            intantiate.GetComponent<RectTransform>().anchoredPosition = pagePositioselector;
            intantiate.GetComponentInChildren<TextMeshProUGUI>().text =
                pagina.ToString();

            intantiate.GetComponent<Button>()
                .onClick.AddListener(() => Reloadestoque(pagina));
            pagePositioselector.x += 80;
        }



    }

    public void DeleteItem(Materialestoque material)
    {
        Materialestoque item = estoque.materiais
         .Find(x => x.Nome == material.Nome);

        if (item != null)
        {
            estoque.materiais.Remove(item);

            DeletaAll();
            Reloadestoque(1);
        }


    }

    private Color GetColorForIndex(int index)
    {
        if (index <= 0) return Color.red;
        if (index >= 0) return Color.green;

        return index % 2 == 0 ? Color.grey : Color.black;
    }

    private void color(GameObject obj)
    {
        Color corSelecionado;
        ColorUtility.TryParseHtmlString("#252E3E", out corSelecionado);

        Color corNormal;
        ColorUtility.TryParseHtmlString("#1B1F26", out corNormal);
        colorselect = !colorselect;
        obj.GetComponent<Image>().color = colorselect ? corNormal : corSelecionado;
        obj.GetComponent<Image>().color = colorselect ? corNormal : corSelecionado;
    }
   /* public void showaddnow(Materialestoque materialestoque)
    {
        if (itemPrefab == null){ Debug.Log("Prefab não encontrado."); return; }
        GameObject intantiate = Instantiate(itemPrefab, posicao, Quaternion.identity);
        intantiate.transform.SetParent(positionestoque.transform, false);
        intantiate.transform.GetChild(4).gameObject.GetComponent<editor>().editar(materialestoque);
        intantiate.transform.SetSiblingIndex(0);
        GameObject nome = intantiate.transform.GetChild(1).gameObject;
        nome.GetComponent<TextMeshProUGUI>().text = materialestoque.Nome;
        GameObject quantidade = intantiate.transform.GetChild(2).gameObject;
        quantidade.GetComponent<TextMeshProUGUI>().text = materialestoque.Quantidade.ToString();
        GameObject preco = intantiate.transform.GetChild(3).gameObject;
        preco.GetComponent<TextMeshProUGUI>().text = materialestoque.PrecoDeCompra.ToString("F2");
        color(intantiate.transform.GetChild(0).gameObject);
        posicao.y -= DistanceItemObject;
    }*/

  

    // Update is called once per frame
    void Update()
    {
     
    }


    private void Awake()
    {
        caminho = Path.Combine(Directory.GetCurrentDirectory(), "estoque.json");

    }
    static string caminho;

    public static void Salvar(Estoque estoque)
    {
        string json = JsonUtility.ToJson(estoque, true);

        File.WriteAllText(caminho, json);

        Debug.Log("Salvo em: " + caminho);
    }

    /*public static void Salvar(Estoque estoque)
    {
        
            string jsonn = File.ReadAllText(caminho);
            JsonUtility.FromJson<Estoque>(jsonn);

            Estoque estoqueAtualizado = new Estoque();
            estoqueAtualizado.materiais.AddRange(estoque.materiais);
            string jsonm = File.ReadAllText(caminho);
        if (jsonm != null && jsonm != "")
        { Estoque jsonnn = JsonUtility.FromJson<Estoque>(jsonm);
            estoqueAtualizado.materiais.AddRange(jsonnn.materiais);
        }
            string json = JsonUtility.ToJson(estoqueAtualizado, true);

            File.WriteAllText(caminho, json);

        estoque = Carregar();
        Debug.Log("Salvo em: " + caminho);
        
        
    }*/

    public static Estoque Carregar()
    {
        if (!File.Exists(caminho))
            return new Estoque();

        string json = File.ReadAllText(caminho);

        return JsonUtility.FromJson<Estoque>(json);
    }

    public void ArmazenarDados(Materialestoque materialestoqueeditavel )
    {
        this.materialestoqueeditavel = materialestoqueeditavel;
      
    }

    public void ObterDadosAtual()
    {
        try
        {
            materialestoqueatual.Nome = itens[0].text.ToString();
            materialestoqueatual.Quantidade = int.Parse(itens[1].text);
            materialestoqueatual.PrecoDeCompra = float.Parse(itens[2].text);
            EditItem();
        }
        catch
        {
            ManagerUI.showErrorMessage("Campo com valores inválidos.");
            return;
        }
    }

    public void EditItem()
    {
        estoque = Carregar();
        print("materialeditavel");
       for (int i = 0;i <= estoque.materiais.Count - 1; i++)
        {
            if (estoque.materiais[i].Nome == materialestoqueeditavel.Nome)
            {
                estoque.materiais[i] = materialestoqueatual;
                DeletaAll();
                Reloadestoque(1);
               ManagerUI.showeditor();
                break;
            }
        }

    }

    public void EditItemQuantidade()
    {
        estoque = Carregar();
        print("troca quantidade");
        for (int i = 0; i <= estoque.materiais.Count - 1; i++)
        {
            if (estoque.materiais[i].Nome == materialestoqueeditavel.Nome)
            {
                estoque.materiais[i] = materialestoqueatual;
                DeletaAll();
                Reloadestoque(1);
                ManagerUI.showeditor();
                break;
            }
        }

    }
    public void DeletaAll()
    {
        string jsonsaver = JsonUtility.ToJson(estoque, true);

        // Apaga o conteúdo anterior e salva o novo
        File.WriteAllText(caminho, jsonsaver);

    }
    public void AtualizarMaterial(Materialestoque antigo, Materialestoque novo)
    {
        estoque = Carregar();

        for (int i = 0; i < estoque.materiais.Count; i++)
        {
            if (estoque.materiais[i].Nome == antigo.Nome)
            {
                estoque.materiais[i] = novo;
                break;
            }
        }

        string json = JsonUtility.ToJson(estoque, true);
        File.WriteAllText(caminho, json);

        Reloadestoque(1);
    }
    public void Reloadestoque(int seletor)
    {
        print("Reloading estoque...");
        Transform[] itens = positionestoque.GetComponentsInChildren<Transform>();
        foreach (Transform item in itens)
        {
            if (positionestoque.gameObject != item.gameObject) {
                Destroy(item.gameObject);
            }
        }
        posicao = new Vector2(0, 0);
        Showitem(seletor);
    }
}


