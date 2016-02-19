namespace RtmpSharp.IO
{
    internal interface IObjectWrapper
    {
        bool GetIsExternalizable(object instance);
        bool GetIsDynamic(object instance);
        // Gets the class definition for an object `obj`, applying transformations like type name mappings
        ClassDescription GetClassDescription(object obj);
    }

    internal interface IMemberWrapper
    {
        string Name { get; }
        string SerializedName { get; }
        object GetValue(object instance);
        void SetValue(object instance, object value);
    }
}