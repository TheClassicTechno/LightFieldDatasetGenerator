using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RandomObjectSpawner : MonoBehaviour
{

    public GameObject[] myObjects;

    public List<GameObject> enemyList = new List<GameObject>();

    int maxSpawnAmount = 40;
    int datasetSize = 2;
    int numImages = 0;
    int numCameras_x = 3;
    int numCameras_y = 3;
    float camera_spacing = 0.5f;

    int resWidth = 1920; 
    int resHeight = 1080;

    void DestroyEnemy() {
        while (enemyList.Count > 0) {
            var firstElement = enemyList.First();
            enemyList.Remove(firstElement);
            Destroy(firstElement);
        }
    }

     public static string ScreenShotName(int imageNumber, int cameraIdx_x, int cameraIdx_y) {
         return string.Format("{0}/screenshots/image{1}_x{2}_y{3}.png", 
                              Application.dataPath, 
                              imageNumber, cameraIdx_x, cameraIdx_y);
     }

    void Update()
    {

        //StartCoroutine(Spawn());

        //IEnumerator void Spawn() {

        //yield return new WaitForSeconds(1); //waits 1 seconds 
        if (numImages < datasetSize)
        {

            if(enemyList.Count < maxSpawnAmount){
                transform.position= new Vector3(0.0f, 10.0f, 0.0f);

                int randomIndex = Random.Range(0, myObjects.Length); // x               // y                  // z      
                Vector3 randomSpawnPosition = new Vector3(Random.Range(-8, 8), Random.Range(5, 15), Random.Range(-10, -5));

                enemyList.Add(Instantiate(myObjects[randomIndex], randomSpawnPosition, Quaternion.identity));

            } else {
                for (int c_x = 0; c_x < numCameras_x; c_x ++) {
                    for (int c_y = 0; c_y < numCameras_y; c_y ++) {
                        // Take Image from Camera 0
                        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
                        transform.position= new Vector3(((float)c_x) * camera_spacing, 10.0f+((float)c_y) * camera_spacing, 0.0f);
                        GetComponent<Camera>().targetTexture = rt;
                        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
                        GetComponent<Camera>().Render();
                        RenderTexture.active = rt;
                        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
                        GetComponent<Camera>().targetTexture = null;
                        RenderTexture.active = null; // JC: added to avoid errors
                        Destroy(rt);
                        byte[] bytes = screenShot.EncodeToPNG();
                        string filename = ScreenShotName(numImages, c_x, c_y);
                        System.IO.File.WriteAllBytes(filename, bytes);
                        Debug.Log(string.Format("Took screenshot to: {0}", filename));
                    }
                }

                // Reset Scene
                DestroyEnemy();
                numImages = numImages + 1;
            }   
        }
                
    }

 
    //  public void TakeHiResShot() {
    //      takeHiResShot = true;
    //  }
 
    //  void LateUpdate() {
    //            // takeHiResShot |= Input.GetKeyDown("k");
    //             if (takeHiResShot) {
    //                 RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
    //                 GetComponent<Camera>().targetTexture = rt;
    //                 Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
    //                 GetComponent<Camera>().Render();
    //                 RenderTexture.active = rt;
    //                 screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
    //                 GetComponent<Camera>().targetTexture = null;
    //                 RenderTexture.active = null; // JC: added to avoid errors
    //                 Destroy(rt);
    //                 byte[] bytes = screenShot.EncodeToPNG();
    //                 string filename = ScreenShotName(resWidth, resHeight);
    //                 System.IO.File.WriteAllBytes(filename, bytes);
    //                 Debug.Log(string.Format("Took screenshot to: {0}", filename));
    //                 takeHiResShot = false;
    //             }
    //         }

}