using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CarManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject carPrefab;
    public List<GameObject> cars = new List<GameObject>();
    Vector3[] iniPosition = {new Vector3(0.5f, -8.5f, 0), new Vector3(15.5f, 7.5f, 0), new Vector3(-0.5f, 22.5f, 0), new Vector3(-15.5f, 6.5f, 0)};
    Quaternion[] iniRotation = {Quaternion.Euler(0, 0, 270), Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 0, 90), Quaternion.Euler(0, 0, 180)};
    
    public float speed = 1;
    public float cur_time = 0;
    float wall_time = 0;
    public float delta;
    public GameObject traffic_light_manager;
    public string file_path;
    bool isStart = false;
    public void start_simulate(){
        isStart = false;
        reset();
        traffic_light_manager.GetComponent<TrafficLightManager>().reset();
        if (File.Exists(file_path))
        {
            string fileContent = File.ReadAllText(file_path);
            string[] fileLines = fileContent.Split('\n');

            for(int i = 0; i < fileLines.Length-5; i += 2){
                string[] carData = fileLines[i].Split(' ');
                int id = int.Parse(carData[0]);
                int p = int.Parse(carData[1]);
                int l = int.Parse(carData[2]);
                int isHV = int.Parse(carData[3]);
                cars.Add(Instantiate(carPrefab, new Vector3(0, 0, 0), Quaternion.identity));
                GameObject sprite = cars[cars.Count-1].transform.GetChild(0).gameObject;
                sprite.transform.position = iniPosition[l];
                sprite.transform.rotation = iniRotation[l];
                sprite.GetComponent<controller>().id = id;
                sprite.GetComponent<controller>().p = p;
                if(isHV == 1){
                    sprite.GetComponent<SpriteRenderer>().color = Color.red;
                }
                else{
                    sprite.GetComponent<SpriteRenderer>().color = Color.blue;
                }
                sprite.GetComponent<controller>().forward_directions = fileLines[i+1].Split(' ');
                // cars[cars.Count-1].SetActive(false);
            }
            for(int i = 0; i < 4; ++i){
                string[] data = fileLines[i+fileLines.Length-5].Split(' ');
                traffic_light_manager.GetComponent<TrafficLightManager>().states.Add(data);
            }

        }
        else
        {
            Debug.Log("File does not exist.");
        }
        isStart = true;
    }
    // Update is called once per frame
    void Update()
    {
        if(!isStart) return;
        delta = Time.deltaTime; 
        wall_time += delta;
        cur_time += delta*speed;
        foreach(GameObject car in cars){
            if(car.transform.GetChild(0).gameObject.GetComponent<controller>().p <= cur_time && !car.transform.GetChild(0).gameObject.GetComponent<controller>().isAppear){
                car.transform.GetChild(0).gameObject.GetComponent<controller>().isAppear = true;
                car.SetActive(true);
                car.transform.GetChild(0).gameObject.GetComponent<controller>().pre_time = car.transform.GetChild(0).gameObject.GetComponent<controller>().p;
            }
            if(car.transform.GetChild(0).gameObject.transform.position.x > 15.5f || car.transform.GetChild(0).gameObject.transform.position.x < -15.5f || car.transform.GetChild(0).gameObject.transform.position.y > 22.5f || car.transform.GetChild(0).gameObject.transform.position.y < -8.5f){
                car.SetActive(false);
            }
            if(car.activeSelf){
                car.transform.GetChild(0).gameObject.GetComponent<controller>().move(cur_time);
            }
        }
        traffic_light_manager.GetComponent<TrafficLightManager>().step(cur_time);

    }

    void reset(){
        foreach(GameObject car in cars){
            Destroy(car);
        }
        cars.Clear();
        cur_time = 0;
    }
}
