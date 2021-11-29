using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace AnalizeText
{
    /*
     * Класс, предназначенный для усечения окончаний и суффиксов исходного слова.
     *     Предназначается для удаления ненужных морфем в русскоязычном слове.
     *     Используется при сравнении двух слов.
     *     За основу алгоритма взят алгоритм стемминга Портера.
     */
    public class BringSeparateWord
    {
        /*
         * В строковые константы определены регулярные выражения для удаления ненужных морфем в слове.
         */
         
        // Окончания имен существительных
        private const string NOUNEND = "(енем|иями|ерью|ери|ени|[ая]ми|[ое]й|[ое]ю|[оеая]м|[ое]в|[ая]х|[иь]е|[иь]я|[иь]ю|[ие]и|ий|а|я|о|е|ы|и|у|ю|й|ь)$";
        // Суффиксы имен существительных
        private const string NOUNSUFFIX = "(инств|[оеи]ньк|онок|[ое]ст|тел|отн|овн|[иео]нк|[иея]чк|[чщн]ик|чиц|[ыуюи]шк)$";
        // Окончания имен прилагательных и причастий
        private const string ADJECTIVEEND = "([ое]го|[ое]му|[ыи]ми|[оыие]й|[ыиое]м|[ая]я|[уюое]ю|юю|[оеиы]е|[ыи]х|а|я|о|е|ы|и)$";
        // Суффиксы имен прилагательных
        private const string ADJECTIVESUFFIX = "([ое]ват|ейше|овит|[ое]ньк|[еи]нск|ейш|[чл]ив|[ео]нн|ск|[аяи]н)$";
        // Суффиксы причастий
        private const string PARTICIPLESUFFIX = "(ованн|ован|[аяу]ющ|[ая]ем|[аяи]вш|[аея]нн|[уюая]щ|[еоыи]м|вш|[аяе]н|ш|т)$";
        // Суффиксы деепричастий
        private const string PERFECTIVEGROUNDSUFFIX = "([иыая]вшись|[иыая]вши|вши|[иыая]в)$";
        // Окончания глаголов
        private const string VERBEND= "([аяен]ешь|[аяен]ете|[аяеу]йте|ните|[аяеио]ть|[аяеи]ла|[аяеи]ло|[аяеи]ли|[аяен]ет|[аяен]ем|[аяе]ют|[еи]шь|[еи]те|нут|[аяеи]л|[аяе]ю|[аяе]й|[аяент]и|[тч]ь|л[аои]|ну|[еи]м|[еиуюая]т|л|у|ю|й|и)$";
        // Суффиксы глаголов
        private const string VERBSUFFIX = "([оеыи]ва|ну)$";
        // Постфиксы, используются в глаголах и глагольных частях речи (деепричастиях и причастиях)
        private const string POSTFIX = "(с[яь])$";
        // Модель слова, которую необходимо проверять
        private const string MODELWORD = "^(.*?[аеиоуыэюя])(.*)$";

        /*
         * Функция, реализующая отсечение ненужных морфем от слова.
         * word - входное слово, которое обрабатывается в данной функции.
         */
        public string StemmerPorterBring(string word)
        {
            word = word.ToLower();
            word = word.Replace("ё", "е");
            if (new Regex(MODELWORD).IsMatch(word))
            {
                if (!IsSubject(ref word, PERFECTIVEGROUNDSUFFIX, ""))
                {
                    IsSubject(ref word, POSTFIX, "");
                    if (IsSubject(ref word, ADJECTIVEEND, ""))
                    {
                        if (!IsSubject(ref word, PARTICIPLESUFFIX, ""))
                        {
                            IsSubject(ref word, ADJECTIVESUFFIX, "");
                        }
                        else
                        {
                            IsSubject(ref word, VERBSUFFIX, "");
                        }
                        IsSubject(ref word, "нн$", "н");
                    }
                    else
                    {
                        if (!IsSubject(ref word, VERBEND, ""))
                        {
                            IsSubject(ref word, NOUNEND, "");
                            IsSubject(ref word, NOUNSUFFIX, "");
                        }
                        else
                        {
                            IsSubject(ref word, VERBSUFFIX, "");
                        }

                    }

                }
                else
                {
                    IsSubject(ref word, VERBSUFFIX, "");
                }
            }
            return word;
        }

        /*
         * Функция, предположительно определяющая, к какой части речи относится слово.
         * word - исходное слово.
         * cleaningPattern - регулярное выражение, служащее для поиска и удаления ненужных морфем слова.
         * by - строка, на что меняется удаляемая морфема.
         * Выходные данные - была ли удалена морфема и, следовательно, принадлежит ли данное слово к исходной
         *     части речи.
         */
        private bool IsSubject(ref string word, string cleaningPattern, string by)
        {
            string original = word;
            word = new Regex(cleaningPattern, RegexOptions.ExplicitCapture |RegexOptions.Singleline).Replace(word, by);
            return original != word;
        }
    }
}
