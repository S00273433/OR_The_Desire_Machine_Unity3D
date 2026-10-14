using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;

public class MapAreasUI : MonoBehaviour
{
    [System.Serializable]
    public class MapAresaImage
    {
        public Image image;
        public MapArea mapArea;
    }

    [SerializeField] private List<MapAresaImage> mapAreaImagesList;

    private void Start()
    {
        foreach (MapAresaImage mapAresaImage in mapAreaImagesList)
        {
            mapAresaImage.mapArea.OnCaptured += (object sender, System.EventArgs e) =>
            {
                if (mapAresaImage.mapArea.capState == MapArea.CapState.CapturedbyBlue)
                {
                    mapAresaImage.image.color = Color.blue;
                }
                if (mapAresaImage.mapArea.capState == MapArea.CapState.CapturedbyRed)
                {
                    mapAresaImage.image.color = Color.red;
                }
                if (mapAresaImage.mapArea.capState == MapArea.CapState.BlueCapturing)
                {
                    mapAresaImage.image.color = Color.magenta;
                }
                if (mapAresaImage.mapArea.capState == MapArea.CapState.RedCapturing)
                {
                    mapAresaImage.image.color = Color.magenta;
                }
                if (mapAresaImage.mapArea.capState == MapArea.CapState.Nuetral)
                {
                    mapAresaImage.image.color = Color.white;
                }
            };
        }  
    }

}
