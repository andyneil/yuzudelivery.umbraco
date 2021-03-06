﻿using System;
using YuzuDelivery.Core;

namespace YuzuDelivery.Umbraco.Core
{
    public class YuzuGroupMapperSettings : YuzuMapperSettings
    {
        public Type Source { get; set; }
        public Type DestParent { get; set; }
        public Type DestChild { get; set; }
        public string PropertyName { get; set; }
        public string GroupName { get; set; }
    }
}
