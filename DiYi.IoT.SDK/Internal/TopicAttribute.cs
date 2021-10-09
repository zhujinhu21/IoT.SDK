﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DiYi.IoT.SDK.Internal
{
    /// <inheritdoc />
    /// <summary>
    /// An abstract attribute that for kafka attribute or rabbit mq attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public abstract class TopicAttribute : Attribute
    {
        protected TopicAttribute(string name, bool isPartial = false)
        {
            Name = name;
            IsPartial = isPartial;
        }

        /// <summary>
        /// Topic or exchange route key name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Defines wether this attribute defines a topic subscription partial.
        /// The defined topic will be combined with a topic subscription defined on class level,
        /// which results for example in subscription on "class.method".
        /// </summary>
        public bool IsPartial { get; }

        /// <summary>
        /// Default group name is CapOptions setting.(Assembly name)
        /// kafka --> groups.id
        /// rabbit MQ --> queue.name
        /// </summary>
        public string Group { get; set; }
    }
}
