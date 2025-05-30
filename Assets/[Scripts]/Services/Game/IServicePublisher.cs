using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IServicePublisher<T>
{
    E GetService<E>() where E : T;
}
