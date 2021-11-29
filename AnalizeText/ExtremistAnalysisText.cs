using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalizeText
{
    /*
     * Класс для исследования исходного текста на экстремистские тексты и фразы из словаря. 
     */
    public class ExtremistAnalysisText
    {
        // enterText - исходный (проверяемый) текст.
        private Text enterText;

        public ExtremistAnalysisText(Text _enterText)
        {
            enterText = _enterText;
        }

        /*
         * Функция нахождения совпадений введенного текста со всеми экстремисткими текстами.
         * vocabulary - словарь экстремистских текстов.
         * availabilities - список результатов совпадений. Состоит из элементов класса AvailabilityExtremistTextResult.
         */
        public List<AvailabilityExtremistTextResult> AvailabilityExtremistText(ExtremistTextVocabulary vocabulary)
        {
            string[] enterTextWords = enterText.TextWords();
            List<AvailabilityExtremistTextResult> availabilities = new List<AvailabilityExtremistTextResult>();
            foreach (ExtremistText extremistText in vocabulary.texts)
            {
                string[] extremistTextWords = extremistText.TextWords();
                List<int> countSpacesOfStartPhrases;
                List<int> startPhrasesLocation = new List<int>();
                List<int> lastPhrasesLocation = new List<int>();
                List<List<string>> availabilityExtremistPhrases =
                    TextWordsOfAvailabilityExtremistText(enterTextWords, extremistTextWords, out countSpacesOfStartPhrases);
                double probability = ProbabilityAvailabilityExtremistText(extremistText.TextWords(), availabilityExtremistPhrases);
                if (probability > 0)
                {
                    int indexPhrases = 0;
                    foreach (int countStartSpace in countSpacesOfStartPhrases)
                    {
                        int startIndex = enterText.SpaceElementLocationTextIndex(countStartSpace);
                        startPhrasesLocation.Add(startIndex);
                        int endIndex = enterText.SpaceElementLocationTextIndex(countStartSpace + availabilityExtremistPhrases[indexPhrases].Count);
                        lastPhrasesLocation.Add(endIndex);
                        indexPhrases++;
                    }
                    availabilities.Add(new AvailabilityExtremistTextResult(probability, extremistText,
                        startPhrasesLocation, lastPhrasesLocation));
                }
            }
            return availabilities;
        }

        /*
         * Функция нахождения фраз в исходном тексте, взятых из экстремистского текста. 
         * enterTextWords - введенный текст в виде массива слов.
         * extremistTextWords - экстремистский текст в виде массива слов.
         * countSpacesOfStartPhrases - список положений (индексов) начала фраз из экстремисткого текста, использованных в исходном тексте.
         * availabilityWords - список фраз из экстремисткого текста, использованных в исходном тексте. Каждая фраза - список слов.
         */
        private List<List<string>> TextWordsOfAvailabilityExtremistText(string[] enterTextWords, string[] extremistTextWords, out List<int> countSpacesOfStartPhrases)
        {
            List<List<string>> availabilityWords = new List<List<string>>();
            countSpacesOfStartPhrases = new List<int>();
            for (int enterIndex = 0; enterIndex < enterTextWords.Length; enterIndex++)
            {
                for (int extremistIndex = 0; extremistIndex < extremistTextWords.Length; extremistIndex++)
                {
                    if (WordCoincidence(enterTextWords[enterIndex], extremistTextWords[extremistIndex]))
                    {
                        List<string> searchList = new List<string>();
                        searchList.Add(extremistTextWords[extremistIndex]);
                        for (int searchIndex = 1; searchIndex < extremistTextWords.Length - extremistIndex; searchIndex++)
                        {
                            if (enterIndex + searchIndex < enterTextWords.Length)
                            {
                                if (WordCoincidence(enterTextWords[enterIndex + searchIndex], extremistTextWords[extremistIndex + searchIndex]))
                                {
                                    searchList.Add(extremistTextWords[extremistIndex + searchIndex]);
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        if (searchList.Count > 2)
                        {
                            availabilityWords.Add(searchList);
                            countSpacesOfStartPhrases.Add(enterIndex);
                            enterIndex += searchList.Count - 1;
                            break;
                        }
                    }
                }
            }
            return availabilityWords;
        }

        /*
         * Функция нахождения процентного соотношения использования экстремистского текста в введенном тексте.
         * extremistTextWords - экстремистский текст в виде массива слов.
         * availabilityExtremistText - список фраз из экстремисткого текста, использованных в исходном тексте. Каждая фраза - список слов.
         */
        private double ProbabilityAvailabilityExtremistText(string[] extremistTextWords,
            List<List<string>> availabilityExtremistText)
        {
            int[] periodicityTextWords = new int[extremistTextWords.Length];
            for (int phraseIndex = 0; phraseIndex < availabilityExtremistText.Count; phraseIndex++)
            {
                for (int wordIndex = 0; wordIndex < extremistTextWords.Length; wordIndex++)
                {
                    if (extremistTextWords[wordIndex] == availabilityExtremistText[phraseIndex][0])
                    {
                        int startIndex = wordIndex;
                        int endIndex = wordIndex;
                        bool isThisPhrase = true;
                        for (int tempIndex = 1; tempIndex < availabilityExtremistText[phraseIndex].Count; tempIndex++)
                        {
                            if (extremistTextWords[wordIndex + tempIndex] != availabilityExtremistText[phraseIndex][tempIndex])
                            {
                                isThisPhrase = false;
                                break;
                            }
                            else
                            {
                                endIndex++;
                            }
                        }
                        if (isThisPhrase)
                        {
                            for (int tempIndex = startIndex; tempIndex <= endIndex; tempIndex++)
                            {
                                periodicityTextWords[tempIndex]++;
                            }
                        }
                        else
                        {
                            wordIndex = endIndex;
                        }
                    }
                }
            }
            return Math.Round((double)periodicityTextWords.Count((element) => element > 0) * 100 / extremistTextWords.Length, 2);
        }

        /*
         * Функция нахождения совпадений введенного текста со всеми экстремисткими фразами и словами из словаря
         *     и оценки вероятности экстремистского характера текста на основании их присутствия.
         * vocabulary - словарь экстремистских слов и фраз.
         * indexSpacesOfStartPhrases - индексы пробелов перед экстремистскими фразами в тексте.
         * indexSpacesOfEndPhrases - индексы пробелов после экстремистских фраз в тексте.
         * probabilityExtremistPhrases - массив значений вероятности присутствия фраз или слов того или иного 
         *     вида экстремизма в тексте, в %.
         */
        public AvailabilityExtremistPhrasesResult AvailabilityExtremistPhrases(ExtremistPhraseVocabulary vocabulary)
        {
            string[] enterTextWords = enterText.TextWords();
            List<int> indexSpacesOfStartPhrases;
            List<int> indexSpacesOfEndPhrases;
            int[] occurenceExtremistPhrases = OccurenceCountExtremistPhrases(vocabulary, enterTextWords,
                out indexSpacesOfStartPhrases, out indexSpacesOfEndPhrases);
            double[] probabilityExtremistPhrases = ProbabilityExtremistPhrases(occurenceExtremistPhrases);
            return new AvailabilityExtremistPhrasesResult(probabilityExtremistPhrases,
                indexSpacesOfStartPhrases, indexSpacesOfEndPhrases);
        }

        /*
         * Функция расчета количества встречавшихся экстремистских фраз и выражений
         *     по их виду экстремизма.
         * vocabulary - словарь экстремистских фраз и слов.
         * textWords - исходный текст в виде массива слов.
         * occurenceExtremistPhrases - массив частот встречающихся экстремистских фраз и слов
         *     в тексте.
         * indexSpacesOfStartPhrases - индексы пробелов перед экстремистскими фразами в тексте.
         * indexSpacesOfEndPhrases - индексы пробелов после экстремистских фраз в тексте.
         */
        private int[] OccurenceCountExtremistPhrases(ExtremistPhraseVocabulary vocabulary, string[] textWords, 
            out List<int> indexSpacesOfStartPhrases, out List<int> indexSpacesOfEndPhrases)
        {
            List<string> extremistTypes = vocabulary.ExtremistTypes();
            int[] occurenceExtremistPhrases = new int[extremistTypes.Count];
            indexSpacesOfStartPhrases = new List<int>();
            indexSpacesOfEndPhrases = new List<int>();
            foreach (ExtremistPhrase extremistPhrase in vocabulary.phrases)
            {
                string[] phraseWords = extremistPhrase.PhraseWords();
                for (int wordIndex = 0; wordIndex < textWords.Length; wordIndex++)
                {
                    if (WordCoincidence(textWords[wordIndex], phraseWords[0]))
                    {
                        int availability = 1;
                        if (textWords.Length - wordIndex >= phraseWords.Length)
                        {
                            for (int phraseWordIndex = 1; phraseWordIndex < phraseWords.Length; phraseWordIndex++)
                            {
                                if (WordCoincidence(textWords[wordIndex + phraseWordIndex], phraseWords[phraseWordIndex]))
                                {
                                    availability++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            if (availability == phraseWords.Length)
                            {
                                occurenceExtremistPhrases[extremistTypes.IndexOf(extremistPhrase.typePhrase)]++;
                                indexSpacesOfStartPhrases.Add(enterText.SpaceElementLocationTextIndex(wordIndex));
                                indexSpacesOfEndPhrases.Add(enterText.SpaceElementLocationTextIndex(wordIndex + phraseWords.Length));
                            }
                        }
                        wordIndex += availability - 1;
                    }
                }
            }
            return occurenceExtremistPhrases;
        }

        /*
         * Функция расчета вероятности экстремистского характера текста на основании присутствия
         *     экстремистских фраз и слов из словаря.
         * occurenceExtremistPhrases - массив частот встречающихся экстремистских фраз и слов
         *     в тексте.
         * probabilityExtremistPhrases - массив значений вероятности присутствия фраз или слов того или иного 
         *     вида экстремизма в тексте, в %.
         */
        private double[] ProbabilityExtremistPhrases(int[] occurenceExtremistPhrases)
        {
            double[] probabilityExtremistPhrases = new double[occurenceExtremistPhrases.Length];
            int occurencePhraseSum = 0;
            foreach (int occurenceElement in occurenceExtremistPhrases)
            {
                occurencePhraseSum += occurenceElement;
            }
            if (occurencePhraseSum > 0)
            {
                for (int index = 0; index < probabilityExtremistPhrases.Length; index++)
                {
                    probabilityExtremistPhrases[index] = Math.Round((double)occurenceExtremistPhrases[index] / occurencePhraseSum * 100, 2);
                }
            }
            return probabilityExtremistPhrases;
        }

        /*
         * Функция сравнения двух слов. Функция выводит true при совпадении слов, а false - если слова не одинаковые.
         *     Одинаковые слова - это полностью совпадающие слова или слова, имеющие одинаковую основу.
         *     Основа слова вычисляется с помощью класса BringSeparateWord.
         */
        private bool WordCoincidence(string firstWord, string secondWord)
        {
            if (firstWord != secondWord)
            {
                if (firstWord.Length > 2 && secondWord.Length > 2)
                {
                    BringSeparateWord basicWordDoing = new BringSeparateWord();
                    firstWord = basicWordDoing.StemmerPorterBring(firstWord);
                    secondWord = basicWordDoing.StemmerPorterBring(secondWord);
                    if (firstWord.Length > 2 && secondWord.Length > 2)
                    {
                        if (firstWord.Length > secondWord.Length)
                        {
                            firstWord = firstWord.Substring(0, secondWord.Length);
                        }
                        else if (secondWord.Length > firstWord.Length)
                        {
                            secondWord = secondWord.Substring(0, firstWord.Length);
                        }
                    }
                }
                return (firstWord == secondWord);
            }
            else
            {
                return true;
            }
        }
    }
}