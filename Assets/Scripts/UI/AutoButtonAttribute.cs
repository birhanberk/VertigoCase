using System;
using UnityEngine;

namespace UI
{
    [AttributeUsage(AttributeTargets.Field)]
    public class AutoButtonAttribute : PropertyAttribute
    {
        public string Keyword { get; }

        public AutoButtonAttribute(string keyword)
        {
            Keyword = keyword;
        }
    }
}
