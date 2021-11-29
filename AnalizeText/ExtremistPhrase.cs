using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;

namespace AnalizeText
{
    /*
     * Класс, хранящий в себе данные об экстремистской фразе из словаря.
     */
    public class ExtremistPhrase
    {
        /*
         * phrase - сама фраза.
         * typePhrase - тип фразы по виду экстремизма.
         */
        public string phrase;
        public string typePhrase;

        public ExtremistPhrase (string _phrase, string _typePhrase)
        {
            phrase = NotUndueSpacesText(_phrase);
            typePhrase = _typePhrase;
        }

        /*
         * Функция, возвращающая слова фразы в виде массива.
         */
        public string[] PhraseWords()
        {
            string normalizePhrase = Regex.Replace(phrase, "[»«\"–.?!)(,:;'-]", "");
            normalizePhrase = NotUndueSpacesText(normalizePhrase);
            normalizePhrase = normalizePhrase.ToLower();
            return normalizePhrase.Split(new char[] { ' ' });
        }

        /*
         * Функция, удаляющая лишние пробелы из фразы. 
         */
        private string NotUndueSpacesText(string normalizeText)
        {
            while (normalizeText.Contains("  "))
                normalizeText = normalizeText.Replace("  ", " ");
            return normalizeText;
        }

        /*
         * Переопределение функции сравнения двух объектов класса ExtremistPhrase.
         */
        public override bool Equals(object obj)
        {
            if(obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            ExtremistPhrase extremistPhrase = (ExtremistPhrase)obj;
            return (this.phrase == extremistPhrase.phrase) &&
            (this.typePhrase == extremistPhrase.typePhrase);
        }

        /*
         * Переопределение хэш-функции для класса ExtremistPhrase.
         * Необходимо переопределить из-за переопределения функции Equals.
         */
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
