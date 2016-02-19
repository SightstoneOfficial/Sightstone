using System;

namespace RtmpSharp.IO
{
    internal class ClassDescription
    {
        internal ClassDescription(string name, IMemberWrapper[] members, bool externalizable, bool dynamic)
        {
            Name = name;
            Members = members;
            IsExternalizable = externalizable;
            IsDynamic = dynamic;
        }

        public string Name { get; }
        public IMemberWrapper[] Members { get; private set; }
        public bool IsExternalizable { get; private set; }
        public bool IsDynamic { get; private set; }

        public bool IsTyped
        {
            get { return !string.IsNullOrEmpty(Name); }
        }

        public virtual bool TryGetMember(string name, out IMemberWrapper memberWrapper)
        {
            throw new NotImplementedException();
        }
    }
}