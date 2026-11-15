using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Manager;
using static ManagerUI;
public class ExibirProdutosVendidos : MonoBehaviour
{

    public GameObject BoxDaCompraPessoal;
    public GameObject ComprasClientesObject;
    ManagerUI managerUI;

    [SerializeField]
    private TextMeshProUGUI ContagemDeVendas;
    [SerializeField]
    private TextMeshProUGUI ContagemDeVendasPagas;
    [SerializeField]
    private TextMeshProUGUI Pendente;
    [SerializeField]
    private TextMeshProUGUI Receita;
    Vector2 PosicaoPainelDoComprador = new Vector2(-36, 20);
    Vector2 PosicaoDadosPainelDoComprador = new Vector2(-35, 3);

    public class Venda
    {
        public string NomeCliente;
        public string Data;
        public float ValorTotal;
        public List<Compra> Produtos = new List<Compra>();
    }
    [System.Serializable]
    public class ProdutoVendido
    {
        public List<ManagerUI.Venda> produtos = new List<ManagerUI.Venda>();   
    }
    public GameObject ExibirProdutoVendido;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        
        
    }

    public void ClickVendidos(string QualDadosPriorizar)
    {
        ResetarCompras();
        //caso o usuário queira ver os produtos vendidos, ele vai ler o arquivo de vendas e exibir os produtos vendidos na tela Normalmente,
        //caso o usuário queira ver os pagamentos pendentes, ele vai ler o arquivo de vendas e exibir os produtos vendidos na tela,
        //mas apenas aqueles que não foram pagos ainda.

        

        ProdutoVendido produtoVendido = new ProdutoVendido();
        string caminhosavercompras = Path.Combine(Directory.GetCurrentDirectory(), "Compras.json");
        managerUI = GameObject.FindObjectsByType<ManagerUI>(FindObjectsSortMode.None)[0];
        string jsonn = File.ReadAllText(caminhosavercompras);

        

        ManagerUI.ListaVendas listaVendas =
        JsonUtility.FromJson<ManagerUI.ListaVendas>(jsonn);
        
        ContagemDeVendas.text = listaVendas.Vendas.Count.ToString();
        if (listaVendas == null)
        {
            managerUI.showErrorMessage("Não há vendas registradas.");
            return;
        }
        foreach (var produto in listaVendas.Vendas)
        {
            if (QualDadosPriorizar == "Tudo")
            {
                //Continue A exibição de todos os produtos vendidos
            }
            if (QualDadosPriorizar == "PagamentosPendentes")
            {
                if (produto.EstaPago)
                {
                    continue; // Skip this product if it has been paid
                }
            }

            var BoxDaCompra = Instantiate(BoxDaCompraPessoal);
            BoxDaCompra.transform.SetParent(ComprasClientesObject.transform, false);
            BoxDaCompra.transform.Find("Cliente").GetComponent<TMPro.TextMeshProUGUI>().text = produto.NomeCliente;
            BoxDaCompra.transform.Find("Data").GetComponent<TMPro.TextMeshProUGUI>().text =  produto.Data;
            BoxDaCompra.transform.Find("ValorTotal").GetComponent<TMPro.TextMeshProUGUI>().text =  produto.ValorTotal.ToString("C");
            
            ContagemDeVendasPagas.text = (int.Parse(ContagemDeVendasPagas.text) + ((produto.EstaPago) ? 1 : 0)).ToString();
            if (!produto.EstaPago)
            {
                Pendente.text = (int.Parse(Pendente.text) + 1).ToString();
            }
            if (produto.EstaPago)
            {
                decimal valorTotal = decimal.Parse(produto.ValorTotal.ToString());
                print(valorTotal);
                print(Receita.text);
                decimal receitaAtual = decimal.Parse(Receita.text);

                Receita.text = (receitaAtual + valorTotal).ToString();
            }

            bool PrecisaEntregar = bool.Parse(produto.PrecisaEntregar.ToString());
            string Entregar = PrecisaEntregar ? "Sim" : "Não";
            print("PrecisaEntregar: " + PrecisaEntregar);
            var PrecisaEntregarText = BoxDaCompra.transform.Find("PrecisaEntregar");
            PrecisaEntregarText.GetComponent<TMPro.TextMeshProUGUI>().text = "Precisa Entregar: " + Entregar;
            PrecisaEntregarText.GetComponent<TMPro.TextMeshProUGUI>().faceColor = !PrecisaEntregar ?  new Color32(0, 120, 0, 255)
         : new Color32(139, 0, 0, 255);

            BoxDaCompra.transform.Find("entregarimage").GetComponent<Image>().color = !PrecisaEntregar ? new Color32(157, 255, 157, 255) : new Color32(255, 154, 155, 255);

            bool EstaPago = bool.Parse(produto.EstaPago.ToString().Replace(" ", ""));
            var EstaPagoText = BoxDaCompra.transform.Find("EstaPago");
            print("EstaPago: " + EstaPago);

            

            string pago = EstaPago ? "Sim" : "Não";
            EstaPagoText.GetComponent<TMPro.TextMeshProUGUI>().text = "Está Pago: " + pago;
            EstaPagoText.GetComponent<TMPro.TextMeshProUGUI>().faceColor =
     EstaPago
         ? new Color32(0, 120, 0, 255)
         : new Color32(139, 0, 0, 255);


            var entrega = BoxDaCompra.transform.Find("ImageEntrega");

            if (produto.LocalizaçãoEntrega == string.Empty) { entrega.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Localização não infomada!"; }
            else {entrega.GetChild(0).GetComponent<TextMeshProUGUI>().text = produto.LocalizaçãoEntrega; }
            
            entrega.gameObject.SetActive(false);



            BoxDaCompra.transform.Find("estapagoimage").GetComponent<Image>().color = EstaPago ? new Color32(157, 255, 157, 255) : new Color32(255, 154, 155, 255);

            BoxDaCompra.GetComponent<RectTransform>().localScale = new Vector3(0.33f, 0.33f, 0.33f);
            BoxDaCompra.GetComponent<RectTransform>().anchoredPosition = PosicaoPainelDoComprador;

            foreach (var item in produto.Produtos)
            {
                var ExibirDadosDaCompras = Instantiate(ExibirProdutoVendido);
                ExibirDadosDaCompras.transform.SetParent(BoxDaCompra.transform, false);
                ExibirDadosDaCompras.GetComponent<RectTransform>().localScale = new Vector3(0.14f, 0.14f, 0.14f);
                ExibirDadosDaCompras.GetComponent<RectTransform>().anchoredPosition = PosicaoDadosPainelDoComprador;
                ExibirDadosDaCompras.GetComponent<TMPro.TextMeshProUGUI>().text = item.NomeProduto;
                ExibirDadosDaCompras.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = item.Quantidade.ToString();
                ExibirDadosDaCompras.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = item.Valor.ToString();
                PosicaoDadosPainelDoComprador -= new Vector2(0, 9);
            }
            PosicaoDadosPainelDoComprador = new Vector2(-35, 3);
            PosicaoPainelDoComprador += new Vector2(38, 0);
            if( PosicaoPainelDoComprador.x >= 60)
            {
                PosicaoPainelDoComprador.x = -36;
                PosicaoPainelDoComprador.y -= 34;
            }
        }
    }

    private void ResetarCompras()
    {
        foreach (Transform child in ComprasClientesObject.transform)
        {
            if (child.gameObject != ComprasClientesObject && child.gameObject.name != "Buttons" && child.gameObject.name != "ComprasClientes")
            {
                Destroy(child.gameObject);
            }
            
        }
        PosicaoPainelDoComprador = new Vector2(-36, 20);    
    }
    public void ExibirPagamentosPendente()
    {
        ClickVendidos("PagamentosPendentes");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
