﻿using System;
using Csla.Reflection;

namespace Csla.Server
{
  /// <summary>
  /// Invoke data portal methods on child
  /// objects.
  /// </summary>
  public class ChildDataPortal
  {
    #region  Data Access

    /// <summary>
    /// Create a new business object.
    /// </summary>
    /// <param name="objectType">Type of business object to create.</param>
    /// <param name="parameters">
    /// Criteria parameters passed from caller.
    /// </param>
    public object Create(System.Type objectType, params object[] parameters)
    {
      object obj = null;
      IDataPortalTarget target = null;
      var eventArgs = new DataPortalEventArgs(null, objectType, DataPortalOperations.Create);
      try
      {
        // create an instance of the business object
        obj = Activator.CreateInstance(objectType, true);

        target = obj as IDataPortalTarget;

        if (target != null)
        {
          target.Child_OnDataPortalInvoke(eventArgs);
          target.MarkAsChild();
          target.MarkNew();
        }
        else
        {
          MethodCaller.CallMethodIfImplemented(obj, "Child_OnDataPortalInvoke",
            eventArgs);
          MethodCaller.CallMethodIfImplemented(obj, "MarkAsChild");
          MethodCaller.CallMethodIfImplemented(obj, "MarkNew");
        }


        // tell the business object to fetch its data
        MethodCaller.CallMethod(obj, "Child_Create", parameters);

        if (target != null)
          target.Child_OnDataPortalInvokeComplete(eventArgs);
        else
          MethodCaller.CallMethodIfImplemented(obj, "Child_OnDataPortalInvokeComplete",
            eventArgs);

        // return the populated business object as a result
        return obj;

      }
      catch (Exception ex)
      {
        try
        {
          if (target != null)
            target.Child_OnDataPortalException(eventArgs, ex);
          else
            MethodCaller.CallMethodIfImplemented(obj, "Child_OnDataPortalException",
              eventArgs, ex);
        }
        catch
        {
          // ignore exceptions from the exception handler
        }
        throw new Csla.DataPortalException("ChildDataPortal.Create " + Properties.Resources.FailedOnServer, ex, obj);
      }

    }

    /// <summary>
    /// Get an existing business object.
    /// </summary>
    /// <param name="objectType">Type of business object to retrieve.</param>
    /// <param name="parameters">
    /// Criteria parameters passed from caller.
    /// </param>
    public object Fetch(Type objectType, params object[] parameters)
    {

      object obj = null;
      IDataPortalTarget target = null;
      var eventArgs = new DataPortalEventArgs(null, objectType, DataPortalOperations.Fetch);
      try
      {
        // create an instance of the business object
        obj = Activator.CreateInstance(objectType, true);

        target = obj as IDataPortalTarget;

        if (target != null)
        {
          target.Child_OnDataPortalInvoke(eventArgs);
          target.MarkAsChild();
          target.MarkOld();
        }
        else
        {
          MethodCaller.CallMethodIfImplemented(obj, "Child_OnDataPortalInvoke",
            eventArgs);
          MethodCaller.CallMethodIfImplemented(obj, "MarkAsChild");
          MethodCaller.CallMethodIfImplemented(obj, "MarkOld");
        }

        // tell the business object to fetch its data
        MethodCaller.CallMethod(obj, "Child_Fetch", parameters);

        if (target != null)
          target.Child_OnDataPortalInvokeComplete(eventArgs);
        else
          MethodCaller.CallMethodIfImplemented(obj, "Child_OnDataPortalInvokeComplete",
            eventArgs);

        // return the populated business object as a result
        return obj;

      }
      catch (Exception ex)
      {
        try
        {
          if (target != null)
            target.Child_OnDataPortalException(eventArgs, ex);
          else
            MethodCaller.CallMethodIfImplemented(obj, "Child_OnDataPortalException",
              eventArgs, ex);
        }
        catch
        {
          // ignore exceptions from the exception handler
        }
        throw new Csla.DataPortalException("ChildDataPortal.Fetch " + Properties.Resources.FailedOnServer, ex, obj);
      }

    }

    /// <summary>
    /// Update a business object.
    /// </summary>
    /// <param name="obj">Business object to update.</param>
    /// <param name="parameters">
    /// Parameters passed to method.
    /// </param>
    public void Update(object obj, params object[] parameters)
    {

      var operation = DataPortalOperations.Update;
      Type objectType = obj.GetType();
      IDataPortalTarget target = obj as IDataPortalTarget;
      try
      {
        if (target != null)
          target.Child_OnDataPortalInvoke(
            new DataPortalEventArgs(null, objectType, operation));
        else
          MethodCaller.CallMethodIfImplemented(obj, "Child_OnDataPortalInvoke",
            new DataPortalEventArgs(null, objectType, operation));

        // tell the business object to update itself
        if (obj is Core.BusinessBase)
        {
          Core.BusinessBase busObj = (Core.BusinessBase)obj;
          if (busObj.IsDeleted)
          {
            if (!busObj.IsNew)
            {
              // tell the object to delete itself
              MethodCaller.CallMethod(busObj, "Child_DeleteSelf", parameters);
            }
            if (target != null)
              target.MarkNew();
            else
              MethodCaller.CallMethodIfImplemented(busObj, "MarkNew");

          }
          else
          {
            if (busObj.IsNew)
            {
              // tell the object to insert itself
              MethodCaller.CallMethod(busObj, "Child_Insert", parameters);

            }
            else
            {
              // tell the object to update itself
              MethodCaller.CallMethod(busObj, "Child_Update", parameters);
            }
            if (target != null)
              target.MarkOld();
            else
              MethodCaller.CallMethodIfImplemented(busObj, "MarkOld");
          }

        }
        else if (obj is CommandBase)
        {
          // tell the object to update itself
          MethodCaller.CallMethod(obj, "Child_Execute", parameters);
          operation = DataPortalOperations.Execute;

        }
        else
        {
          // this is an updatable collection or some other
          // non-BusinessBase type of object
          // tell the object to update itself
          MethodCaller.CallMethod(obj, "Child_Update", parameters);
          if (target != null)
            target.MarkOld();
          else
            MethodCaller.CallMethodIfImplemented(obj, "MarkOld");
        }

        if (target != null)
          target.Child_OnDataPortalInvokeComplete(
            new DataPortalEventArgs(null, objectType, operation));
        else
          MethodCaller.CallMethodIfImplemented(obj, "Child_OnDataPortalInvokeComplete",
            new DataPortalEventArgs(null, objectType, operation));

      }
      catch (Exception ex)
      {
        try
        {
          if (target != null)
            target.Child_OnDataPortalException(
              new DataPortalEventArgs(null, objectType, operation), ex);
          else
            MethodCaller.CallMethodIfImplemented(obj, "Child_OnDataPortalException",
              new DataPortalEventArgs(null, objectType, operation), ex);
        }
        catch
        {
          // ignore exceptions from the exception handler
        }
        throw new Csla.DataPortalException("ChildDataPortal.Update " + Properties.Resources.FailedOnServer, ex, obj);
      }

    }

    #endregion
  }
}