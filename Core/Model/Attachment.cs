using System;
using Live2k.Core.Model.Base;

namespace Live2k.Core.Model
{
    public class Attachment
    {
        public string Name { get; set; }
        public string Extension { get; set; }
        public string FillName => string.Format("{0}.{1}", Name, Extension);
        public string Description { get; set; }
        public UserSignature AddedBy { get; set; }
        public string Uri { get; set; }
    }
}
