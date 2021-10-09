// Copyright (c) .NET Core Community. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DiYi.IoT.SDK.Internal;


// ReSharper disable once CheckNamespace
namespace DiYi.IoT.SDK
{
    public class IoTSubscribeAttribute : TopicAttribute
    {
        public IoTSubscribeAttribute(string name, bool isPartial = false)
            : base(name, isPartial)
        {

        }

        public override string ToString()
        {
            return Name;
        }
    }

    [AttributeUsage(AttributeTargets.Parameter)]
    public class FromIoTAttribute : Attribute
    {
       
    }

    public class IoTHeader : ReadOnlyDictionary<string, string>
    {
        public IoTHeader(IDictionary<string, string> dictionary) : base(dictionary)
        {

        }
    }
}