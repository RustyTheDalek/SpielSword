using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookRope : MonoBehaviour 
{
    #region Public Fields

    public Vector2 destination;

    public float speed = 30;

    public float distance = 1;

    public GameObject player;

    #endregion

    #region Protected Fields
    #endregion

    #region Private Fields

    float distanceTravelled;

    Vector3 lastPosition;
    Vector3 deltaPos;

    GameObject lastNode;

    bool complete = false;

    #endregion

    #region Unity Methods
    void Start()
    {
        distanceTravelled = 0;

        lastPosition = transform.position;

        lastNode = this.gameObject;
    }
 
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);

        if ((Vector2)transform.position != destination)
        {
            //if (Vector2.Distance(player.transform.position, lastNode.transform.position) > distance)
            //{
            //    CreateNode();
            //}
            //Here we are checking how far the rope is travelling so we can add new 
            //RopeNodes when enough distance is travelled
            deltaPos = transform.position - lastPosition;
            distanceTravelled += deltaPos.magnitude;

            if (distanceTravelled > 2)
            {
                CreateNode();

                distanceTravelled = 0;
            }

            if (distanceTravelled > 20)
            {
                complete = true;

                lastNode.GetComponent<HingeJoint2D>().connectedBody = player.GetComponent<Rigidbody2D>();

            }

            lastPosition = transform.position;

        }
        else if (!complete)
        {
            complete = true;

            lastNode.GetComponent<HingeJoint2D>().connectedBody = player.GetComponent<Rigidbody2D>();
        }
    }
    #endregion

    #region Private Methods

    void CreateNode()
    {
        Vector3 pos2Create = player.transform.position - lastNode.transform.position;
        pos2Create.Normalize();
        pos2Create *= distance;
        pos2Create += lastNode.transform.position;

        GameObject go = AssetManager.RopeNode.Spawn(pos2Create);

        go.transform.SetParent(transform);

        lastNode.GetComponent<HingeJoint2D>().connectedBody = go.GetComponent<Rigidbody2D>();

        lastNode = go;
    }

    #endregion
}
