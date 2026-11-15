using System.IO;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class ExibirClientes : MonoBehaviour
{
    public GameObject ColunasClientes;
    Vector2 PosicaoColunaClientes = new Vector2(-60, 55);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public ManagerUI.ListaVendas vendas = new ManagerUI.ListaVendas();
    public void ExibirClientesButton()
    {
        ResetListCliente();
        print(1);
        PosicaoColunaClientes = new Vector2(-60, 55);
        string json = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "Compras.json"));
        vendas = JsonUtility.FromJson<ManagerUI.ListaVendas>(json);
        foreach (var item in vendas.Vendas)
        {
            
            if (transform.Find(item.NomeCliente.ToLower()))
            {print(item.NomeCliente);
                if (item.EstaPago) { continue; }
                var divida = transform.Find(item.NomeCliente.ToLower()).Find("Divida");
                var textoDivida = divida.GetComponent<TextMeshProUGUI>();

                float valorAtual = 0f;

                string texto = textoDivida.text.Replace("Divida R$ ", "").Trim();

                if (!float.TryParse(texto, out valorAtual))
                {
                    valorAtual = 0f;
                }

                float valorDivida = valorAtual + item.ValorTotal;

                textoDivida.text = "Divida R$ " + valorDivida.ToString("F2");

                continue;
            }

            

            ColunasClientes.name = item.NomeCliente;
            var coluna = Instantiate(ColunasClientes, transform); 
           coluna.transform.SetParent(transform, false);
            coluna.name = item.NomeCliente.ToLower();
            coluna.GetComponent<RectTransform>().anchoredPosition = PosicaoColunaClientes;
            coluna.GetComponent<RectTransform>().localScale = new Vector3(0.13f, 0.13f, 0.13f);
            var nomeCliente = coluna.transform.Find("Nome");
            nomeCliente.GetComponent<TextMeshProUGUI>().text = "Cliente: " + item.NomeCliente;

            var Morar= coluna.transform.Find("Morar");
            var Divida= coluna.transform.Find("Divida");
            
            Morar.GetComponent<TextMeshProUGUI>().text = item.LocalizaçãoEntrega;
            if(item.LocalizaçãoEntrega == null || item.LocalizaçãoEntrega == string.Empty)
            {
                Morar.GetComponent<TextMeshProUGUI>().text = "Localização não informada";
            }
            if(!item.EstaPago)
            {
                Divida.GetComponent<TextMeshProUGUI>().text = "Divida R$ " + item.ValorTotal.ToString("F2");
            }
            PosicaoColunaClientes.x += 55;
            if (PosicaoColunaClientes.x > 80)
            {
                PosicaoColunaClientes.x = -60;
                PosicaoColunaClientes.y -= 25;
            }
        }
        print(2);

    }
    private void ResetListCliente()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
}
