## Keyboard Service

The intent of the KeyboardService is to provide an abstracted way of reliably handling keyboard input. It was initially created to handle the back and forward gestures in the NavigationService but has been abstracted in this service for general use.

````csharp Properties
//aids in processing control key
public Action AfterControlEGesture {get;set;}


public Action AfterMenuGesture{ get;set;}

````