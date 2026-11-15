using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ExibirLocalizatio : MonoBehaviour
{
    
    public   GameObject ImageEntrega;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //ImageEntrega = GameObject.Find("ImageEntrega");
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.anyKey.wasPressedThisFrame && ImageEntrega.activeSelf == true)
        {
            ImageEntrega.SetActive(false);

                
            Debug.Log("Alguma tecla foi pressionada!");
        }
    }

    public void VerLocalizacaoDaEntrega()
    {
        ImageEntrega.SetActive(true);
      
       
        
    }


    private IEnumerator Temp()
    {

        yield return new WaitForSeconds(2);
       
    }

}
