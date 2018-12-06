using DiffMatchPatch;

namespace HGGM
{
    public static class DiffHelper
    {
        /// <summary>
        ///     Diffs the provided strings and returns a HTML formatted output
        /// </summary>
        /// <returns>HTML</returns>
        public static string Diff(this string before, string after)
        {
            return DiffMatchPatchModule.Default.DiffPrettyHtml(
                DiffMatchPatchModule.Default.DiffMain(before, after));
        }
    }
}