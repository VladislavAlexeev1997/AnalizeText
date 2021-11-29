using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalizeText
{
    /*
     * Класс для хранения информации экстремистского текста.
     *     Наследуется от класса Text.
     */
    public class ExtremistText : Text
    {
        /*
         * startWords - поле для хранения начальных слов экстремистского текста.
         * endWords - поле для хранения последних несколько слов экстремистского текста.
         */
        public string startWords;
        public string endWords;

        public ExtremistText(string _startWords, string _endWords, string _fullText)
            : base(_fullText)
        {
            startWords = _startWords;
            endWords = _endWords;
        }

        /*
         * Переопределение функции сравнения двух объектов класса ExtremistText.
         */
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            ExtremistText extremistText = (ExtremistText)obj;
            return (this.startWords == extremistText.startWords) &&
            (this.endWords == extremistText.endWords) &&
            (this.fullText == extremistText.fullText);
        }

        /*
         * Переопределение хэш-функции для класса ExtremistText.
         * Необходимо переопределить из-за переопределения функции Equals.
         */
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
