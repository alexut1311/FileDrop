using System;

namespace FileDrop.TL.Helpers
{
   public static class FileHelper
   {
      private static readonly string[] SUFFIXES = { "Bytes", "KB", "MB", "GB", "TB", "PB" };

      public static string FormatSize(long bytes)
      {
         int counter = 0;
         decimal number = bytes;
         while (Math.Round(number / 1024) >= 1)
         {
            number /= 1024;
            counter++;
         }

         return $"{number:n1} {SUFFIXES[counter]}";
      }
   }
}
