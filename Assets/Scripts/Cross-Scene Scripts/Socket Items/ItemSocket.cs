using System;
using UnityEngine;

public enum ItemSocketLocation
{
    A,
    B,
    C,
    D,
    E,
}

public class ItemSocket : MonoBehaviour
{
    [SerializeField] ItemSocketLocation socketMatch;
    [SerializeField] private SocketDragItem parentContainer;

    [HideInInspector]
    public bool isDragging;
    private ItemSocket otherSocket;

    private void ConnectSockets()
    {
        //Socket that touched, has the same location as this one
        if (socketMatch == otherSocket.socketMatch)
        {
            SocketInformation thisSocket = null;

            //Set this socket in entire item as connected
            foreach (SocketInformation socket in parentContainer.socketsInfo)
            {
                if (this == socket.socket)
                {
                    thisSocket = socket;
                    socket.isConnected = true;
                    break;
                }
            }

            //Set the socket on the other item as connected
            SocketDragItem otherDragItem = otherSocket.GetComponentInParent<SocketDragItem>();
            foreach (SocketInformation otherSocketForEach in otherDragItem.socketsInfo)
            {
                if (otherSocket == otherSocketForEach.socket)
                {
                    otherSocketForEach.isConnected = true;
                    otherSocketForEach.connection = this;
                    thisSocket.connection = otherSocketForEach.socket;
                    break;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!isDragging)
        {
            return;
        }

        else if (otherSocket = collision.GetComponent<ItemSocket>())
        {
            ConnectSockets();
        }
    }
}
