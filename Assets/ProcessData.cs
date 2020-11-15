using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Diagnostics;
using LitJson;
using UnityEngine.UI;
using TMPro;

public class ProcessData : MonoBehaviour
{
    private string jsonString;
    private JsonData itemData; //Json objektui laikyti
    public GameObject button_magenta;
    private Canvas renderCanvas;
    private int dotCount = 0;
    private LineAction lineAction;
           

    public void StartGame(int level)
    {
        lineAction = GameObject.FindGameObjectWithTag("MyManager").GetComponent<LineAction>();


        //Susirandu canvas pagal taga
        GameObject canvas = GameObject.FindGameObjectWithTag("MyCanvas");
        renderCanvas = canvas.GetComponent<Canvas>();

        //Nuskaito duomenis is failo i string
        jsonString = File.ReadAllText(Application.dataPath + "/level_data.json");
       
        //Prideda eilute i zodyna
        itemData = JsonMapper.ToObject(jsonString);

        Vector3 p = Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelHeight, Camera.main.nearClipPlane)); //Randa kairio virsutinio kamers kampo koordinates. GERAS

        Vector3 camBottomRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, 0, Camera.main.nearClipPlane)); //Matuoju atstuma width desinys krastas
        Vector3 camTopLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelHeight, Camera.main.nearClipPlane));
        Vector3 camCenter = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2, Camera.main.nearClipPlane));

        float coordheightRange = System.Math.Abs(camBottomRight.y) + System.Math.Abs(camTopLeft.y);
        float coordWidthRange = System.Math.Abs(camBottomRight.x) + System.Math.Abs(camTopLeft.x);

        float heigthDivider = coordheightRange / 1000; // Kad taskai butu ekrano aukscio skalej, 1000 nes tasku koordinates nuo [0;1000]
        float widthDivider = coordWidthRange / 1000; //Kad taskai butu ekrano plocio skalej


        for (int i = 0; i < itemData["levels"][level]["level_data"].Count; i += 2)
        {
            //Nusiskaitau koordinates is failo
            float xGiven = int.Parse(itemData["levels"][level]["level_data"][i].ToString());
            float yGiven = int.Parse(itemData["levels"][level]["level_data"][i + 1].ToString());

            
            //Pritaikau koordinates kamerai
            float x = p.x + xGiven * widthDivider;
            float y = p.y - yGiven * heigthDivider;           

          

                GameObject a = Instantiate(button_magenta, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                a.transform.SetParent(renderCanvas.transform, true);


                dotCount = (i / 2) + 1;
                a.name = "" + dotCount;

                a.transform.Find("Text").transform.GetComponent<Text>().text = "" + dotCount;
                int levelDotNumber = itemData["levels"][level]["level_data"].Count;

                a.GetComponent<Button>().onClick.AddListener(() => { lineAction.MethodThatButton1ShouldDo(true, a, levelDotNumber); });

        }

        dotCount = 0;//?
    }
}
