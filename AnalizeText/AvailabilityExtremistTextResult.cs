using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalizeText
{
    /*
     * Класс, формирующий результат схожести введенного текста с определенным экстремистким текстом.
     * Предназначен для вывода данных функции AvailabilityExtremistText класса ExtremistAnalysisText.
     */
    public class AvailabilityExtremistTextResult
    {
        /*
         * usindProbability - поле, хранящее процентное соотношение использования экстремистского текста в введенном тексте.
         * usingExtremistText - экстремисткий текст.
         * usingWordsStartLocation - список позиций 1ых символов 1ых слов каждой фразы исходного текста, взятых из экстремисткого текста.
         *     Используется при раскрашивании фраз в интерфейсе.
         * usingWordsLastLocation - список позиций последних символов последних слов каждой фразы исходного текста, взятых из 
         *     экстремисткого текста. Используется при раскрашивании фраз в интерфейсе.
         */
        public double usindProbability;
        public ExtremistText usingExtremistText;
        public List<int> usingWordsStartLocation;
        public List<int> usingWordsLastLocation;

        public AvailabilityExtremistTextResult(double _usindProbability, ExtremistText _usingExtremistText,
            List<int> _usingWordsStartLocation, List<int> _usingWordsLastLocation)
        {
            usindProbability = _usindProbability;
            usingExtremistText = _usingExtremistText;
            usingWordsStartLocation = _usingWordsStartLocation;
            usingWordsLastLocation = _usingWordsLastLocation;
        }

        /*
         * Переопределение функции сравнения двух объектов класса AvailabilityExtremistTextResult.
         * Необходим для тестирования.
         */
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            AvailabilityExtremistTextResult aETR = (AvailabilityExtremistTextResult)obj;
            return (this.usindProbability == aETR.usindProbability) &&
            (this.usingExtremistText.Equals(aETR.usingExtremistText)) &&
            (this.usingWordsStartLocation.SequenceEqual(aETR.usingWordsStartLocation, new IntSequenceEqualComparer())) &&
            (this.usingWordsLastLocation.SequenceEqual(aETR.usingWordsLastLocation, new IntSequenceEqualComparer()));
        }

        /*
         * Переопределение хэш-функции для класса AvailabilityExtremistTextResult.
         * Необходимо переопределить из-за переопределения функции Equals.
         */
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
