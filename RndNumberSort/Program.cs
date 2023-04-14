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
        /// Основной метод работы программы
        /// </summary>
        /// <param name="args">Аргументы для запуска</param>
        static void Main(string[] args)
        {
            while(true)
            {
                Console.WriteLine(getCredentials());
                Console.WriteLine(getPrettyBox("Выберите тип для сортировки: 1 - Вставка, 2 - Отбор"));
                var SortTypeChoice = InputController("Ваш выбор:",new Dictionary<string, string>() { { "typeof","int" }, { "minVal","0" }, { "maxVal","3" } });

                Console.ReadLine();
            }
        }

        /// <summary>
        /// Контроллер ввода значений
        /// </summary>
        /// <param name="promt">Текст подсказки</param>
        /// <param name="validationRules">Правила валидации</param>
        /// <returns></returns>
        public static KeyBoardInput_DTO InputController(string promt, Dictionary<string,string> validationRules)
        {
            Console.Write(promt);
            string input = Console.ReadLine();
            var validatorInstance = new Validator(input, validationRules);
            if (!validatorInstance.isValid())
            {
                foreach(var message in validatorInstance.messages)
                {
                    Console.WriteLine(message);
                }
                
            }
            var keyBoardInput_DTO = new KeyBoardInput_DTO()
            {
                succed = validatorInstance.isValid(),
                value = input
            };
            return keyBoardInput_DTO;
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

    public class MagicStuffClass
    {        
        /// <summary>
        /// "магические" размеры массивов из задания
        /// </summary>
        public static int[] _possibleSizes = new int[] { 12, 24, 48, 96, 192, 384, 768 };

        /// <summary>
        /// Получить массив с размером из "магических массивов", по его идентификатору
        /// </summary>
        /// <param name="idOfArray">Идентификатор "магического" размера</param>
        /// <param name="floor">Мин. значение генерируемых чисел</param>
        /// <param name="limit">Макс. значение генерируемых чисел</param>
        /// <returns></returns>
        public static int[] getMagicArray(int idOfArray, int floor = -100, int limit = 100)
        {
            return getArray(_possibleSizes[idOfArray], floor, limit);
        }

        /// <summary>
        /// Получить массив необходимого размера
        /// </summary>
        /// <param name="sizeOfArray">Размер массива - (0 < размер)</param>
        /// <param name="floor">Мин. значение генерируемых чисел</param>
        /// <param name="limit">Макс. значение генерируемых чисел</param>
        /// <returns>Сгенерированных массив</returns>
        public static int[] getArray(int sizeOfArray, int floor = -100, int limit = 100)
        {
            var _rndGenerator = new Random();
            var buff = new int[sizeOfArray];
            for (int i = 0; i < sizeOfArray; i++)
            {
                buff[i] = _rndGenerator.Next(floor, limit);
            }
            return buff;
        }
    }

    /// <summary>
    /// Класс для хранения и передачи информации о вводе с клавиатуры и его успешности
    /// </summary>
    public class KeyBoardInput_DTO
    {
        /// <summary>
        /// Успешность ввода
        /// </summary>
        public bool succed = false;
        /// <summary>
        /// Введенное значение
        /// </summary>
        public string value = "";
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
        public List<string> messages = new List<string>();
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
        /// <summary>
        /// Возвращает значение валидации
        /// </summary>
        /// <returns>True/False В зависимости от валидности значений</returns>
        public bool isValid()
        {
            return this.valid;
        }
        private void Validate()
        {
            foreach (var rule in this.validationRules)
            {
                switch (rule.Key)
                {
                    case "typeof": valid = ((typeOfCast(rule.Value) == false) ? false : valid);  break;
                    case "minVal": valid = ((minValCheck(rule.Value) == false) ? false : valid); break;
                    case "maxVal": valid = ((maxValCheck(rule.Value) == false) ? false : valid); break; 
                    default: Console.Write("[Ошибка!]Незивестное правило валидации - " + rule.Key); break;
                }
            }
        }
        /// <summary>
        /// Валидация типа данных
        /// </summary>
        /// <param name="type">Тип</param>
        /// <returns>true/false в зависимости от успешности валидации</returns>
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
        /// <summary>
        /// Валидация "пола" Int32 значений
        /// </summary>
        /// <param name="valToCheck">Сравниваемое значение</param>
        /// <returns>true/false в зависимости от успешности валидации</returns>
        private bool minValCheck(string valToCheck)
        {
            int intVal = 0;
            int checkVal = 0;
            Int32.TryParse(valToCheck, out checkVal);
            bool casted = Int32.TryParse(this.strToValidate, out intVal);
            
            if (casted)
            {
                if(checkVal < intVal)
                {
                    return true;
                } else
                {
                    this.messages.Add("Введенное значение находилось в недопустимом диапазоне!");
                    return false;
                }
            } else
            {
                this.messages.Add("Введенное значение имеет недопустимый тип!");
                return false;
            }
            
        }
        /// <summary>
        /// Валидация "потолка" Int32 значений
        /// </summary>
        /// <param name="valToCheck">Сравниваемое значение</param>
        /// <returns>true/false в зависимости от успешности валидации</returns>
        private bool maxValCheck(string valToCheck)
        {
            int intVal = 0;
            int checkVal = 0;
            Int32.TryParse(valToCheck, out checkVal);
            bool casted = Int32.TryParse(this.strToValidate, out intVal);

            if (casted)
            {
                if (checkVal > intVal)
                {
                    return true;
                }
                else
                {
                    this.messages.Add("Введенное значение находилось в недопустимом диапазоне!");
                    return false;
                }
            }
            else
            {
                this.messages.Add("Введенное значение имеет недопустимый тип!");
                return false;
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
