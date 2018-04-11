using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase1Controller : MonoBehaviour {

    public Transform[] path;
    public Transform character;

    //Movement
    public float speed;
    public float jumpForce;

    Vector3 lookTarget;

    public enum Direction { Forward, Reverse};
    Direction playerDirection;
    Transform targetNode;
    int targetNodeInt;

    private void OnDrawGizmos()
    {
        iTween.DrawPath(path, Color.blue);
    }

    private void Start()
    {
        character.position = path[0].position;
        targetNode = path[1];
        targetNodeInt = 1;
    }

    private void Update()
    {
        Movement(KeyInput());
        FaceCurrentNode(targetNode);
    }

    Direction KeyInput()
    {
        if(Input.GetKey(KeyCode.D))
        {
            playerDirection = Direction.Forward;
            character.transform.position = Vector3.MoveTowards(character.transform.position, targetNode.position, speed * Time.deltaTime);
            if (character.position == targetNode.position)
            {
                Debug.Log("+1 Right Node");
                targetNode = path[targetNodeInt + 1];
            }
        }

        if (Input.GetKey(KeyCode.A))
        {
            playerDirection = Direction.Reverse;
            character.transform.position = Vector3.MoveTowards(character.transform.position, path[targetNodeInt - 1].position, speed * Time.deltaTime);
        }

        return playerDirection;
    }

    void Movement(Direction dir)
    {
        if(dir == Direction.Forward)
        {
            if(character.position.x == targetNode.position.x)
            {
                Debug.Log("Shouldn't be here yet");
                targetNode = path[targetNodeInt + 1];
            }
        }
        if (dir == Direction.Reverse)
        {
          
        }
    }

    void FaceCurrentNode(Transform currentNode)
    {
        character.GetChild(0).transform.LookAt(new Vector3(currentNode.transform.position.x, character.GetChild(0).transform.position.y, currentNode.transform.position.z));
    }

}
