using NUnit.Framework;
using System.Collections.Generic;

namespace WordleUnitTest
{
    public class Tests
    {

        [Test]
        public void TestTeñ()
        {
            bool result = Wordle.WordleAgustinGaribotto.NoTeAccentÑÇ("piañs");
            Assert.That(result, Is.EqualTo(false));
        }
        [Test]
        public void TestTeç()
        {
            bool result2 = Wordle.WordleAgustinGaribotto.NoTeAccentÑÇ("peçes");
            Assert.That(result2, Is.EqualTo(false));
        }
        [Test]
        public void TestTeAccents()
        {
            bool result3 = Wordle.WordleAgustinGaribotto.NoTeAccentÑÇ("àèìòùáéíóú");
            Assert.That(result3, Is.EqualTo(false));
        }

        [Test]
        public void TestGetList() // Como no pueden llegar parametros que no sean 1,2 false,true o un array de listas con menos listas no las compruebo
        {
            List<string> list1 = new List<string>() { "llista cat fcil" };
            List<string> list2 = new List<string>() { "llista cat difcil" };
            List<string> list3 = new List<string>() { "llista es fcil" };
            List<string> list4 = new List<string>() { "llista es difcil" };

            List<string>[] llistes = { list1, list2, list3, list4 };

            List<string> result = Wordle.WordleAgustinGaribotto.GetList("1", false, llistes);
            CollectionAssert.AreEqual(list1, result);
            List<string> result2 = Wordle.WordleAgustinGaribotto.GetList("1", true, llistes);
            CollectionAssert.AreEqual(list2, result2);
            List<string> result3 = Wordle.WordleAgustinGaribotto.GetList("2", false, llistes);
            CollectionAssert.AreEqual(list3, result3);
            List<string> result4 = Wordle.WordleAgustinGaribotto.GetList("2", true, llistes);
            CollectionAssert.AreEqual(list4, result4);
        }
        [Test]
        public void TestGeneratesWordInsideGivenList()
        {
            List<string> listWords = new List<string>() { "hola", "adios", "pedro", "sioque" };
            string result = Wordle.WordleAgustinGaribotto.WordGenerator(listWords);
            Assert.That(listWords, Does.Contain(result));
        }
        /// <summary>
        /// Testeja que si hi ha dues lletres iguals a wordToGuess per exemple "a", i l'usuari introdueix una paraula
        /// amb dues "a" i cap esta en la posici correcta, li mostri ambdes en groc(1);
        /// aparici ha de ser en gris(0).
        /// </summary>
        [Test]
        public void TestWordFeedbackAllMissPlaced()
        {
            // 2 correcte, 1 posici incorrecta, 0 no est
            int[] feedback = { 0, 0, 1, 0, 1 };
            int[] result = Wordle.WordleAgustinGaribotto.WordFeedback("XXAXA", "BANAL");
            CollectionAssert.AreEqual(feedback, result);
        }
        /// <summary>
        /// La segunda B ha de aparecer en rojo ya que ya ha sido acertada y no hay ms
        /// Y la segunda A ha de aparecer en naranja ya que sigue quedando una.
        /// </summary>
        [Test]
        public void TestWordFeedbackTwoEqualLetters()
        {
            int[] feedback = { 2, 0, 0, 0, 1 };
            int[] result = Wordle.WordleAgustinGaribotto.WordFeedback("BXBXA", "BANAL");
            CollectionAssert.AreEqual(feedback, result);
        }
        /// <summary>
        /// En cas d'haver una paraula com "AMARA" amb tres lletres iguals, el output debera corresponder
        /// I l'usuari introdueix una paraula amb 3 A sense importar la posici:
        /// si encerta totes, i ha possat A de ms, les que no estn en posici correcta sern GRIS
        /// si encerta 2 l'altra a haur d'estar en groc
        /// si encerta 1 les altres dues estarn en groc
        /// es impossible no encertar cap si l'usuari introdueix 3 AAA sense importar la posici
        /// </summary>
        [Test]
        public void TestWordFeedbackThreeEqualLetters()
        {
            int[] feedback = { 2, 1, 0, 1, 0 };
            int[] result = Wordle.WordleAgustinGaribotto.WordFeedback("AAXAX", "AMARA");
            CollectionAssert.AreEqual(feedback, result);
        }
        [Test]
        public void TestWordFeedbackThreeEqualLetters2()
        {
            int[] feedback = { 2, 0, 2, 0, 2 };
            int[] result = Wordle.WordleAgustinGaribotto.WordFeedback("AAAAA", "AMARA");
            CollectionAssert.AreEqual(feedback, result);
        }
        [Test]
        public void TestShowLostCat()
        {
            string result = Wordle.WordleAgustinGaribotto.ShowLostVictory("1", false, "PECAS");
            Assert.That(result, Is.EqualTo("DERROTA"));
        }
        [Test]
        public void TestShowLostEs()
        {
            string result = Wordle.WordleAgustinGaribotto.ShowLostVictory("2", false, "PECAS");
            Assert.That(result, Is.EqualTo("DERROTA"));
        }
        [Test]
        public void TestShowWinCat()
        {
            string result = Wordle.WordleAgustinGaribotto.ShowLostVictory("1", true, "PECAS");
            Assert.That(result, Is.EqualTo("VICTÒRIA"));
        }
        [Test]
        public void TestShowWinEs()
        {
            string result = Wordle.WordleAgustinGaribotto.ShowLostVictory("2", true, "PECAS");
            Assert.That(result, Is.EqualTo("VICTORIA"));
        }
    }
}
