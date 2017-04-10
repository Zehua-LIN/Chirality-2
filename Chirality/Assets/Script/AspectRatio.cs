using UnityEngine.UI;
using UnityEngine;

public class AspectRatio : MonoBehaviour {

	void Start () {
		float standard_width = 1136f;        
        float standard_height = 640f;       
        float device_width = 0f;               
        float device_height = 0f;             
        float adjustor = 0f;       
        
        device_width = Screen.width;  
        device_height = Screen.height;  
        
        float standard_aspect = standard_width / standard_height;  
        float device_aspect = device_width / device_height;  
 
        if (device_aspect < standard_aspect)  
        {  
            adjustor = standard_aspect / device_aspect;  
        }  
  
        CanvasScaler canvasScalerTemp = transform.GetComponent<CanvasScaler>();  
        if (adjustor == 0)  
        {  
            canvasScalerTemp.matchWidthOrHeight = 1;  
        }  
        else  
        {  
            canvasScalerTemp.matchWidthOrHeight = 0;  
        }  
	}
	
}
