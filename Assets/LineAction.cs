using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Diagnostics;
using LitJson;
using UnityEngine.UI;
using System.Threading;
using TMPro;

public class LineAction : MonoBehaviour
{
    
    public Sprite button_blue;
    private Queue<IEnumerator> coroutineQueue = new Queue<IEnumerator>(); //Korutinu eile

    private bool pressed = false;
    public GameObject Line;
    private LineRenderer lineRenderer;
    private int buttonCount = 0;// Skaiciuoja kiek tasko objektu isaugota
    private GameObject firsDot; // Pirmo tasko objiektui saugot, kad zinot, kuriuos juntg
    private bool firsDotPressed = true;// Tikrina ar pimas taskas
    private Canvas renderCanvas;
    private float lineSpeed = 2f;
    public List<GameObject> buttonsForDelete;
    public List<GameObject> linesForDelete; 
    private GameObject backButton;

    
    public void StartLineAction()
    {
        StartCoroutine(CoroutineCoordinator()); //Inicijuojama korutinu eile
        buttonsForDelete = new List<GameObject>();
        linesForDelete = new List<GameObject>();
        backButton = GameObject.FindGameObjectWithTag("MyGameBackButton");
        backButton.SetActive(false);        
        GameObject canvas = GameObject.FindGameObjectWithTag("MyCanvas");
        renderCanvas = canvas.GetComponent<Canvas>();
    }

    public void MethodThatButton1ShouldDo(bool press, GameObject buttonObject, int levelDotNumber)
    {
        this.pressed = press;

        if (pressed)
        {

            //Pirma kart paspaudus taska
            if (int.Parse("" + buttonObject.name) == 1 && firsDotPressed)
            {
                firsDotPressed = false;
                firsDot = buttonObject;                
                firsDot.GetComponent<Button>().GetComponent<Image>().sprite = button_blue;
                buttonsForDelete.Add(buttonObject);
                buttonCount++;
            }
            else if ((int.Parse("" + firsDot.name) + 1 == int.Parse("" + buttonObject.name) && !firsDotPressed) || (buttonCount == levelDotNumber / 2 && int.Parse("" + buttonObject.name) == 1))  //Visiem kitiem po pirmo karto
            {
                
                Vector3 s = new Vector3(firsDot.transform.position.x, firsDot.transform.position.y, 90); //Nustatomos linijos pradžios koordinatės
                Vector3 e = new Vector3(buttonObject.transform.position.x, buttonObject.transform.position.y, 90); //Nustatomos linijos pabaigos koordinatės

                if ((buttonCount == levelDotNumber / 2 && int.Parse("" + buttonObject.name) == 1))
                {
                    coroutineQueue.Enqueue(Move(s, e,true)); //Pridedama į eilę, true, jei paskitine linija
                }
                else
                {
                    coroutineQueue.Enqueue(Move(s, e, false));
                }
                buttonObject.GetComponent<Button>().GetComponent<Image>().sprite = button_blue;

                buttonsForDelete.Add(buttonObject);
                buttonCount++;

                firsDot = buttonObject;               
            }          
        }
    }
    /// <summary>
    /// Korutinu koordinatorius
    /// </summary>
    /// <returns></returns>
    IEnumerator CoroutineCoordinator()
    {
        while (true)
        {
            while (coroutineQueue.Count > 0)
                yield return StartCoroutine(coroutineQueue.Dequeue());
            yield return null;
        }
    }
    /// <summary>
    /// Linijos piesimo metodas
    /// </summary>
    /// <param name="start">Linijos pradžios kordinatės</param>
    /// <param name="end">Linijos pabaigos kordinatės</param>
    /// <returns></returns>
    private IEnumerator Move(Vector3 start, Vector3 end, bool gameEnd)
    {    
        GameObject newLine = Instantiate(Line, new Vector3(start.x, start.y, 90), Quaternion.identity) as GameObject;
        newLine.transform.SetParent(renderCanvas.transform, true);
        linesForDelete.Add(newLine);

        Vector3 step = (end - start) / (Vector3.Distance(start, end) / lineSpeed); //Apsakaiciuoja kokiu zingsniu zengti 
        Vector3 buff = start; //buferis 
        lineRenderer = newLine.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;

        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, start);

        while (Vector3.Distance(lineRenderer.GetPosition(0), lineRenderer.GetPosition(1)) < Vector3.Distance(start, end)) //Tikrina ar turimas vektorius yra trumpesnis nei norimas ilgis
        {
            yield return new WaitForSeconds(0.01f); //Laukia 0.01s
            buff += step; //Prie buferio pridedamas zingsnis
            lineRenderer.SetPosition(1, buff); //Linijos galas nustatomas i buferio vieta            
        }
        lineRenderer.SetPosition(1, end); //Isitikina, jog vektorius nera ilgesnis nei turi buti

        //Pabaigos veiksmai
        if (gameEnd)
        {
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < buttonsForDelete.Count; i++)
            {
                Destroy(buttonsForDelete[i]);
               
            }
            for (int i = 0; i < linesForDelete.Count; i++)
            {
                Destroy(linesForDelete[i]);
               
            }
           
            backButton.SetActive(true);          
            buttonCount = 0;
        }
       
    }   
}