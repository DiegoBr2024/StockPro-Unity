using UnityEngine;

public class SelectProduct : MonoBehaviour
{
    public ManagerUI ManagerUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      
    }

   public void Select()
    {
        var name = gameObject.transform.GetChild(0);
        var text = name.GetComponent<TMPro.TMP_Text>().text;
        ManagerUI.SelectProduct(text);
    }


}
