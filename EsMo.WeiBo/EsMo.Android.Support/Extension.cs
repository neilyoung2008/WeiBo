using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace EsMo.Android.Support
{
   public static class Extension
    {
        public static Java.Lang.String ToJava(this string str)
        {
            return new Java.Lang.String(str);
        }
    }
}