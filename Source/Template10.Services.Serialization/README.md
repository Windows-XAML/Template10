# SerializationService
The `SerializationService` provides a simple abstracted interface to a service that implements consistent serialization and deserialization of objects:

```csharp
// Serialize the passed object to a string. Returns null if the parameter is null. Returns an empty string
// if the parameter is an empty string.
string Serialize(object parameter);

// Converts the passed string back to an object. If the passed parameter is null, null is returned. If the
// string is empty, an empty string is returned.
object Deserialize(string parameter);

// Converts the passed string back to a object of type T. If deserialization fails, the default value
// for type T is returned. Note that if T is the wrong type for the passed serialized value, an
// InvalidCastException can occur. This can be avoided by using the next method.
T Deserialize<T>(string parameter);

// Attempts to deserialize the value while catching any InvalidCastException that may occur. Returns
// true if a value value was obtained, otherwise false is returned, along with the defaul value for
// type T.
bool TryDeserialize<T>(string parameter, out T result);
```

The `NavigationService` sets up an instance to the Template 10-included JSON `SerializationService`:

```csharp
SerializationService = Service.SerializationService.SerializationService.Json;
```