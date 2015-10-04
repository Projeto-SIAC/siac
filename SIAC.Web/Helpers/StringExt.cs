using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Web.Helpers
{
    public class StringExt
    {
        public static bool IsNullOrEmpty(params string[] strs)
        {
            for (int i = 0; i < strs.Length; i++)
            {
                if (String.IsNullOrEmpty(strs[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsNullOrWhiteSpace(params string[] strs)
        {
            for (int i = 0; i < strs.Length; i++)
            {
                if (String.IsNullOrWhiteSpace(strs[i]))
                {
                    return false;
                }
            }
            return true;
        }
    }
}