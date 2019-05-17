using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputPanel : MonoBehaviour ,IPointerDownHandler//,IPointerEnterHandler
{

    public GameObject panel;
    public InputField inputField;

    private void Start()
    {
       inputField= this.transform.GetComponent<InputField>();

        inputField.onEndEdit.AddListener(delegate 
        {
            Debug.Log("input finish");
            panel.SetActive(false);
        });
    }

    public  void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("start input");
        panel.SetActive(true);
    }
    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    panel.SetActive(true);
    //}
    private void Update()
    {
        inputField.onEndEdit.AddListener(delegate
        {
            Debug.Log("input finish");
            panel.SetActive(false);
        });
    }


}
