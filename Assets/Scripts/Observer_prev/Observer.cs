using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Observer
{
    void OnNotify(object obj, NotificationType notificationType);
}

public enum NotificationType
{
    numberBox,
    box,
    container_box
}