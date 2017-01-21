// Generic event classes
public class CallbackEvent
{
    public System.Action callbacks;

    /// <summary>
    /// Should be called only during initialization, make sure to call Unregister in shutdown code
    /// </summary>
    public void Register(System.Action callback)
    {
        callbacks += callback;
    }

    /// <summary>
    /// Should be called in shutdown code
    /// </summary>
    public void Unregister(System.Action callback)
    {
        callbacks -= callback;
    }

    public void Dispatch()
    {
        if (callbacks != null)
        {
            callbacks();
        }
    }
}

public class CallbackEvent<T>
{
    public System.Action<T> callbacks;

    /// <summary>
    /// Should be called only during initialization, make sure to call Unregister in shutdown code
    /// </summary>
    public void Register(System.Action<T> callback)
    {
        callbacks += callback;
    }

    /// <summary>
    /// Should be called in shutdown code
    /// </summary>
    public void Unregister(System.Action<T> callback)
    {
        callbacks -= callback;
    }

    public void Dispatch(T data)
    {
        if(callbacks != null)
        {
            callbacks(data);
        }
    }
}

public class CallbackEvent<T1, T2>
{
    public System.Action<T1,T2> callbacks;

    /// <summary>
    /// Should be called only during initialization, make sure to call Unregister in shutdown code
    /// </summary>
    public void Register(System.Action<T1, T2> callback)
    {
        callbacks += callback;
    }

    /// <summary>
    /// Should be called in shutdown code
    /// </summary>
    public void Unregister(System.Action<T1, T2> callback)
    {
        callbacks -= callback;
    }

    public void Dispatch(T1 data1, T2 data2)
    {
        if (callbacks != null)
        {
            callbacks(data1, data2);
        }
    }
}

public class CallbackEvent<T1, T2, T3>
{
    public System.Action<T1, T2, T3> callbacks;

    /// <summary>
    /// Should be called only during initialization, make sure to call Unregister in shutdown code
    /// </summary>
    public void Register(System.Action<T1, T2, T3> callback)
    {
        callbacks += callback;
    }

    /// <summary>
    /// Should be called in shutdown code
    /// </summary>
    public void Unregister(System.Action<T1, T2, T3> callback)
    {
        callbacks -= callback;
    }

    public void Dispatch(T1 data1, T2 data2, T3 data3)
    {
        if (callbacks != null)
        {
            callbacks(data1, data2, data3);
        }
    }
}

public class CallbackEvent<T1, T2, T3, T4>
{
    public System.Action<T1, T2, T3, T4> callbacks;

    /// <summary>
    /// Should be called only during initialization, make sure to call Unregister in shutdown code
    /// </summary>
    public void Register(System.Action<T1, T2, T3, T4> callback)
    {
        callbacks += callback;
    }

    /// <summary>
    /// Should be called in shutdown code
    /// </summary>
    public void Unregister(System.Action<T1, T2, T3, T4> callback)
    {
        callbacks -= callback;
    }

    public void Dispatch(T1 data1, T2 data2, T3 data3, T4 data4)
    {
        if (callbacks != null)
        {
            callbacks(data1, data2, data3, data4);
        }
    }
}