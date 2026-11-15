using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Config : MonoBehaviour
{
    [SerializeField]
    Sprite ButtonVendaSemEstoquePermissionTRUE;
    [SerializeField]
    Sprite ButtonVendaSemEstoquePermissionFalse;
    [SerializeField]
    Button VendasSemEstoqueButton;

    public bool VendaSemEstoquePermission;
    // Start is called once before the first execution of Update after the MonoBehaviour is created



    string Caminho;
    public class Configuracoes
    {
       public string NomeDoUsuario;
       public bool PermitirVendaSemEstoque;


    }

    [SerializeField]
     Configuracoes configuracoes = new Configuracoes();

    void Start()
    {
        
        Caminho = Path.Combine(Application.dataPath + "Config.json");
        configuracoes = ReadConfig() ?? new Configuracoes();
        ChargeConfig();
        
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickButtonVendasSemEstoque()
    {
        VendaSemEstoquePermission = !VendaSemEstoquePermission;
        VendasSemEstoqueButton.image.sprite = VendaSemEstoquePermission ? ButtonVendaSemEstoquePermissionTRUE : ButtonVendaSemEstoquePermissionFalse;
        configuracoes.PermitirVendaSemEstoque = VendaSemEstoquePermission;
        SavercCofig();
    }

    private void SavercCofig()
    {
        string json = JsonUtility.ToJson(configuracoes,true);
        File.WriteAllText(Caminho,json);

    }

    private void GetDadosConfig()
    {
        configuracoes.NomeDoUsuario = string.Empty;
        configuracoes.PermitirVendaSemEstoque = VendaSemEstoquePermission;
    }

    private Configuracoes ReadConfig()
    {
        if (!File.Exists(Caminho)) { return null; }
         string json = File.ReadAllText(Caminho);
        return JsonUtility.FromJson<Configuracoes>(json);

        
    }
    private void Awake()
    {
        
    }
    private void ChargeConfig()
    {
        VendasSemEstoqueButton.image.sprite = configuracoes.PermitirVendaSemEstoque ? ButtonVendaSemEstoquePermissionTRUE : ButtonVendaSemEstoquePermissionFalse;
    }

}
