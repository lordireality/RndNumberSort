using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RndNumberSort
{
    internal class Program
    {
        /// <summary>
        /// "магические" размеры массивов из задания
        /// </summary>
        public static int[] _possibleSizes = new int[] { 12, 24, 48, 96, 192, 384, 768 };

        /// <summary>
        /// Основной метод работы программы
        /// </summary>
        /// <param name="args">Аргументы для запуска</param>
        static void Main(string[] args)
        {
            while(true)
            {
                Console.WriteLine(getCredentials());
                Console.WriteLine(getPrettyBox("Выберите тип для сортировки: 1 - Вставка, 2 - Отбор"));
                InputController("Ваш выбор:",new Dictionary<string, string>() { { "typeof","int" }, { "minVal","0" }, { "maxVal","3" } });

                Console.ReadLine();
            }
        }


        public static void InputController(string promt, Dictionary<string,string> validationRules)
        {
            Console.Write(promt);
            string input = Console.ReadLine();
            var validatorInstance = new Validator(input, validationRules);
            
            return;
        }




        /// <summary>
        /// Получить массив необходимого размера
        /// </summary>
        /// <param name="sizeOfArray">Размер массива - (0 < размер)</param>
        /// <param name="floor">Мин. значение генерируемых чисел</param>
        /// <param name="limit">Макс. значение генерируемых чисел</param>
        /// <returns>Сгенерированных массив</returns>
        public static int[] getArray(int sizeOfArray, int floor = -100, int limit=100)
        {
            var _rndGenerator = new Random();
            var buff = new int[sizeOfArray];
            for(int i =0;i<sizeOfArray;i++)
            {
                buff[i]= _rndGenerator.Next(floor, limit);
            }
            return buff;
        }

        /// <summary>
        /// Копирайт работы в CMD
        /// </summary>
        /// <returns>текст</returns>
        public static string getCredentials()
        {
            return 
            "┌────────────────────────────────────────────────────────────────────────────────────────────────────┐"+ "\n"+
            "│Задание №2 для технологии программирования, вариант - 21                                            │"+ "\n" +
            "│Выполнил студент гр. з3530902/20001 Зыкин Герман Леонидович                                         │"+ "\n" +
            "│Для выхода в самое начало программы - в качестве значения переменной достаточно написать - 'Сначала'│"+ "\n" +
            "│Для выхода из программы - команда 'Выход'                                                           │"+ "\n" +
            "└────────────────────────────────────────────────────────────────────────────────────────────────────┘";
        }
        /// <summary>
        /// Красивая коробочка для текста
        /// </summary>
        /// <param name="promt">Текст вписываемый в коробочку</param>
        /// <returns></returns>
        public static string getPrettyBox(string promt)
        {
            int spacesToAdd = 100 - promt.Length;
            return
            "┌────────────────────────────────────────────────────────────────────────────────────────────────────┐" + "\n" +
            "│" + promt + new String(' ',spacesToAdd) + "│" + "\n" +
            "└────────────────────────────────────────────────────────────────────────────────────────────────────┘";
        }
    }

    /// <summary>
    /// Класс - валидатор для вводимых значений
    /// </summary>
    public class Validator
    {
        /// <summary>
        /// Строка для валидации
        /// </summary>
        private string strToValidate;
        /// <summary>
        /// Правила валидации
        /// </summary>
        private Dictionary<string, string> validationRules;
        /// <summary>
        /// Сообщения об ошибках валидации, если такие имеются
        /// </summary>
        public string[] messages;
        /// <summary>
        /// Является ли значение валидным
        /// </summary>
        private bool valid = true;

        /// <summary>
        /// Класс конструктор валидатора
        /// </summary>
        /// <param name="strToValidate">Строка, которую необходимо проверить</param>
        /// <param name="validationRules">Правила валидации</param>
        public Validator(string strToValidate, Dictionary<string, string> validationRules)
        {
            this.strToValidate = strToValidate;
            this.validationRules = validationRules;
            this.Validate();
        }
        private void Validate()
        {
            foreach (var rule in this.validationRules)
            {
                switch (rule.Key)
                {
                    case "typeof": valid = ((typeOfCast(rule.Value) == false) ? false : valid);  break;
                    case "minVal": break; //доделать
                    case "maxVal": break; //доделать
                    default: Console.Write("[Ошибка!]Незивестное правило валидации - " + rule.Key); break;
                }
            }
        }
        private bool typeOfCast(string type)
        {
            int intVal = 0; // костыль для TryParse
            switch(type)
            {
                case "int": return Int32.TryParse(this.strToValidate, out intVal);
                case "string": return true; //Емаё ввод с клавы и так стринг, костыль
                default: return false; //не знаем что за тип, возвращаем нет 
            }
        }
    }


    /// <summary>
    /// Базовый класс сортировщиков массивов
    /// </summary>
    public class SortBase
    {
        private int[] inputArray;
        private int[] tempArray;
        private int[] outputArray;
        private bool sortExecuted = false;
        /// <summary>
        /// Класс конструктор
        /// </summary>
        /// <param name="inputArray">Массив для сортировки</param>
        public SortBase(int[] inputArray)
        {
            this.inputArray = inputArray;
            this.tempArray = inputArray;
        }
        /// <summary>
        /// Получить вх. данные для данного экземпляра
        /// </summary>
        /// <returns></returns>
        public int[] getInputArray()
        {
            return this.inputArray;
        }
        /// <summary>
        /// Получить выходные данные для данного экземпляра
        /// Если не была произведена сортировка, будет выдано исключение
        /// </summary>
        /// <returns>Отсортированный массив</returns>
        public int[] getOutputArray()
        {
            if(sortExecuted) { return this.outputArray; } else
            {
                throw new NullReferenceException();
            }
            
        }
        /// <summary>
        /// Выполнение сортировки
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Run()
        {
            throw new NotImplementedException();
        }
    }


    /*public class InsertionSort : SortBase
    {
        
        public void Run()
        {
            int x;
            int j;
            for (int i = 1; i < this.arrayToSort.Length; i++)
            {
                x = arrayToSort[i];
                j = i;
                while (j > 0 && arrayToSort[j - 1] > x)
                {
                    int temp = array[i];
                    array[i] = array[j];
                    array[j] = temp;
                    Swap(arrayToSort, j, j - 1);
                    j -= 1;
                }
                arrayToSort[j] = x;
            }
        }
    }*/
}
