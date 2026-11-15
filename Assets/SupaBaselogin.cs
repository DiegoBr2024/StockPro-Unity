
using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

public class SupabaseLogin : MonoBehaviour
{
    [Header("Supabase")]
    public string SupabaseURL = "https://tfhxxwjgpruivvshweeu.supabase.co";
    public string SupabaseKey = "sb_publishable_CbRUrEXZUxrulZPywYhQIg_lKdDWi5K";

    ManagerUI managerUI;

    [Serializable]
    public class ClienteDados
    {
        public string Cliente;
        public bool Permission;
        public int Dias;
    }

    [Serializable]
    public class ListaClientes
    {
        public ClienteDados[] items;
    }

    public bool Existe { get; private set; }
    public bool Permission { get; private set; }
    public int? Dias { get; private set; }

    private void Start()
    {
        managerUI = GameObject.Find("ManagerUI").GetComponent<ManagerUI>();
        BuscarCliente("Bia");
    }

    public void BuscarCliente(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
        {
            Debug.LogWarning("Digite o nome do cliente.");
            return;
        }

        StartCoroutine(ConsultarTodosClientes());
    }

    private IEnumerator ConsultarTodosClientes()
    {
        string url = SupabaseURL.TrimEnd('/')
            + "/rest/v1/Login?select=Cliente,Permission,Dias"
            + "&apikey=" + SupabaseKey; // chave como query string

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            // Header opcional, já que o apikey está na URL
            request.SetRequestHeader("apikey", SupabaseKey);

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                managerUI.showErrorMessage("Erro Supabase: " + request.responseCode
                    + " - " + request.downloadHandler.text);
                yield break;
            }

            string json = request.downloadHandler.text;

            // Envolve o JSON em um objeto com "items"
            string wrapped = "{\"items\":" + json + "}";

            // Desserializa
            ListaClientes lista = JsonUtility.FromJson<ListaClientes>(wrapped);

            if (request.isDone)
            {
                managerUI.showErrorMessage("CONECTADO AO BANCO");
            }

            /*if (lista != null && lista.items.Length > 0)
            {
                foreach (var cliente in lista.items)
                {
                    
                    managerUI.showErrorMessage("Cliente: " + cliente.Cliente +
                              " | Permission: " + cliente.Permission +
                              " | Dias: " + cliente.Dias);
                    yield return new WaitForSeconds(2f);
                }
            }
            else
            {
                managerUI.showErrorMessage("Nenhum cliente encontrado.");
            }*/

        }
    }
}
