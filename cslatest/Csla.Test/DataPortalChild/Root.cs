﻿using System;
using System.Collections.Generic;
using System.Text;
using Csla;

namespace Csla.Test.DataPortalChild
{
  [Serializable]
  public class Root : BusinessBase<Root>
  {
    private static PropertyInfo<string> DataProperty = new PropertyInfo<string>("Data");
    public string Data
    {
      get { return GetProperty<string>(DataProperty); }
      set { SetProperty<string>(DataProperty, value); }
    }

    private static PropertyInfo<Child> ChildProperty = new PropertyInfo<Child>("Child");
    public Child Child
    {
      get 
      {
        if (!FieldManager.FieldExists(ChildProperty))
          SetProperty<Child>(ChildProperty, Child.NewChild());
        return GetProperty<Child>(ChildProperty); 
      }
    }

    private static PropertyInfo<ChildList> ChildListProperty = new PropertyInfo<ChildList>("ChildList");
    public ChildList ChildList
    {
      get
      {
        if (!FieldManager.FieldExists(ChildListProperty))
          SetProperty<ChildList>(ChildListProperty, ChildList.GetList());
        return GetProperty<ChildList>(ChildListProperty);
      }
    }

    public void FetchChild()
    {
      SetProperty<Child>(ChildProperty, Child.GetChild());
    }

    protected override void DataPortal_Insert()
    {
      FieldManager.UpdateChildren();
    }

    protected override void DataPortal_Update()
    {
      FieldManager.UpdateChildren();
    }
  }
}