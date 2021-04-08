using System;
using System.Reflection;

namespace MooltiPackServerMod {
  public static class ReflectionExtensions {
    public static T XXX_GetFieldValue<T>(this object obj, string name) {
      var bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;
      var field = obj.GetType().GetField(name, bindingFlags);
      return (T)field?.GetValue(obj);
    }
    // e.g. .XXX_GetMethod("foo", new Type[] { typeof(int), typeof(byte[]) }) // finds `void foo(inf, byte[])`
    public static MethodInfo XXX_GetMethod(this object obj, string name, Type[] parameterTypes = null) {
      var bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;
      var method = obj.GetType().GetMethod(name, bindingFlags, null, CallingConventions.Any, parameterTypes, null);
      return method;
    }
    public static T XXX_InvokeMethod<T>(this object obj, string name, Type[] parameterTypes = null, object[] parameters = null) {
      var result = _XXX_InvokeMethod(obj, name, parameterTypes, parameters);
      return (T)result;
    }
    public static void XXX_InvokeVoidMethod(this object obj, string name, Type[] parameterTypes = null, object[] parameters = null) {
      _XXX_InvokeMethod(obj, name, parameterTypes, parameters);
    }
    private static object _XXX_InvokeMethod(this object obj, string name, Type[] parameterTypes = null, object[] parameters = null) {
      var bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;
      if (parameters == null) {
        parameters = new object[0];
        parameterTypes = new Type[0];
      }
      var type = obj.GetType();
      var method = type.GetMethod(name, bindingFlags, null, CallingConventions.Any, parameterTypes, null);
      var result = method.Invoke(obj, bindingFlags, null, parameters, null);
      return result;
    }
  }
}