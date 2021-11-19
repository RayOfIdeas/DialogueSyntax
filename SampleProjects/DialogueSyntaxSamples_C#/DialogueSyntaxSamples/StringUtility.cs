using System.Collections;
using System;
using System.Collections.Generic;
using System.Text;

namespace StringUtility
{
    public static class StringUtility
    {
        #region [Modifier Methods]

        /// <summary>
        /// Replace only the first occurence of the searched string
        /// </summary>
        /// <param name="text">whole text</param>
        /// <param name="search">to be replaced text</param>
        /// <param name="replace">new text to replace search</param>
        /// <returns>text that has been replaced</returns>
        public static string ReplaceFirst(this string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        /// <summary>
        /// Remove parts of text that are enclosed by an open and a close key
        /// </summary>
        /// <param name="text">input string</param>
        /// <param name="openToken">The string that indentify the start of the string</param>
        /// <param name="closeToken">The string that identify the end of the string</param>
        /// <param name="count">The number of removal; To remove all occurence : -1</param>
        /// <returns>The remaining text which has no enclosed texts and the keys</returns>
        public static string RemoveByKeys(string text, string openToken, string closeToken, int count = -1)
        {
            while (text.IndexOf(openToken) != -1)
            {
                if (count == 0) break;
                count--;

                int start = text.IndexOf(openToken);
                int end = text.IndexOf(closeToken);
                #region [Checking error]

                if (end == -1)
                {
                    Console.WriteLine("[StringUtility] RemoveByKeys: expecting another '" + closeToken + "' in this text:\n" + text);
                    break;
                }

                #endregion

                text = StringUtility.ReplaceFirst(text, text.Substring(start, (end + closeToken.Length) - start), "").Trim();
            }

            return text;
        }

        #endregion

        #region [Separator Methods]

        /// <summary>
        /// Segregate a text into texts using a separatorKey
        /// Separation starts after the key; Example:<br></br><br></br>
        /// input: "separtion zero [separation one [separation 2"<br></br>
        /// output: "[separation one", "[separation two"
        /// </summary>
        /// <param name="text">input string</param>
        /// <param name="separatorToken">The string that identify the separation from one text to another</param>
        /// <returns>A list of separated texts</returns>
        public static List<string> SeparateToListAfterKey(string text, string separatorToken)
        {
            List<string> separatedTexts = new List<string>();
            string _text = text;
            while (_text.Length > 0)
            {
                // Find the range of a segregation
                int start = _text.IndexOf(separatorToken);
                int end = _text.IndexOf(separatorToken, start + 1);
                if (end == -1) end = _text.Length - start;
                #region [Checking Error]
                if (start == -1)
                {
                    Console.WriteLine("[StringUtility] SeparateToList: expecting another '" + separatorToken + "'\n in this text: " + _text + "\n\nLocated inside this text:\n" + text);
                    break;
                }
                #endregion

                // Record a separated text into a list
                separatedTexts.Add(_text.Substring(start, end - start).Trim());

                // Remove the separated text from the whole text
                _text = StringUtility.ReplaceFirst(_text, _text.Substring(start, end - start), "").Trim();
            }

            return separatedTexts;
        }

        /// <summary>
        /// Segregate a text into texts using a separatorKey
        /// Separation starts before the key; Example:<br></br><br></br>
        /// input: "separtion zero [separation one [separation 2"<br></br>
        /// output: "separation zero [", "separation one ["
        /// </summary>
        /// <param name="text">input string</param>
        /// <param name="separatorToken">The string that identify the separation from one text to another</param>
        /// <returns>A list of separated texts</returns>
        public static List<string> SeparateToListBeforeKey(string text, string separatorToken)
        {
            List<string> separatedTexts = new List<string>();
            string _text = text;
            while (_text.Length > 0)
            {
                // Find the range of a segregation
                int start = 0;
                int end = _text.IndexOf(separatorToken);
                if (end == -1) break;

                // Record a separated text into a list
                separatedTexts.Add(_text.Substring(start, end + 1).Trim());

                // Remove the separated text from the whole text
                _text = StringUtility.ReplaceFirst(_text, _text.Substring(start, end + 1), "").Trim();
            }

            #region [Checking Error]
            if (_text.Length > 0)
            {
                Console.WriteLine("[StringUtility] SeparateToList: expecting another '" + separatorToken + "' in this text:\n" + _text + "\n\nLocated inside this text:\n" + text);
            }
            #endregion

            return separatedTexts;
        }

        #endregion

        #region [Extractor Methods]

        /// <summary>
        /// Extract the name and the parameter from a string that must contain both<br></br><br></br>
        /// input : "[myName] My paramater"<br></br>
        /// output: "myName", "My parameter"
        /// </summary>
        /// <param name="text">input string</param>
        /// <param name="nameOpenToken">The string that indentify the start of the name</param>
        /// <param name="nameCloseToken">The string that identify the end of the name</param>
        /// <returns>(name,parameter)</returns>
        public static (string, string) ExtractPairByNameKeys(string text, string nameOpenToken, string nameCloseToken)
        {
            // Extract name
            int startIndex_Name = text.IndexOf(nameOpenToken);
            int endIndex_Name = text.IndexOf(nameCloseToken);
            #region [Checking Error]
            if (startIndex_Name == -1)
            {
                Console.WriteLine("[StringUtility] ExtractPair: cannot find '" + nameOpenToken + "' in this text:\n" + text);
            }
            if (endIndex_Name == -1)
            {
                Console.WriteLine("[StringUtility] ExtractPair: cannot find '" + nameCloseToken + "' in this text:\n" + text);
            }
            #endregion

            string name = text.Substring(startIndex_Name + 1, endIndex_Name - startIndex_Name - 1).Trim();

            // Extract parameter
            int startIndex_Parameter = endIndex_Name + 1;
            int endINdex_Parameter = text.Length;
            string parameter = text.Substring(startIndex_Parameter, endINdex_Parameter - startIndex_Parameter).Trim();

            return (name, parameter);
        }

        /// <summary>
        /// Extract the name and the parameter from a string that must contain both<br></br><br></br>
        /// input: "my name {myParameter}"<br></br>
        /// output: "my name", "myParameter"
        /// </summary>
        /// <param name="text">input string</param>
        /// <param name="parameterOpenToken">The string that indentify the start of the name</param>
        /// <param name="parameterCloseToken">The string that identify the end of the name</param>
        /// <returns>(name,parameter)</returns>
        public static (string, string) ExtractPairByParameterKeys(string text, string parameterOpenToken, string parameterCloseToken)
        {
            // Extract name
            int startIndex_Name = 0;
            int endIndex_Name = text.IndexOf(parameterOpenToken);
            #region [Checking Error]
            if (endIndex_Name == -1)
            {
                Console.WriteLine("[StringUtility] ExtractPair: cannot find '" + parameterOpenToken + "' in this text:\n" + text);
            }
            #endregion

            string name = text.Substring(startIndex_Name, endIndex_Name - startIndex_Name).Trim();

            // Extract parameter
            int startIndex_Parameter = endIndex_Name + 1;
            int endINdex_Parameter = text.IndexOf(parameterCloseToken);
            #region [Checking Error]
            if (endIndex_Name == -1)
            {
                Console.WriteLine("[StringUtility] ExtractPair: cannot find '" + parameterCloseToken + "' in this text:\n" + text);
            }
            #endregion
            string parameter = text.Substring(startIndex_Parameter, endINdex_Parameter - startIndex_Parameter).Trim();

            return (name, parameter);
        }

        /// <summary>
        /// Extract a string that's enclosed by an open key and a close key
        /// </summary>
        /// <param name="text">input string</param>
        /// <param name="openToken">The string that indentify the start of the string</param>
        /// <param name="closeToken">The string that identify the end of the string</param>
        /// <returns>Text that's enclosed</returns>
        public static string? ExtractByKeys(string text, string openToken, string closeToken, bool suppressWarning = false)
        {
            int startIndex_Name = text.IndexOf(openToken);
            int endIndex_Name = text.IndexOf(closeToken);
            #region [Checking error]

            if (startIndex_Name == -1)
            {
                if (!suppressWarning) Console.WriteLine("[StringUtility] ExtractByKeys: cannot find '" + openToken + "' in this text:\n" + text);
                return null;
            }

            if (endIndex_Name == -1)
            {
                if (!suppressWarning) Console.WriteLine("[StringUtility] ExtractByKeys: cannot find '" + closeToken + "' in this text:\n" + text);
                return null;
            }

            #endregion

            return text.Substring(startIndex_Name + 1, endIndex_Name - (startIndex_Name + 1)).Trim();
        }

        /// <summary>
        /// Extract all texts that are enclosed by an open key and a close key
        /// </summary>
        /// <param name="text">input string</param>
        /// <param name="openToken">The string that indentify the start of the string</param>
        /// <param name="closeToken">The string that identify the end of the string</param>
        /// <returns>All texts that are enclosed</returns>
        public static List<string> ExtractAllByKeys(string text, string openToken, string closeToken)
        {
            List<string> extractedTexts = new List<string>();
            while (true)
            {
                string? extractedText = ExtractByKeys(text, openToken, closeToken);
                if (!string.IsNullOrEmpty(extractedText))
                {
                    extractedTexts.Add(extractedText);
                    text = RemoveByKeys(text, openToken, closeToken, 1);
                    if (string.IsNullOrEmpty(text)) break;
                }
                else break;
            }

            return extractedTexts;
        }

        #endregion
    }
}
