using UnityEngine;

/// <summary>
/// ServiceInitializer is responsible for registering and initializing all core game services
/// at the start of the game, and shutting them down cleanly when the object is destroyed.
/// Attach this script to a GameObject in your scene.
/// </summary>
public class ServiceInitializer : MonoBehaviour
{
    [SerializeField] private bool _isNetworked = false;

    private void Awake()
    {
        InitializeServices(_isNetworked);
    }

    /// <summary>
    /// Registers all required services and initializes them in dependency order.
    /// Add your service registration logic here.
    /// </summary>
    private void InitializeServices(bool isNetworked)
    {
        // Register dependencies first if needed
        //ServiceLocator.RegisterDependency<IPlayerService, IInputService>();

        // Create and register services (with null checks)
        ServiceLocator.Register<GameManager>(new GameManager());

        // Add networked services if needed
        if (isNetworked)
        {
            //ServiceLocator.Register<INetworkService>(new NetworkService());
        }

        // Initialize services in dependency order
        var orderedServices = ServiceLocator.GetServicesInDependencyOrder();
        foreach (var service in orderedServices)
        {
            if (service is IService gs)
                gs.Initialize();
        }
    }

    /// <summary>
    /// Helper method to register a MonoBehaviour service attached to this GameObject.
    /// </summary>
    private void RegisterService<T>() where T : class
    {
        var service = gameObject.GetComponent<T>();
        if (service != null)
            ServiceLocator.Register(service);
        else
            Debug.LogWarning($"Service of type {typeof(T).Name} not found on GameObject.");
    }

    /// <summary>
    /// Shuts down all registered services in reverse dependency order when the game ends or this object is destroyed.
    /// </summary>
    // TODO: Ensure shutdown order is correct
    private void OnDestroy()
    {
        // Shutdown services in dependency order (BUG: should be reverse order)
        var orderedServices = ServiceLocator.GetServicesInDependencyOrder();
        foreach (var service in orderedServices)
        {
            if (service is IService gs)
                gs.Shutdown();
        }
    }
}