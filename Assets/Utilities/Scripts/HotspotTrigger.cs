using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotspotTrigger : MonoBehaviour {

    public GameObject hotspotCollectionGO;
    private HotspotManager hotspotMgr;

	// Use this for initialization
	void Start () {
        hotspotMgr = hotspotCollectionGO.GetComponent<HotspotManager>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider col)
    {
        if (hotspotMgr != null)
        {
            hotspotMgr.HotspotTriggered(col.gameObject.name);
        }
    }


}
