using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragPlatform : MonoBehaviour
{
    private Vector3 origin;
    private Vector3 difference;
    private bool isDragging = false;

    private Vector3 mouseWorldPos()
    {
        Vector3 result = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        result.z = 0.0f;

        return result;
    }

    private void ceilPosition()
    {
        Vector3 result = new Vector3(0, 0, 0);

        result.x = Mathf.Round(transform.position.x);
        result.y = Mathf.Round(transform.position.y);

        transform.position = result;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos(), Vector2.zero);

            if (hit && hit.transform.parent && hit.transform.parent.transform == transform && !isDragging && !GameManager.gameManager.isRunning)
            {
                origin = transform.position;
                difference = mouseWorldPos() - transform.position;
                isDragging = true;
            }
        }

        if (isDragging && Input.GetMouseButton(0) && !GameManager.gameManager.isRunning)
        {
            transform.position = mouseWorldPos() - difference;
        }

        if (isDragging && (!Input.GetMouseButton(0) || GameManager.gameManager.isRunning))
        {
            ceilPosition();

            for (int i = 0; i < transform.childCount; i++)
            {
                Transform childTransform = transform.GetChild(i).transform;

                RaycastHit2D[] hits = Physics2D.RaycastAll(new Vector3(childTransform.position.x + 0.5f, childTransform.position.y + 0.5f), Vector2.zero);

                //RaycastHit2D hit = childTransform.GetComponent<BoxCollider2D>().Raycast(new Vector3(childTransform.position.x + 0.5f, childTransform.position.y + 0.5f), Vector2.zero);
                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.transform != childTransform)
                    {
                        transform.position = origin;
                        break;
                    }
                }
            }

            isDragging = false;
        }
    }
}
