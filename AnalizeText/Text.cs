using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AnalizeText
{
    /*
     * Класс для работы с текстом.
     */
    public class Text
    {
        /* 
         * fullText - поле для хранения исходного введенного текста. 
         *     В конструкторе идет удаление из этого текста лишних пробелов.
         */
        public string fullText;

        public Text(string _fullText)
        {
            fullText = NotUndueSpacesText(_fullText);
        }

        /* 
         * Метод нормализации текста. Удаляет все знаки препинания и лишние пробелы из текста, 
         *     а также переводит все символы в нижний регистр.
         */
        public string NormalizeText()
        {
            string normalizeText = Regex.Replace(fullText, "[\t»«\"–.?!)(,:;'-]", "");
            normalizeText = Regex.Replace(normalizeText, "\n", " ");
            normalizeText = NotUndueSpacesText(normalizeText);
            normalizeText = normalizeText.ToLower();
            return normalizeText;
        }

        /*
         * Вывод нормализованного текста в виде массива слов.
         */
        public string[] TextWords()
        {
            return NormalizeText().Split(new char[] { ' ' });
        }

        // Метод для нахождения положения (индекса) n-ого пробела в тексте. В данном случае n = countSpace.
        public int SpaceElementLocationTextIndex(int countSpace)
        {
            string text = fullText + " ";
            string[] spaceUntilElements = { "»", ",", ".", ":", ";", "!", "?", ")" };
            string[] spaceLaterElements = { "«", "(", "'", "\"", "–", "-" };
            string substring = " ";
            int index = 0;
            int nowIndex = 0;
            int countIndex = 0;
            while ((nowIndex = text.IndexOf(substring, nowIndex)) != -1 && countIndex < countSpace)
            {
                bool falseSpace = false;
                for (int indexElement = 0; indexElement < spaceLaterElements.Length; indexElement++)
                {
                    if (nowIndex >= 3)
                    {
                        if (" " + spaceLaterElements[indexElement] + " " == text.Substring(nowIndex - 2, 3))
                        {
                            falseSpace = true;
                            break;
                        }
                    }
                }
                if (!falseSpace)
                {
                    for (int indexElement = 0; indexElement < spaceUntilElements.Length; indexElement++)
                    {
                        if (nowIndex <= text.Length - 3)
                        {
                            if (" " + spaceUntilElements[indexElement] + " " == text.Substring(nowIndex, 3))
                            {
                                falseSpace = true;
                                break;
                            }
                        }
                    }
                }
                if (!falseSpace)
                {
                    index = nowIndex;
                    countIndex++;
                }
                nowIndex += substring.Length;
            }
            return index;
        }

        // Метод удаления лишних пробелов и переходов на новую строку из текста. 
        private string NotUndueSpacesText(string normalizeText)
        {
            while (normalizeText.Contains("  "))
                normalizeText = normalizeText.Replace("  ", " ");
            normalizeText = Regex.Replace(normalizeText, "\n", " \n");
            return normalizeText;
        }
    }
}