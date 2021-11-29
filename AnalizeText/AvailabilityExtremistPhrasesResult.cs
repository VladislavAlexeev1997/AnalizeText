using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalizeText
{
    /*
     * Класс, формирующий результат оценки вероятности экстремистского характера текста на основании 
     *     присутствия в нём характерных слов и выражений.
     * Предназначен для вывода данных функции AvailabilityExtremistPhrases класса ExtremistAnalysisText.
     */
    public class AvailabilityExtremistPhrasesResult
    {
        /*
         * probabilityUsingExtremistPhrases - массив значений вероятности присутствия фраз или слов того или иного 
         *     вида экстремизма в тексте, в %.
         * indexSpacesOfStartUsingPhrases - индексы пробелов перед экстремистскими фразами в тексте.
         * indexSpacesOfEndUsingPhrases - индексы пробелов после экстремистских фраз в тексте.
         */
        public double[] probabilityUsingExtremistPhrases;
        public List<int> indexSpacesOfStartUsingPhrases;
        public List<int> indexSpacesOfEndUsingPhrases;

        public AvailabilityExtremistPhrasesResult(double[] _probabilityUsingExtremistPhrases,
            List<int> _indexSpacesOfStartUsingPhrase, List<int> _indexSpacesOfEndUsingPhrases)
        {
            probabilityUsingExtremistPhrases = _probabilityUsingExtremistPhrases;
            indexSpacesOfStartUsingPhrases = _indexSpacesOfStartUsingPhrase;
            indexSpacesOfEndUsingPhrases = _indexSpacesOfEndUsingPhrases;
        }

        /*
         * Переопределение функции сравнения двух объектов класса AvailabilityExtremistPhrasesResult.
         * Необходим для тестирования.
         */
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            AvailabilityExtremistPhrasesResult aEPR = (AvailabilityExtremistPhrasesResult)obj;
            return (this.probabilityUsingExtremistPhrases.SequenceEqual(aEPR.probabilityUsingExtremistPhrases, new DoubleSequenceEqualComparer())) &&
            (this.indexSpacesOfStartUsingPhrases.SequenceEqual(aEPR.indexSpacesOfStartUsingPhrases, new IntSequenceEqualComparer())) &&
            (this.indexSpacesOfEndUsingPhrases.SequenceEqual(aEPR.indexSpacesOfEndUsingPhrases, new IntSequenceEqualComparer()));
        }

        /*
         * Переопределение хэш-функции для класса AvailabilityExtremistPhrasesResult.
         * Необходимо переопределить из-за переопределения функции Equals.
         */
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
