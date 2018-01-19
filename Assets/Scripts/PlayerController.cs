using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float range;
    public MapGenerator map;

    Rigidbody2D rb;
    float realRange;

	void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        realRange = range * map.nodeSize;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f) * speed;
        rb.MovePosition(transform.position + movement);
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, range);
            if (hit.collider != null && Vector3.Distance(transform.position, hit.collider.transform.position) < realRange)
                if (hit.collider.gameObject.CompareTag("Breakable"))
                    Destroy(hit.collider.gameObject);
        }
    }

    void DestroyTile(int x, int y)
    {
        map.DestroyTile(x, y);
    }
}