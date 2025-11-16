using System;
using UnityEngine;

namespace UI.AutoButton
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
