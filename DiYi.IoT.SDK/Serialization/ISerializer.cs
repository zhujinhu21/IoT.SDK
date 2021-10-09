﻿
using System;
using System.Threading.Tasks;
using DiYi.IoT.SDK.Messages;
using JetBrains.Annotations;

namespace DiYi.IoT.SDK.Serialization
{
    public interface ISerializer
    {
        /// <summary>
        /// Serializes the given <see cref="Message"/> into a string
        /// </summary>
        string Serialize(Message message);

        /// <summary>
        /// Serializes the given <see cref="Message"/> into a <see cref="TransportMessage"/>
        /// </summary>
        Task<TransportMessage> SerializeAsync(Message message);

        /// <summary>
        /// Deserialize the given string into a <see cref="Message"/>
        /// </summary>
        Message Deserialize(string json);

        /// <summary>
        /// Deserialize the given <see cref="TransportMessage"/> back into a <see cref="Message"/>
        /// </summary>
        Task<Message> DeserializeAsync(TransportMessage transportMessage, [CanBeNull] Type valueType);

        /// <summary>
        /// Deserialize the given object with the given Type into an object
        /// </summary>
        object Deserialize(object value, Type valueType);

        /// <summary>
        /// Check if the given object is of Json type, e.g. JToken or JsonElement
        /// depending on the type of serializer implemented
        /// </summary>
        /// <example>
        /// <code>
        /// // Example implementation for System.Text.Json
        /// public bool IsJsonType(object jsonObject)
        /// {
        ///    return jsonObject is JsonElement;
        /// }
        /// </code>
        /// </example>
        bool IsJsonType(object jsonObject);
    }
}