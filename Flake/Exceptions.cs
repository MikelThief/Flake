/*
 * 
 * File:    General_Toolbox.cs
 * Author:  Michał Bator
 * 
 * Contains Exception classes.
 * 
 */


using System;

[Serializable()]
public class Parsing_Error_Exception : System.Exception
{
    public Parsing_Error_Exception() : base() { }
    public Parsing_Error_Exception(string message) : base(message) { }
    public Parsing_Error_Exception(string message, System.Exception inner) : base(message, inner) { }

    // A constructor is needed for serialization when an
    // exception propagates from a remoting server to the client. 
    protected Parsing_Error_Exception(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) { }
}

[Serializable()]
public class Undefined_Element_Exception : System.Exception
{
    public Undefined_Element_Exception() : base() { }
    public Undefined_Element_Exception(string message) : base(message) { }
    public Undefined_Element_Exception(string message, System.Exception inner) : base(message, inner) { }

    // A constructor is needed for serialization when an
    // exception propagates from a remoting server to the client. 
    protected Undefined_Element_Exception(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) { }
}

[Serializable()]
public class Empty_Directory_Exception : System.Exception
{
    public Empty_Directory_Exception() : base() { }
    public Empty_Directory_Exception(string message) : base(message) { }
    public Empty_Directory_Exception(string message, System.Exception inner) : base(message, inner) { }

    // A constructor is needed for serialization when an
    // exception propagates from a remoting server to the client. 
    protected Empty_Directory_Exception(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) { }
}

[Serializable()]
public class Invalid_Library_Version : System.Exception
{
    public Invalid_Library_Version() : base() { }
    public Invalid_Library_Version(string message) : base(message) { }
    public Invalid_Library_Version(string message, System.Exception inner) : base(message, inner) { }

    // A constructor is needed for serialization when an
    // exception propagates from a remoting server to the client. 
    protected Invalid_Library_Version(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) { }
}