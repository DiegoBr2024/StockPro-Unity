using UnityEngine;
using UnityEngine.UI;
using System.IO;
using static Manager;
using TMPro;
using NUnit.Framework;
using System.Collections.Generic;


public class Search : MonoBehaviour
{
    public Manager manager;
    public Estoque material = new Estoque();

    public TMP_InputField Barra_de_Pesquisa;
    public GameObject canvas;
    public GameObject PositionProdutosdapesquisa;
    public GameObject Possivelprocura ;
    private Vector2 posicao = new Vector2(-300, 47);
    private string caminho;
    private List<GameObject> ItemSugeridosNaPesquisa = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        caminho = Path.Combine(Directory.GetCurrentDirectory(), "estoque.json");
    }

    public void EstoqueLoad()
    {
        material = Carregar();
    }
    private Estoque Carregar()
    {
        if (!File.Exists(caminho))
            return new Estoque();

        string json = File.ReadAllText(caminho);

        return JsonUtility.FromJson<Estoque>(json);
    }

    bool Combina(string nome, string pesquisa)
    {
       
        return nome.ToLower().Contains(pesquisa.ToLower());
    }

    public void Pesquisar()
    {
        posicao = new Vector2(-300, 47);
        var transform = PositionProdutosdapesquisa.GetComponentInChildren<Transform>();
        foreach (GameObject child in ItemSugeridosNaPesquisa)
        {
            
                Destroy(child.gameObject);
            
        }

        

        if (Barra_de_Pesquisa == null || Barra_de_Pesquisa.text == ""|| Barra_de_Pesquisa.text == string.Empty) return;
        foreach (var tester in material.materiais)
        {
            if (Combina(tester.Nome.ToString(), Barra_de_Pesquisa.text))
            {
               
                var instance = Instantiate(Possivelprocura, transform);
                ItemSugeridosNaPesquisa.Add(instance);
                instance.name = tester.Nome;
                var nomeproduct = instance.transform.GetChild(0).gameObject;
                nomeproduct.GetComponent<TextMeshProUGUI>().text = tester.Nome;
                instance.transform.SetParent(canvas.transform, false);
                instance.transform.localPosition = posicao; 
               posicao.y -= 70;
            }
        }    
        
    
    }




}
