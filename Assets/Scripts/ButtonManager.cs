using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;
public class ButtonManager : MonoBehaviour
{
    // Start is called before the first frame update
    
    public GameObject manager;
    private string[] data = {"","","","","",""};
    public TextAsset[] default_files = new TextAsset[2];
    public int current_id;
    public Scrollbar scrollBar;
    public void btn_CAV(){
        manager.GetComponent<CarManager>().file_path = "/Users/chuchulab/Desktop/Master_2022_Fall/Research/Code/IntersectionManagement/FourWay-SingleLane/output/result-CAV";
        manager.GetComponent<CarManager>().start_simulate();
    }
    public void btn_HV(){
        manager.GetComponent<CarManager>().file_path = "/Users/chuchulab/Desktop/Master_2022_Fall/Research/Code/IntersectionManagement/FourWay-SingleLane/output/result-HV";
        manager.GetComponent<CarManager>().start_simulate();
    }
    public void btn_Hybrid(){
        manager.GetComponent<CarManager>().file_path = "/Users/chuchulab/Desktop/Master_2022_Fall/Research/Code/IntersectionManagement/FourWay-SingleLane/output/result-Hybrid";
        manager.GetComponent<CarManager>().start_simulate();
    }
    public void btn_ThreeLane_start(){
        manager.GetComponent<ThreeLaneManager>().fileLines.Clear();
        int id = 0;
        if(data[id] == "") manager.GetComponent<ThreeLaneManager>().fileLines.AddRange(default_files[id].text.Split('\n'));
        else manager.GetComponent<ThreeLaneManager>().fileLines.AddRange(data[id].Split('\n'));
        manager.GetComponent<ThreeLaneManager>().start_simulate();
    }
    public void btn_ThreeLane_H_AIM_start(){
        manager.GetComponent<ThreeLaneManager>().fileLines.Clear();
        int id = 1;
        if(data[id] == "") manager.GetComponent<ThreeLaneManager>().fileLines.AddRange(default_files[id].text.Split('\n'));
        else manager.GetComponent<ThreeLaneManager>().fileLines.AddRange(data[id].Split('\n'));
        manager.GetComponent<ThreeLaneManager>().start_simulate();
    }
    public void UpdateData()
    {
    		// Requesting a file from the user
        FileUploaderHelper.RequestFile((path) => 
        {
        		// If the path is empty, ignore it.
            if (string.IsNullOrWhiteSpace(path))
                return;
						
            StartCoroutine(UploadData(path));
        }, ".txt");
    }
    // Coroutine for image upload
    IEnumerator UploadData(string path)
    {
        string text;
        // using to automatically call Dispose, create a request along the path to the file
        using (UnityWebRequest www = new UnityWebRequest(path, UnityWebRequest.kHttpVerbGET))
        {
			// We create a "downloader" for textures and pass it to the request
            www.downloadHandler = new DownloadHandlerBuffer();
						
            // We send a request, execution will continue after the entire file have been downloaded
            yield return www.SendWebRequest();
						
            // Getting the texture from the "downloader"
            text = www.downloadHandler.text;
        }
		data[current_id] = text;
        
    }
    public void btn_our_upload_file(){
        current_id = 0;
        UpdateData();
    }
    public void btn_H_AIM_upload_file(){
        current_id = 1;
        UpdateData();
    }
    public void scrollbar_on_change(){
        manager.GetComponent<ThreeLaneManager>().speed = scrollBar.value*12;
    }
}
